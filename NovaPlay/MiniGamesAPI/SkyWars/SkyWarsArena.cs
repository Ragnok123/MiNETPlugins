using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities.Projectiles;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovaPlay;
using NovaPlay.Object;
using NovaPlay.MiniGamesAPI;


namespace NovaPlay.MiniGamesAPI.SkyWars
{
    public class SkyWarsArena : GameAPI
    {

        public NovaCore novacore;

        public int signX;
        public int signY;
        public int signZ;

        public SkyWarsTimer timermanager;

        public SkyWarsArena(NovaCore novacore, string arenaname, string worldname,string gameId, Dictionary<string, PlayerLocation> arenadata)
        {
            this.novacore = novacore;
            this.arenaname = arenaname;
            this.worldname = worldname;
            this.arenadata = arenadata;
            this.gameType = "SkyWars";
            timermanager = new SkyWarsTimer(this);
            this.gamePrefix = "§l§b[§cSky§bWars]";
        }
        

        public void GameTimer()
        {

            if (this.gameStatus == 0)
            {
                if(GetPlayerCount() > 1)
                {
                    this.lastTime = this.waitTime;
                    this.gameStatus = 1;
                }
            }
            if(this.gameStatus == 1)
            {
                this.lastTime--;
                switch (this.lastTime)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    case 44:
                    case 45:
                    case 46:
                    case 47:
                    case 48:
                    case 49:
                    case 50:
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                    case 56:
                    case 57:
                    case 58:
                    case 59:
                    case 60:
                        foreach(GamePlayer data in this.arenaplayers.Values)
                        {
                            data.baseData.GetPlayer().AddPopup(new Popup
                            {
                                Message = "§aGame starts in §b" +this.lastTime + " §aseconds..."
                            });
                        }
                    break;

                }

            }
        }


    }
}
