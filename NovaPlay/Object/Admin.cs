using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPlay.Object
{
    public class Admin
    {

        public PlayerData player;
        public bool isAdmin = false;
        public bool isTesting = false;

        public Admin(PlayerData data)
        {
            this.player = data;
        }

        public string GetAdminPlace()
        {
            String place = "";
            switch (this.player.GetRank())
            {
                case "owner":
                    place = "owner";
                    break;
                case "admin+":
                    place = "admin+";
                    break;
                case "admin":
                    place = "admin";
                    break;
                case "dev":
                    place = "dev";
                    break;
                case "builder":
                    place = "builder";
                    break;
                case "helper":
                    place = "helper";
                    break;
            }
            return place;
        }

    }
}
