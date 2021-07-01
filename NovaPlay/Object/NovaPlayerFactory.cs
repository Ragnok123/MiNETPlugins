using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using NovaPlay;
using System.Net;

namespace NovaPlay.Object
{
    public class NovaPlayerFactory : PlayerFactory
    {
        public NovaCore novacore;

        public override Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, PlayerInfo playerInfo)
        {
            var player = new NovaPlayer(server, endPoint);
            player.HealthManager = new HealthManager(player);
            player.HungerManager = new HungerManager(player);
            OnPlayerCreated(new PlayerEventArgs(player));
            return player;
        }


    }
}
