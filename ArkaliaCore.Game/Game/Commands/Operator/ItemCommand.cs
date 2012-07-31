using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Engines;

namespace ArkaliaCore.Game.Game.Commands.Operator
{
    public class ItemCommand : Command
    {
        public override string Prefix
        {
            get
            {
                return "item";
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
                return "Genere un objet";
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
            if (parameters.Lenght > 0)
            {
                var itemID = parameters.GetIntParameter(0);
                var itemTemplate = Items.ItemManager.GetTemplate(itemID);
                var quantity = 1;
                var style = false;
                if (itemTemplate != null)
                {
                    if (parameters.Lenght > 1)
                    {
                        quantity = parameters.GetIntParameter(1);
                    }
                    if (parameters.Lenght > 2)
                    {
                        var styleStr = parameters.GetParameter(2).ToLower();
                        if (styleStr == "max")
                        {
                            style = true;
                        }
                    }
                    var item = Items.ItemManager.GenerateItem(client.Character.ID, itemTemplate, quantity, style);
                    client.Character.Bag.Add(item);
                    client.ConsoleMessage("L'objet <b>'" +  itemTemplate.Name + "'</b> a correctement ete generer !", Enums.ConsoleColorEnum.GREEN);
                }
                else
                {
                    client.ConsoleMessage("Impossible de trouver l'objet n°" + itemID, Enums.ConsoleColorEnum.RED);
                }
            }
            else
            {
                client.ConsoleMessage("Parametres invalide !", Enums.ConsoleColorEnum.RED);
            }
        }
    }
}
