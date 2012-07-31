using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkaliaCore.Game.Game.Handlers
{
    public static class ItemHandler
    {
        public static void HandleDeleteItem(Network.Game.GameClient client, string packet)
        {
            var data = packet.Substring(2).Split('|');
            var id = int.Parse(data[0]);
            var quantity = int.Parse(data[1]);

            if (client.Character != null)
            {
                var stack = client.Character.Bag.GetStack(id);
                if (stack != null)
                {
                    client.Character.Bag.Remove(stack, quantity);
                }
            }
        }

        public static void HandleMoveItem(Network.Game.GameClient client, string packet)
        {
            var data = packet.Substring(2).Split('|');
            var id = int.Parse(data[0]);
            var position = int.Parse(data[1]);

            if (client.Character != null)
            {
                var stack = client.Character.Bag.GetStack(id);
                if (stack != null)
                {
                    client.Character.Bag.MoveStack(stack, position);
                }
            }
        }
    }
}
