using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaPlay.Object;

namespace NovaPlay.MiniGamesAPI
{

    public enum Teams
    {
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4,
        Orange = 5,
        Purple = 6,
        Pink = 7,
        Gold = 8,
        Grey = 9,
        Black = 10
    }

    public class Team
    {

        public string name;
        public string color;
        public Dictionary<string,GamePlayer> players = new Dictionary<string, GamePlayer>();
        public GameAPI api;
        public int id;
        public bool canRespawn = true;

        public Team(GameAPI api, string name, string color, int id)
        {
            this.api = api;
            this.name = name;
            this.color = color;
            this.id = id;
        }

        public int GetId()
        {
            return this.id;
        }

        public bool CanRespawn()
        {
            return this.canRespawn;
        }

        public string GetName()
        {
            return this.name;
        }

        public string GetColor()
        {
            return this.color;
        }

        public int GetPlayers()
        { 
            return this.players.Count();
        }

        public void AddPlayer(GamePlayer data)
        {
            if (data.GetTeam().GetName() == this.GetName())
            {
                this.players.Add(data.baseData.GetPlayer().Username, data);
            }
        }

        public void RemovePlayer(GamePlayer data)
        {
            this.players.Remove(data.baseData.GetPlayer().Username);
        }


    }
}
