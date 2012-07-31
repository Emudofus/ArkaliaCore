using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Commands
{
    public class CommandParameters
    {
        private List<string> _parameters = new List<string>();

        public string Prefix { get; set; }

        public CommandParameters(string[] parameters)
        {
            this._parameters = parameters.ToList();
            this.Prefix = this._parameters[0];
            this._parameters.RemoveAt(0);
        }

        public string GetFullPameters
        {
            get
            {
                return string.Join(" ", this._parameters);
            }
        }

        public string GetParameter(int index)
        {
            return this._parameters[index];
        }

        public int GetIntParameter(int index)
        {
            return int.Parse(this._parameters[index]);
        }

        public bool GetBoolParameter(int index)
        {
            return bool.Parse(this._parameters[index]);
        }

        public string GetParametersAfter(int index)
        {
            var parameters = new List<string>();
            for (int i = 0; i <= this._parameters.Count - 1; i++)
            {
                if (i > index)
                {
                    parameters.Add(this._parameters[i]);
                }
            }
            return string.Join(" ", parameters);
        }

        public int Lenght
        {
            get
            {
                return this._parameters.Count;
            }
        }
    }
}
