using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//This is a basic path engine by NightWolf
//for Arkalia Core v0.1

namespace ArkaliaCore.Game.Game.Engines
{
    public class PathEngine
    {
        #region Fields

        /// <summary>
        /// Const hash for getting cells/dir
        /// </summary>
        public const string Hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

        /* Private fields */
        private List<Pathfinding.PathNode> _cells = new List<Pathfinding.PathNode>();
        private Database.Models.MapModel _map { get; set; }

        #endregion

        #region Ctors

        public PathEngine(string path, Database.Models.MapModel map)
        {
            this._map = map;
            this.LoadMoves(path);
        }

        public PathEngine(string path, int sCell, int sDir, Database.Models.MapModel map)
        {
            this._map = map;
            this._cells.Add(new Pathfinding.PathNode(sCell, sDir));//Only for start move path
            this.LoadMoves(path);
        }

        public PathEngine(List<Pathfinding.PathNode> cells, Database.Models.MapModel map)
        {
            this._map = map;
            this._cells = cells;
        }

        public PathEngine(Database.Models.MapModel map)
        {
            this._map = map;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load moves from a string
        /// </summary>
        /// <param name="path"></param>
        public void LoadMoves(string path)
        {
            for (int i = 0; i <= path.Length - 1; i += 3)
            {
                try
                {
                    string node = path.Substring(i, 3);
                    string dirchar = node[0].ToString();
                    string cellChar = node.Substring(1);

                    int dir = GetDirNum(dirchar);
                    int cell = GetCellNum(cellChar);

                    if (cell > 0)
                    {
                        this._cells.Add(new Pathfinding.PathNode(cell, dir));
                    }
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error("Can't parse moves : " + e.Message);
                    break;
                }
            }
        }

        /// <summary>
        /// Get final path builded by pathnodes
        /// </summary>
        public string Path
        {
            get
            {
                return string.Join("", this._cells);
            }
        }

        /// <summary>
        /// Get cells from the path gived
        /// </summary>
        public List<Pathfinding.PathNode> Cells
        {
            get
            {
                return this._cells;
            }
        }

        /// <summary>
        /// Get next cell in dir
        /// </summary>
        public int NextCell(int cell, int dir)
        {
            switch (dir)
            {
                case 0:
                    return cell + 1;

                case 1:
                    return cell + this._map.Width;

                case 2:
                    return cell + (this._map.Width * 2) - 1;

                case 3:
                    return cell + this._map.Width - 1;

                case 4:
                    return cell - 1;

                case 5:
                    return cell - this._map.Width;

                case 6:
                    return cell - (this._map.Width * 2) + 1;

                case 7:
                    return cell - this._map.Width + 1;

            }
            return -1;
        }

        /// <summary>
        /// Get node in move by index
        /// </summary>
        public Pathfinding.PathNode GetNode(int index)
        {
            return this._cells[index];
        }

        /// <summary>
        /// Get the last node of this move
        /// </summary>
        public Pathfinding.PathNode LastNode()
        {
            if (this._cells.Count > 0)
            {
                return this._cells.LastOrDefault();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Add node from cellid and dir
        /// </summary>
        public void AddNode(int cellid, int direction)
        {
            this._cells.Add(new Pathfinding.PathNode(cellid, direction));
        }

        /// <summary>
        /// Add node from existing node
        /// </summary>
        public void AddNode(Pathfinding.PathNode node)
        {
            this._cells.Add(node);
        }

        /// <summary>
        /// Get pattern for sending to client move
        /// </summary>
        public string GetPathPattern(int id)
        {
            return "GA0;1;" + id + ";" + this.Path;
        }

        #endregion

        #region Path Converter Methods

        /// <summary>
        /// Get cellID by char
        /// </summary>
        public static int GetCellNum(string CellChars)
        {
            int NumChar1 = Hash.IndexOf(CellChars[0]) * Hash.Length;
            int NumChar2 = Hash.IndexOf(CellChars[1]);
            return NumChar1 + NumChar2;
        }

        /// <summary>
        /// get char by cellid
        /// </summary>
        public static string GetCellChars(int CellNum)
        {
            int CharCode2 = (CellNum % Hash.Length);
            int CharCode1 = (CellNum - CharCode2) / Hash.Length;
            return Hash[CharCode1].ToString() + Hash[CharCode2].ToString();
        }

        /// <summary>
        /// Get dirchar by id
        /// </summary>
        public static string GetDirChar(int DirNum)
        {
            if (DirNum >= Hash.Length)
                return "";
            return Hash[DirNum].ToString();
        }

        /// <summary>
        /// Get dir by chardir
        /// </summary>
        public static int GetDirNum(string DirChar)
        {
            return Hash.IndexOf(DirChar);
        }

        /// <summary>
        /// Return string path with a direction and cellid
        /// </summary>
        public static string CreateNodePath(int dir, int cellid)
        {
            return GetDirChar(dir) + GetCellChars(cellid);
        }

        /// <summary>
        /// Return string path with a node
        /// </summary>
        public static string CreateNodePath(Pathfinding.PathNode node)
        {
            return GetDirChar(node.Dir) + GetCellChars(node.CellID);
        }

        /// <summary>
        /// Check if cells is in the same line
        /// </summary>
        public bool InLine(int cell1, int cell2)
        {
            bool isX = GetCellXCoord(cell1) == GetCellXCoord(cell2);
            bool isY = GetCellYCoord(cell1) == GetCellYCoord(cell2);
            return isX || isY;
        }

        /// <summary>
        /// Get X location for cell
        /// </summary>
        public int GetCellXCoord(int cellid)
        {
            int w = this._map.Width;
            return ((cellid - (w - 1) * GetCellYCoord(cellid)) / w);
        }

        /// <summary>
        /// Get Y location for cell
        /// </summary>
        public int GetCellYCoord(int cellid)
        {
            int w = this._map.Width;
            int loc5 = (int)(cellid / ((w * 2) - 1));
            int loc6 = cellid - loc5 * ((w * 2) - 1);
            int loc7 = loc6 % w;
            return (loc5 - loc7);
        }

        /// <summary>
        /// Get direction of cell with other cell
        /// </summary>
        public int GetDirection(int Cell, int Cell2)
        {

            int MapWidth = _map.Width;

            int[] ListChange = {
		            1,
		            MapWidth,
		            MapWidth * 2 - 1,
		            MapWidth - 1,
		            -1,
		            -MapWidth,
		            -MapWidth * 2 + 1,
		            -(MapWidth - 1)
	                            };

            dynamic Result = Cell2 - Cell;

            for (int i = 7; i >= 0; i += -1)
            {
                if (Result == ListChange[i])
                    return i;
            }

            int ResultX = GetCellXCoord(Cell2) - GetCellXCoord(Cell);
            int ResultY = GetCellYCoord(Cell2) - GetCellYCoord(Cell);

            if (ResultX == 0)
            {
                if (ResultY > 0)
                    return 3;
                return 7;
            }
            else if (ResultX > 0)
            {
                return 1;
            }
            else
            {
                return 5;
            }
        }

        #endregion
    }
}