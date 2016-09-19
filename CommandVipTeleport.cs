using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace VipTeleport
{
    class CommandVipTeleport : IRocketCommand
    {
        public static string syntax = "";
        public static string help = "Use this Command if you're VIP and have died in the last 120 seconds!";
        public float rotationD = 120;


        public List<string> Aliases
        {
            get
            {
                return new List<string>();
            }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return (AllowedCaller)1;
            }
        }

        public string Help
        {
            get
            {
                return CommandVipTeleport.help;
            }
        }

        public string Name
        {
            get
            {
                return "back";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>()
                {
                    "viptele"
                };
            }
        }

        public string Syntax
        {
            get
            {
                return CommandVipTeleport.syntax;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {

            for (int i = 0; i < VipTeleport.Instance.playerSteamID.Length; i++)
            {
                UnturnedPlayer Pcaller = (UnturnedPlayer)caller;
                string converter = Pcaller.CSteamID.ToString();
                int steamIDLocal = (int)Int64.Parse(converter);

                if (VipTeleport.Instance.playerSteamID[i] == steamIDLocal)
                {
                    if (command.Length > 1)
                    {
                        UnturnedChat.Say(caller, VipTeleport.Instance.DefaultTranslations.Translate("vip_help"));
                    }
                    else
                    {
                        if (VipTeleport.Instance.playersCanTeleport[i])
                        {
                            Logger.LogWarning(Pcaller.SteamName + " Used VIP teleport");
                            Pcaller.Teleport(VipTeleport.Instance.deathPos[i], rotationD);
                            VipTeleport.Instance.playersCanTeleport[i] = false;
                        }
                        else
                        {
                            DateTime now = DateTime.Now;
                            if ((DateTime.Compare(now, VipTeleport.Instance.timerDone[i])) > 0)
                            {
                                UnturnedChat.Say(caller, "Unable to teleport as it has been more than 2 minutes!");
                                break;
                            }
                            else if (((DateTime.Compare(VipTeleport.Instance.playerDeathTime[i], VipTeleport.Instance.timerDone[i])) < 0) && (!VipTeleport.Instance.playersCanTeleport[i]))
                            {
                                UnturnedChat.Say(caller, "You have already used this command please wait the two minutes!");
                                break;
                            }
                            else
                            {
                                UnturnedChat.Say(caller, "You haven't recently died, please use this command after you die!");
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
}
