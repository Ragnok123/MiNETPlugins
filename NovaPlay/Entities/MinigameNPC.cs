using System;
using MiNET;
using MiNET.Entities.Passive;
using MiNET.Entities;
using MiNET.Worlds;
using MiNET.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NovaPlay.Object;
using NovaPlay.MiniGamesAPI;
using System.Collections.Generic;

namespace NovaPlay.Entities
{
    public class MinigameNPC : PlayerMob
    {

        public string name;
        public Level level;
        public GameAPI arena;
        public string type;
        public List<GameAPI> games = new List<GameAPI>();
        public string currentId;
        private int ar = 0;

        public MinigameNPC(string name, Level level, string type) : base(name, level)
        {
            this.name = name;
            this.type = type;
        }

        public string GetNameTag()
        {
            return this.name;
        }

        public void Init()
        {
            int a = 0;
            this.currentId = games[a].arenaname;
        }

        public void SetNameTag(string name)
        {
            this.name = name;
            NameTag = this.name;
            BroadcastSetEntityData();
        }

        public void ProcessQueue(NovaPlayer player)
        {
            if(games[ar].GetPlayerCount() == games[ar].GetMaxPlayerCount() || games[ar].gameStatus >= 2)
            {
                ++ar;
                this.currentId = games[ar].arenaname;
            }
            if(ar >= games.Count)
            {
                ar = 0;
            }
            games[ar].AddPlayer(player.pData);
        }
        

    }
}
