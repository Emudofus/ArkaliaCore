using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArkaliaCore.Game.Game.Handlers;

namespace ArkaliaCore.Game.Network.Game
{
    public static class GameHandler
    {
        public static void HandlePacket(this GameClient client, string packet)
        {
            switch(packet[0])
            {
                case 'A'://Account packet
                    switch (packet[1])
                    {
                        case 'A'://Character creation
                            AccountHandler.HandleCharacterCreationRequest(client, packet);
                            break;

                        case 'B'://Boost stat
                            StatHandler.HandleBoostStat(client, packet);
                            break;

                        case 'T'://Ticket checking
                            AccountHandler.HandleTicket(client, packet);
                            break;

                        case 'P'://Random name request
                            AccountHandler.HandleRandomNameCharacter(client, packet);
                            break;

                        case 'D'://Character deletion
                            AccountHandler.HandleCharacterDeletionRequest(client, packet);
                            break;

                        case 'L'://Characters list
                            AccountHandler.SendCharactersList(client);
                            break;

                        case 'S'://Character selection
                            AccountHandler.HandleSelectCharacter(client, packet);
                            break;
                    }
                    break;

               case 'B'://Basic packet
                    switch (packet[1])
                    {
                        case 'A'://Console message
                            CommandHandler.HandleConsoleCommand(client, packet);
                            break;
                            
                        case 'M'://Chat message
                            ChatHandler.HandleMessageRequest(client, packet);
                            break;

                        case 'S'://Smiley
                            EmoteHandler.HandleDoSmiley(client, packet);
                            break;
                    }
                    break;

                case 'D'://Dialog
                    switch (packet[1])
                    {
                        case 'C'://Create dialog
                            NpcHandler.HandleRequestDialog(client, packet);
                            break;

                        case 'R'://Dialog response
                            NpcHandler.HandleDialogResponse(client, packet);
                            break;

                        case 'V'://Dialog leave
                            NpcHandler.HandleDialogLeave(client, packet);
                            break;
                    }
                    break;

                case 'e'://Environment packet
                    switch (packet[1])
                    {
                        case 'U'://Do emote
                            EmoteHandler.HandleDoEmote(client, packet);
                            break;
                    }
                    break;

                case 'F'://Communauty packet
                    switch (packet[1])
                    {
                        case 'L'://Friends list
                            FriendHandler.HandleRequestFriends(client, packet);
                            break;

                        case 'D'://Delete friend
                            FriendHandler.HandleDeleteFriend(client, packet);
                            break;

                        case 'A'://Add friend
                            FriendHandler.HandleAddFriend(client, packet);
                            break;
                    }
                    break;

               case 'G'://Game packet
                    switch (packet[1])
                    {
                        case 'A'://Game action
                            GameActionHandler.HandleGameAction(client, packet);
                            break;

                        case 'K':
                            switch (packet[2])
                            {
                                case 'E'://Change move
                                    GameActionHandler.RequestChangeMove(client, packet);
                                    break;

                                case 'K'://End move
                                    GameActionHandler.RequestEndMove(client);
                                    break;
                            }
                            break;

                        case 'C'://Game create
                            GameMapHandler.HandleGameCreate(client, packet);
                            break;

                        case 'I'://Game informations
                            GameMapHandler.HandleGameInformationsRequest(client, packet);
                            break;
                    }
                    break;
                    
                case 'O'://Objects
                    switch (packet[1])
                    {
                        case 'd'://Delete object
                            ItemHandler.HandleDeleteItem(client, packet);
                            break;

                        case 'M'://Move object
                            ItemHandler.HandleMoveItem(client, packet);
                            break;
                    }
                    break;

                case 'W'://Waypoints
                    switch (packet[1])
                    {
                        case 'U'://Use waypoint
                            WaypointHandler.HandleUseWaypoint(client, packet);
                            break;

                        case 'V'://Leave waypoint
                            WaypointHandler.HandleCloseWaypoint(client, packet);
                            break;
                    }
                    break;
            }
        }
    }
}
