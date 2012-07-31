using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Database.Models
{
    public class CharacterModel
    {
        public CharacterModel()
        {
            this.Bag = new Game.Items.CharacterBag(this);
        }

        public int ID { get; set; }
        public int Owner { get; set; }
        public string Nickname { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }
        public int Look { get; set; }
        public int Scal { get; set; }
        public int Gender { get; set; }
        public int Breed { get; set; }
        public int Color1 { get; set; }
        public int Color2 { get; set; }
        public int Color3 { get; set; }
        public int MapID { get; set; }
        public int CellID { get; set; }
        public int Direction { get; set; }
        public int Kamas { get; set; }
        public int CurrentLife { get; set; }
        public int Life { get; set; }
        public int Wisdom { get; set; }
        public int Strenght { get; set; }
        public int Fire { get; set; }
        public int Agility { get; set; }
        public int Water { get; set; }
        public int CaractPoint { get; set; }
        public int SpellPoint { get; set; }
        public int SaveMap { get; set; }
        public int SaveCell { get; set; }
        public string OwnZaaps { get; set; }
        public string OwnSpells { get; set; }
        public int FactionID { get; set; }
        public int FactionPower { get; set; }
        public int FactionHonor { get; set; }
        public int FactionDeshonor { get; set; }
        public int FactionEnabled { get; set; }
        public int GuildID { get; set; }
        public int GuildRank { get; set; }
        public string GuildRights { get; set; }
        public int MountID { get; set; }
        public int RideMount { get; set; }
        public int TitleID { get; set; }
        public int EliteLevel { get; set; }
        public string EmoteList { get; set; }
        public string JobsList { get; set; }

        #region NoDB Fields

        public List<int> Emotes = new List<int>();
        public List<Game.Jobs.JobInstance> Jobs = new List<Game.Jobs.JobInstance>();
        public Game.Stats.BasicStatistics Stats = new Game.Stats.BasicStatistics();
        public List<Database.Models.WaypointModel> Waypoints = new List<WaypointModel>();
        public Game.Items.CharacterBag Bag { get; set; }
        public Network.Game.GameClient ClientLink { get { return Game.World.GetClient(this.Nickname); } }

        #endregion

        #region Dynamic fields

        public MapModel Map
        {
            get
            {
                return Game.Controllers.MapController.GetMap(this.MapID);
            }
        }

        public int MaxLife
        {
            get
            {
                return this.GetBreed.StartLife + Stats.GetField(Enums.StatsTypeEnum.LIFE).Total + (5 * Level);
            }
        }

        public int Initiative
        {
            get
            {
                return 1000 + (this.MaxLife * 2);
            }
        }

        public BreedModel GetBreed
        {
            get
            {
                return Game.Breeds.BreedManager.GetBreed(this.Breed);
            }
        }

        public int EmoteClient
        {
            get
            {
                lock (Emotes)
                {
                    var value = 0;
                    Emotes.ForEach(x => value += x);
                    return value;
                }
            }
        }

        public ExpFloorModel LevelFloor
        {
            get
            {
                return Game.Floors.FloorsManager.GetFloorByLevel(this.Level);
            }
        }

        public int MaxPods
        {
            get
            {
                return 1000 + (this.Stats.GetField(Enums.StatsTypeEnum.STRENGHT).Total * 2);
            }
        }

        public bool IsOverpods
        {
            get
            {
                return this.Bag.BagWeight > this.MaxPods;
            }
        }

        #endregion

        #region Methods

        public void UpdateStatsFields()
        {
            Stats.GetField(Enums.StatsTypeEnum.LIFE).Basic = Life;
            Stats.GetField(Enums.StatsTypeEnum.WISDOM).Basic = Wisdom;
            Stats.GetField(Enums.StatsTypeEnum.STRENGHT).Basic = Strenght;
            Stats.GetField(Enums.StatsTypeEnum.FIRE).Basic = Fire;
            Stats.GetField(Enums.StatsTypeEnum.WATER).Basic = Water;
            Stats.GetField(Enums.StatsTypeEnum.AGILITY).Basic = Agility;
            Stats.GetField(Enums.StatsTypeEnum.AP).Basic = 6 + (this.Level >= 100 ? 1 : 0);
            Stats.GetField(Enums.StatsTypeEnum.MP).Basic = 3;
        }

        public int GetStat(Enums.StatsTypeEnum sType, out int value)
        {
            switch (sType)
            {
                case Enums.StatsTypeEnum.LIFE:
                    return value = Life;

                case Enums.StatsTypeEnum.WISDOM:
                    return value = Wisdom;

                case Enums.StatsTypeEnum.STRENGHT:
                    return value = Strenght;

                case Enums.StatsTypeEnum.FIRE:
                    return value = Fire;

                case Enums.StatsTypeEnum.WATER:
                    return value = Water;

                case Enums.StatsTypeEnum.AGILITY:
                    return value = Agility;

                default:
                    return value = 0;
            }
        }

        public void Save()
        {
            Database.Tables.CharacterTable.Save(this);
        }

        public bool HaveWaypoint(int mapid, int cellid)
        {
            return Waypoints.FindAll(x => x.MapID == mapid && x.CellID == cellid).Count > 0;
        }

        #endregion

        #region DB Pattern

        public string EmotesString
        {
            get
            {
                lock (Emotes)
                {
                    return string.Join(",", Emotes);
                }
            }
        }

        public string JobsString
        {
            get
            {
                return string.Join(";", this.Jobs);
            }
        }

        public string WaypointString
        {
            get
            {
                return string.Join(",", this.Waypoints);
            }
        }

        #endregion

        #region Patterns

        public string SelectionPattern
        {
            get
            {
                StringBuilder pattern = new StringBuilder();
                pattern.Append(ID).Append(";").Append(Nickname).Append(";").Append(Level)
                    .Append(";").Append(Look).Append(";").Append(Color1.ToString("x")).Append(";").Append(Color2.ToString("x"))
                    .Append(";").Append(Color3.ToString("x")).Append(";").Append(this.Bag.DisplayBagOnCharacter).Append(";0;1;0;0;");
                return pattern.ToString();
            }
        }

        public string SelectedPattern
        {
            get
            {
                StringBuilder pattern = new StringBuilder();
                pattern.Append(ID).Append("|").Append(Nickname).Append("|").Append(Level).Append("|")
                .Append(Breed).Append("|").Append(Look).Append("|").Append(Color1)
                .Append("|").Append(Color2).Append("|").Append(Color3).Append("||").Append(this.Bag.DisplayBagConnectionPattern).Append("|");
                return pattern.ToString();
            }
        }

        public string ShowPattern
        {
            get
            {
                StringBuilder pattern = new StringBuilder();
    //            pattern.Append(CellID).Append(";").Append(Direction).Append(";0;").Append(ID).Append(";")
    //.Append(Nickname).Append(";").Append(Breed.ToString())
    //.Append(TitleID > 0 ? "," + TitleID : "").Append(";").Append(Look).Append("^").Append(Scal).Append(";")
    //.Append(Gender).Append(";").Append(Faction.Wings).Append(",").Append((ID + Level).ToString()).Append(";")
    //.Append(Color1.ToString("x")).Append(";").Append(Color2.ToString("x")).Append(";")
    //.Append(Color3.ToString("x")).Append(";")
    //.Append(Items.DisplayItem()).Append(";" + Aura + ";;;" + GuildInfos + ";0;" + ShowMount + ";");
                pattern.Append(CellID).Append(";").Append(Direction).Append(";0;").Append(ID).Append(";").Append(Nickname).Append(";")
                    .Append(Breed).Append("").Append(";").Append(Look).Append("^").Append(Scal).Append(";").Append(Gender).Append(";")
                    .Append("0,0,0,").Append((Level + ID).ToString()).Append(";")
                    .Append(Color1.ToString("x")).Append(";")
                    .Append(Color2.ToString("x")).Append(";")
                    .Append(Color3.ToString("x")).Append(";")
                    .Append(this.Bag.DisplayBagOnCharacter).Append(";0;;;;0;;");
                return pattern.ToString();
            }
        }

        public string StatsPattern
        {
            get
            {
                StringBuilder pattern = new StringBuilder();
                pattern.Append(this.Experience + "," + this.LevelFloor.Character + "," + (Game.Floors.FloorsManager.GetNextFloor(this.Level) != null ? Game.Floors.FloorsManager.GetNextFloor(this.Level).Character.ToString() : "-1"));//TODO: Exp !
                pattern.Append("|" + Kamas + "|" + CaractPoint + "|" + SpellPoint);
                pattern.Append("|,,,");
                pattern.Append("|" + CurrentLife + "," + MaxLife);
                pattern.Append("|10000,10000");
                pattern.Append("|0|0");
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.AP).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.MP).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.STRENGHT).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.LIFE).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.WISDOM).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.WATER).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.AGILITY).ToString());
                pattern.Append("|" + Stats.GetField(Enums.StatsTypeEnum.FIRE).ToString());
                return pattern.ToString();
            }
        }

        #endregion
    }
}
