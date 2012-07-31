using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class FindCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "find";
            }
        }

        public override int AccessLevel
        {
            get
            {
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "Commande de recherche";
            }
        }

        public override bool NeedLoaded
        {
            get
            {
                return true;
            }
        }

        public override void Execute(Network.Game.GameClient client, CommandParameters parameters)
        {
            if(parameters.Lenght > 0)
            {
                var searchType = parameters.GetParameter(0).ToLower();
                switch (searchType)
                {
                    case "item"://Item finder order by level
                        if (parameters.Lenght > 1)
                        {
                            var itemCriterion = parameters.GetParameter(1);
                            var itemSearched = new StringBuilder();
                            foreach (var item in Database.Tables.ItemTemplateTable.Cache.Values.
                                ToList().FindAll(x => x.Name.ToLower().Contains(itemCriterion.ToLower())).OrderBy(x => x.Level))
                            {
                                itemSearched.Append(item.Name + "(" + item.ID + ") Niveau : " + item.Level + "<br />");
                            }
                            client.ConsoleMessage("Liste des resultats :", Enums.ConsoleColorEnum.GREEN);
                            client.ConsoleMessage(itemSearched.ToString(), Enums.ConsoleColorEnum.WHITE);
                        }
                        else
                        {
                            client.ConsoleMessage("Parametre invalide, entrer un terme a chercher !", Enums.ConsoleColorEnum.RED);
                        }
                        break;

                    default:
                        client.ConsoleMessage("Terme de recherche '" + searchType + "' invalide", Enums.ConsoleColorEnum.RED);
                        break;
                }
            }
            else
            {
                client.ConsoleMessage("Parametre invalide, entrer un terme a chercher !", Enums.ConsoleColorEnum.RED);
            }
        }
    }
}
