using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.ImageProviders;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Sounds;
using MiNET.Utils;
using MiNET.Worlds;
using NovaPlay.Object;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using NovaPlay.MiniGamesAPI;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object
{
    public class PlayerData
    {
        public static string server = "82.208.17.16";
        public static string database = "252926_mysql_db";
        public static string username = "252926_mysql_db";
        public static string password = "Stargatewars1488";
        public static string connector = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";
        public MySqlConnection connection = new MySqlConnection(connector);
        

        public NovaPlayer player;
        public GamePlayer gData;
        public NovaGadgets novaGadgets;
        public Admin administrator;
        public Dictionary<string, string> userdata = new Dictionary<string, string>();
        public static ILog Logger = LogManager.GetLogger(typeof(PlayerData));
        public string gameId;
        public string rankPrefix;

        public string fakeLang = "english";
        public int fakeMoney = 0;
        public int fakeLevel = 0;
        public int fakeExp = 0;
        public int fakeGems = 0;
        public string fakeRank = "user";

        public PlayerData(NovaPlayer player)
        {
            this.player = player;
            this.novaGadgets = new NovaGadgets(this);
            this.administrator = new Admin(this);
        }

        public bool IsInMinigame()
        {
            if(this.gData != null)
            {
                return this.gData.IsInGame();
            } else
            {
                return false;
            }
        }

        public NovaGadgets GetGadgetsManager()
        {
            return this.novaGadgets;
        }
        
        public bool IsAuthed()
        {
            return !NovaCore.GetInstance().nonAuthed.ContainsKey(this.player.Username.ToLower());
        }

        public void RegisterPlayer(string password, string ip)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("INSERT INTO `nova_players` (`nickname`, `password`, `ip`, `rank`, `language`, `money`, `gems`, `exp`, `level`) VALUES ('" + this.player.Username.ToLower() + "', '" + password + "', '" + ip + "', 'user', 'english', '0', '0', '0', '0')");
            Logger.Info("registered player");
        }

        
        public void RegisterPlayerNew(string password, string ip)
        {
            WebClient client = new WebClient();
            var registerData = new JObject()
            {
                { "action", "register" },
                { "nickname", this.player.Username.ToLower() },
                { "password", password },
                { "ip", ip },
                { "rank", "user" },
                { "language", "english" },
                { "money", "0" },
                { "gems", "0" },
                { "exp", "0" },
                { "level", "0" }
            };
            client.UploadString("http://localhost/registerPlayer.php", registerData.ToString());
        } 
         


        public bool IsRegistered()
        {
            Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
            if (userdata != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetPrefix()
        {
            switch (this.GetRank())
            {
                case "owner":
                    this.rankPrefix = "§7[§4OWN§7] " + this.GetPlayer().Username;
                    break;
                case "admin+":
                    this.rankPrefix = "§7[§bADMIN+§7] " + this.GetPlayer().Username;
                    break;
                case "admin":
                    this.rankPrefix = "§7[§aADMIN§7] " + this.GetPlayer().Username;
                    break;
                case "dev":
                    this.rankPrefix = "§7[§eDEV§7] " + this.GetPlayer().Username;
                    break;
                case "builder":
                    this.rankPrefix = "§7[§6BUILDER§7] " + this.GetPlayer().Username;
                    break;
                case "mod":
                    this.rankPrefix = "§7[§2MOD§7] " + this.GetPlayer().Username;
                    break;
                case "helper":
                    this.rankPrefix = "§7[§aHELPER§7] " + this.GetPlayer().Username;
                    break;
                case "premium":
                    this.rankPrefix = "§7[§dPREMIUM§7] " + this.GetPlayer().Username;
                    break;
                case "vip":
                    this.rankPrefix = "§7[§9VIP§7] " + this.GetPlayer().Username;
                    break;
                case "user":
                    this.rankPrefix = "§7" + this.GetPlayer().Username;
                    break;
            }

            return this.rankPrefix;
        }

        public int GetMoney()
        {
            if (IsRegistered())
            {
                Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
                return int.Parse(userdata["money"]);
            } else
            {
                return fakeMoney;
            }
        }

        public void AddMoney(int money)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `money` = `money` + '" + money + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public int GetExp()
        {
            if (IsRegistered())
            {
                Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
                return int.Parse(userdata["exp"]);
            }
            else
            {
                return fakeExp;
            }
        }

        public void AddExp(int money)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `exp` = `exp` + '" + money + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public int GetLevel()
        {
            if (IsRegistered())
            {
                Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
                return int.Parse(userdata["level"]);
            }
            else
            {
                return fakeLevel;
            }
        }

        public void AddLevel(int money)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `level` = `level` + '" + money + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public int GetGems()
        {
            if (IsRegistered())
            {
                Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
                return int.Parse(userdata["gems"]);
            }
            else
            {
                return fakeMoney;
            }
        }

        public void AddGems(int money)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `gems` = `gems` + '" + money + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public NovaPlayer GetPlayer()
        {
            return this.player;
        }

        public bool IsInLobby()
        {
            return NovaCore.GetInstance().lobbyplayers.ContainsKey(this.player.Username);
        }

        public void SetLobby()
        {
            NovaCore.GetInstance().lobbyplayers.Add(this.player.Username, this.player);
            Logger.Info("[NovaCore] Setting lobby mode to player " + this.player.Username + "...");
        }

        public void UnsetLobby()
        {
            NovaCore.GetInstance().lobbyplayers.Remove(this.player.Username);
            Logger.Info("[NovaCore] Unsetting lobby mode for player " + this.player.Username + "...");
        }

        public string GetLanguage()
        {
            if (IsRegistered())
            {
                Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
                return (string)userdata["language"];
            } else
            {
                return fakeLang;
            }
        }

        public void SetLanguage(string language)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `language` = '" + language + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public string GetIP()
        {
            Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
            return (string) userdata["ip"];
        }

        public void SetIP(string ip)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `ip` = '" + ip + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public string GetPassword()
        {
            Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
            return (string) userdata["password"];
        }

        public void SetPassword(string ip)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `password` = '" + ip + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public void SetRank(string rank)
        {
            NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `rank` = '" + rank + "' WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
        }

        public string GetRank()
        {
            if (IsRegistered())
            {
                Dictionary<string, string> userdata = NovaCore.GetInstance().mysql.Query("SELECT * FROM `nova_players` WHERE `nickname` = '" + this.player.Username.ToLower() + "'");
                return (string)userdata["rank"];
            } else
            {
                return fakeRank;
            }
        }

        public bool IsAdmin()
        {
            return this.administrator.isAdmin;
        }

        public void SetAdmin(bool value)
        {
            this.administrator.isAdmin = value;
        }

        public string GetGame()
        {
            return this.gameId;
        }

        public void SetGame(string id)
        {
            this.gameId = id;
        }

    }
}
