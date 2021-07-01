using NovaPlay.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPlay.MiniGamesAPI
{
    public class GamePlayer
    {

        public GameAPI api;

        public Team team;

        public string killer;

        public PlayerData baseData;

        public GamePlayer(PlayerData data, GameAPI api)
        {
            this.baseData = data;
            this.api = api;
        }

        public bool CanRespawn()
        {
            return this.team.CanRespawn();
        }

        public Team GetTeam()
        {
            return this.team;
        }

        public bool IsInGame()
        {
            if (this.api.arenaplayers.ContainsKey(this.baseData.GetPlayer().Username))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
