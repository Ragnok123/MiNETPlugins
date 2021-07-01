using System;
using MiNET;
using MiNET.Entities.Passive;
using MiNET.Entities;
using MiNET.Worlds;
using MiNET.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace NovaPlay.MiniGamesAPI.BedWars
{
    public class ShopVillager : PlayerMob
    {

        public string name;
        public Level level;
        public GameAPI arena;
        public string type;

        public ShopVillager(string name, Level level, string type) : base(name, level)
        {
            this.name = name;
            this.type = type;
        }

        public string GetNameTag()
        {
            return this.name;
        }

        public void SetNameTag(string name)
        {
            this.name = name;
            NameTag = this.name;
            BroadcastSetEntityData();
        }
        

    }
}
