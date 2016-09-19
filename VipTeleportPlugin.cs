using UnityEngine;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using System;
using Rocket.Unturned.Chat;

namespace VipTeleport
{
    public class VipTeleport : RocketPlugin<VipTeleportConfig>
    {
        public static VipTeleport Instance;
        public int[] playerSteamID = new int[24];
        public bool[] playersCanTeleport = new bool[24];
        public Vector3[] deathPos = new Vector3[24];
        public DateTime[] playerDeathTime = new DateTime[24];
        public DateTime[] timerDone = new DateTime[24];


        protected override void Load()
        {
            VipTeleport.Instance = this;
            Logger.LogWarning("##################################");
            Logger.LogWarning("#Vip Teleport Loaded Sucessfully!#");
            Logger.LogWarning("##################################");
            Logger.LogWarning("#         Version  V.3.0         #");
            Logger.LogWarning("##################################");

            U.Events.OnPlayerConnected += (UnturnedPlayer player) =>
            {
                string playerGrab = player.CSteamID.ToString();
                long addTo = Int64.Parse(playerGrab);
                for (int f = 0; f < playerSteamID.Length; f++)
                {
                    if (playerSteamID[f] == 0)
                    {
                        playerSteamID[f] = (int)addTo;
                        foreach (bool t in playersCanTeleport)
                        {
                            playersCanTeleport[f] = false;
                        }
                        break;
                    }
                }
            };

            UnturnedPlayerEvents.OnPlayerDead += (UnturnedPlayer player, Vector3 Vector3) =>
            {
                string converter = player.CSteamID.ToString();
                int localSteamID = (int)Int64.Parse(converter);
                for (int i = 0; i < playerSteamID.Length; i++)
                {
                    if (playerSteamID[i] == localSteamID)
                    {
                        deathPos[i] = player.Position;
                        playerDeathTime[i] = DateTime.Now;
                        if ((DateTime.Compare(playerDeathTime[i], timerDone[i]) < 0))
                        {
                            UnturnedChat.Say("You will be not be able to use /back until the two minutes cooldown goes away and you have died again after that!");
                        }
                        else
                        {
                            TimeSpan duration = new TimeSpan(0, 0, VipTeleport.Instance.Configuration.Instance.cooldownTime);
                            timerDone[i] = playerDeathTime[i].Add(duration);

                            if ((DateTime.Compare(playerDeathTime[i], timerDone[i]) < 0))
                            {
                                playersCanTeleport[i] = true;
                            }
                            else
                            {
                                playersCanTeleport[i] = false;
                            }
                        }
                        break;
                    }
                }
            };

            U.Events.OnPlayerDisconnected += (UnturnedPlayer player) =>
            {
                for(int i = 0; i < playerSteamID.Length; i++)
                {
                    string converter = player.CSteamID.ToString();
                    int localSteamID = (int)Int64.Parse(converter);
                    if (playerSteamID[i] == localSteamID)
                    {
                        playerSteamID[i] = 0;
                        playersCanTeleport[i] = false;
                        break;
                    }
                }
            };
        }

        protected override void Unload()
        {

        }



        private void FixedUpdate()
        {

        }
    }
}         

