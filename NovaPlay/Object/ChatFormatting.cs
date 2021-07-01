using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPlay.Object
{
    public class ChatFormatting
    {

        public PlayerData data;
        public String message;

        public ChatFormatting(PlayerData data)
        {
            this.data = data;

        }

        public string ProcessMessage(String message)
        {
            switch (this.data.GetRank())
            {
                case "owner":
                    this.message = "§7[§4OWN§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "admin+":
                    this.message = "§7[§bADMIN+§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "admin":
                    this.message = "§7[§aADMIN§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "dev":
                    this.message = "§7[§eDEV§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "builder":
                    this.message = "§7[§6BUILDER§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "mod":
                    this.message = "§7[§2MOD§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "helper":
                    this.message = "§7[§aHELPER§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "premium":
                    this.message = "§7[§dPREMIUM§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "vip":
                    this.message = "§7[§9VIP§7] " + this.data.GetPlayer().Username + "> §f" + message;
                    break;
                case "user":
                    this.message = "§7" + this.data.GetPlayer().Username + "> §f" + message;
                    break;
            }

            return this.message;
        }

    }

}
