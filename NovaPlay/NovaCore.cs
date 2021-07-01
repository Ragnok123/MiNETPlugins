using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Runtime.InteropServices;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data.SqlClient;
using fNbt;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.Passive;
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

using NovaPlay;
using NovaPlay.Entities;
using NovaPlay.Listeners;
using NovaPlay.Object;
using NovaPlay.Object.NovaItems;
using NovaPlay.MiniGamesAPI;
using NovaPlay.MiniGamesAPI.SkyWars;
using System.Data;
using NovaPlay.MiniGamesAPI.BedWars;
using NovaPlay.Object.ModalForm;
using NovaPlay.Object.ModalForm.Elements;
using System.Text;

namespace NovaPlay
{
    [Plugin(PluginName = "NovaCore v1", PluginVersion = "1.0", Author = "Ragok123", Description = "Core")]
    public class NovaCore : Plugin
    {

        public MySQL mysql;
        public JoinListener joinlistener;
        public LeaveListener leavelistener;
        public static NovaCore instance;
        public static string prefix = "§l§7[§cNova§9Play§7]";
        public Dictionary<string, NovaPlayer> lobbyplayers = new Dictionary<string, NovaPlayer>();
        public Dictionary<string, NovaPlayer> nonAuthed = new Dictionary<string, NovaPlayer>();
        public static ILog Logger = LogManager.GetLogger(typeof(NovaCore));
        public static MiNetServer servinstance;
        public Dictionary<string, ConcurrentDictionary<ChunkCoordinates, ChunkColumn>> mapChunks = new Dictionary<string, ConcurrentDictionary<ChunkCoordinates, ChunkColumn>>();

        public void Configure(MiNetServer server)
        {
            server.PlayerFactory = new NovaPlayerFactory();
        }

        protected override void OnEnable()
        {
            instance = this;
            Configure(Context.Server);
            servinstance = Context.Server;
            mysql = new MySQL(this);
            mysql.Connect();
            leavelistener = new LeaveListener(this);
            joinlistener = new JoinListener(this);
            Language.InitEng();
            Language.InitRus();
            Context.Server.PlayerFactory.PlayerCreated += (sender, args) =>
            {
                Player player = args.Player;
                player.PlayerJoin += PlayerJoin;
                player.PlayerLeave += PlayerLeave;
            };
            Level homeWorld = GetLevelByName("Overworld");
            homeWorld.SpawnPoint.X = 632;
            homeWorld.SpawnPoint.Y = 9;
            homeWorld.SpawnPoint.Z = 984;
            homeWorld.BlockBreak += OnBlockBreak;
            Logger.Warn("[NovaPlay] NovaCore v1 enabled");
        }

        public static MiNetServer GetServer()
        {
            return servinstance;
        }
        public void LoadLevel(string world)
        {
            string worldname = Config.GetProperty("PluginDirectory", "Plugins") + "\\" + world;
            AnvilWorldProvider _worldprovider = new AnvilWorldProvider(worldname);
            _worldprovider.Initialize();
            _worldprovider.PruneAir();
            _worldprovider.MakeAirChunksAroundWorldToCompensateForBadRendering();
            Level levelWorld = new Level(Context.Server.LevelManager, world, _worldprovider, Context.Server.LevelManager.EntityManager);
            levelWorld.Initialize();
            mapChunks.Add(world, _worldprovider._chunkCache);
            Context.Server.LevelManager.Levels.Add(levelWorld);
        }
        public void RestartLevel(string world)
        {
            string worldname = Config.GetProperty("PluginDirectory", "Plugins") + "\\" + world;
            AnvilWorldProvider _worldprovider = new AnvilWorldProvider(worldname);
            _worldprovider._chunkCache = mapChunks[world];
        }


        [Command(Name = "testmode", Description = "")]
        public void Uiiiii(NovaPlayer player)
        {
            if (player.pData.IsAuthed())
            {
                if (player.pData.IsAdmin())
                {
                    if (player.pData.administrator.isTesting)
                    {
                        player.pData.administrator.isTesting = false;
                    } else
                    {
                        player.pData.administrator.isTesting = true;
                    }
                }
            }
        }

        [Command(Name = "save", Description = "Saves world")]
        public void SaveWorld(NovaPlayer player)
        {
            if (player.pData.IsAuthed())
            {
                if (player.pData.IsAdmin())
                {
                    AnvilWorldProvider provider = (AnvilWorldProvider)player.Level.WorldProvider;
                    provider.SaveChunks();
                }
            }
        }

        

        public void OnBlockBreak(object globalObject, BlockBreakEventArgs args)
        {
            PlayerData data = ((NovaPlayer)args.Player).pData;
            if (data.IsInLobby())
            {
                if (!data.IsAdmin())
                {
                    args.Cancel = true;
                }
                else
                {
                    args.Cancel = false;
                }
            }
        }       

        [PacketHandler, Receive]
        public Packet OpenGameMenu(McpeInventoryTransaction datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            var item = data.GetPlayer().GetItemInHand();
            var trans = datapacket.transaction;
            if (trans.TransactionType == McpeInventoryTransaction.TransactionType.ItemUse && (McpeInventoryTransaction.ItemUseAction)trans.ActionType == McpeInventoryTransaction.ItemUseAction.Use)
            {
                if (item.ToItem() is NovaCompass)
                {
                    if (!data.IsAuthed())
                    {
                        if (data.IsRegistered())
                        {
                            data.GetPlayer().NameTag = data.GetPrefix();
                            data.GetPlayer().DisplayName = data.GetPrefix();
                            data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §aWelcome on server", MessageType.Raw);
                            data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §aThis account is registered", MessageType.Raw);
                            var custom = new NovaModalFormCustom("Authetication");
                            custom.AddElement(new NovaLabelElement("§aWelcome on server"));
                            custom.AddElement(new NovaLabelElement("§aThis account is registered. Please, log in."));
                            custom.AddElement(new NovaInputElement("Password", "your password"));
                            string jsonData = custom.ToJson();
                            data.GetPlayer().SendModalData(jsonData, 2);
                            if (data.GetRank() != "user" || data.GetRank() != "vip" || data.GetRank() != "premium")
                            {
                                data.SetAdmin(true);
                            }
                        }
                        else
                        {
                            data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §aWelcome on server", MessageType.Raw);
                            data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §aSeems you are not registered", MessageType.Raw);
                            var custom = new NovaModalFormCustom("Registration");
                            custom.AddElement(new NovaLabelElement("§aWelcome on server"));
                            custom.AddElement(new NovaLabelElement("§aSeems you are not registered. Please, regster."));
                            custom.AddElement(new NovaInputElement("Password", "your password"));
                            custom.AddElement(new NovaInputElement("Confirm password", "password again"));
                            string jsonData = custom.ToJson();
                            data.GetPlayer().SendModalData(jsonData, 1);
                        }
                    }
                }
            }
            return datapacket;
        }
        

        public static NovaCore GetInstance()
        {
            return instance;
        }

        public static string GetPrefix()
        {
            return prefix;
        }
        

        [PacketHandler]
        public Packet HandleChat(McpeText package, NovaPlayer player)
        {
            string message = package.message;
            PlayerData data = player.pData;
            if (data.IsInLobby() && data.IsAuthed())
            {
                data.GetPlayer().Level.BroadcastMessage(new NovaPlay.Object.ChatFormatting(data).ProcessMessage(message));
                return null;
            }
            if (!data.IsAuthed())
            {
                if (!data.IsRegistered())
                {
                    data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §cRegister firstly");
                    return null;
                }
                else
                { 
                    data.GetPlayer().SendMessage(NovaCore.GetPrefix() + Language.Translate("login_f", player, new string[0]));
                    return null;
                }
            }

            
            return package;
        }

        public static Entity GetEntity(Level level, long entityId)
        {
            Player entity;
            level.Players.TryGetValue(entityId, out entity);
            return entity ?? level.Entities.Values.FirstOrDefault(e => e.EntityId == entityId);
        }

        [PacketHandler]
        public Packet HandleHit(McpeInventoryTransaction datapacket, NovaPlayer player)
        {
            TransactionHelper helper = new TransactionHelper(datapacket, player);
            PlayerData data = player.pData;
            if (data.IsInLobby())
            {
                Entity entity = helper.GetEntity();
                if (datapacket.transaction.TransactionType == McpeInventoryTransaction.TransactionType.ItemUseOnEntity)
                    if (datapacket.transaction.ActionType == 1)
                    {
                        if (entity is MinigameNPC)
                        {
                            if (((MinigameNPC)entity).GetNameTag() == "BedWars")
                            {
                                return null;
                            }
                        }
                    }
            }
            return datapacket;
        }
        
        


        public void PlayerJoin(object obj, PlayerEventArgs args)
        {
            NovaPlayer player = (NovaPlayer)args.Player;
            player.pData = new PlayerData(player);
            joinlistener.SpawnPlayer(player.pData);
            Logger.Error(player.pData.GetRank() + " " + player.Username + " joined");
        }

        public void PlayerLeave(object obg, PlayerEventArgs args)
        {
            NovaPlayer player = (NovaPlayer)args.Player;
            PlayerData data = player.pData;
            leavelistener.DespawnPlayer(data);
        }

        public int GetLevelPlayers(string world)
        {
            Player[] players = GetLevelByName(world).GetSpawnedPlayers();
            return Convert.ToInt32(players);
        }

        public Level GetLevelByName(string world)
        {
            return Context.LevelManager.GetLevel(null, world);
        }

        [Command(Name = "npc", Description = "Secret")]
        public void SpawnNpc(NovaPlayer player, string typ, string name)
        {
            PlayerData data = player.pData;
            if (data.IsAuthed())
            {
                var npc = new MinigameNPC(name, data.GetPlayer().Level, typ);
                npc.KnownPosition = data.GetPlayer().KnownPosition;
                npc.Skin = player.Skin;
                npc.SpawnEntity();
                npc.SetNameTag(name);
            }
        }

        [Command(Name = "plugins", Description = "Show server plugins")]
        public void Plugins(NovaPlayer player)
        {
            PlayerData data = player.pData;
            if (data.IsAuthed())
            {
                player.SendMessage("Plugins(1): §aNovaCore_v1, BedWars_v1", type: MessageType.Raw);
            }
        }

        

/*        [Command(Name = "server", Description = "Transfer to another server on NovaPlay")]
        public void World(NovaPlayer player, string server)
        {
            PlayerData data = player.pData;
            if (data.IsRegistered())
            {
                if (data.IsAuthed())
                {
                    Level currentLevel = player.Level;
                    Level toLevel = GetLevelByName(server);
                    player.SpawnLevel(toLevel, new PlayerLocation()
                    {
                        X = toLevel.SpawnPoint.X,
                        Y = toLevel.SpawnPoint.Y,
                        Z = toLevel.SpawnPoint.Z,
                        Yaw = toLevel.SpawnPoint.Yaw,
                        Pitch = toLevel.SpawnPoint.Pitch,
                    });
                }
                else
                {
                    player.SendMessage(GetPrefix() + Language.Translate("login_f", player, new string[0]));
                }
            } else
            {
                player.SendMessage(NovaCore.GetPrefix() + " §cRegister firstly");
            }
        }*/

        [PacketHandler]
        public Packet CmdManager(McpeCommandRequest datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            if (!data.IsAuthed())
            {
                player.SendMessage("error");
                return null;
            }
            return datapacket;
        }


        [PacketHandler, Receive]
        public Packet Reg(McpeModalFormResponse datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            var parsed = JsonConvert.DeserializeObject<JArray>(datapacket.data);
            if(datapacket.formId == 1 || datapacket.formId == 3)
               if(parsed[2].Equals(parsed[3]))
                {
                    nonAuthed.Remove(data.GetPlayer().Username.ToLower());
                    data.RegisterPlayer(parsed[2].ToString(), data.GetPlayer().EndPoint.Address.ToString());
                    data.GetPlayer().SendMessage(GetPrefix() + " §aYou have registered this account", MessageType.Raw);
                    data.GetPlayer().SetNoAi(false);

                }
               else
                {
                    var custom = new NovaModalFormCustom("Registration");
                    custom.AddElement(new NovaLabelElement("§cError"));
                    custom.AddElement(new NovaLabelElement("§cBoth passwords must be same"));
                    custom.AddElement(new NovaInputElement("Password", "your password"));
                    custom.AddElement(new NovaInputElement("Confirm password", "password again"));
                    string jsonData = custom.ToJson();
                    data.GetPlayer().SendModalData(jsonData, 3);
                }
            
            return datapacket;
        }

        [PacketHandler, Receive]
        public Packet SaveServerSettings(McpeModalFormResponse datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            switch (datapacket.formId)
            {
                case 12345:
                    var parsed = new ModalFormHandler().GetCustom(datapacket.data);
                    
                    if (data.IsRegistered())
                    {
                        if (parsed[2].ToString() == "0")
                        {
                            data.SetLanguage("english");
                        }
                        if (parsed[2].ToString() == "1")
                        {
                            data.SetLanguage("russian");
                        }
                        //player.SendMessage(parsed[2].ToString());
                        player.SendMessage(NovaCore.GetPrefix() + Language.Translate("settings_save", player, new string[0]));
                    } else
                    {
                        player.SendMessage(NovaCore.GetPrefix() + " §cRegister firstly");
                    }
                    break;
            }
            return datapacket;
        }

        [PacketHandler, Receive]
        public Packet TestModal(McpeModalFormResponse datapacket, NovaPlayer player)
        {
            switch (datapacket.formId)
            {
                case 100000: //ModalFormCustom
                    var parsed = JsonConvert.DeserializeObject<JArray>(datapacket.data);
                    break;
                case 200000:
                    switch (datapacket.data)
                    {
                        case "0":
                            break;
                    }
                    break;
            }
            return datapacket;
        }

        [PacketHandler, Receive]
        public Packet Log(McpeModalFormResponse datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            var parsed = JsonConvert.DeserializeObject<JArray>(datapacket.data);
            if(datapacket.formId == 2 || datapacket.formId == 4)
            {
                if(Convert.ToString(parsed[2]).Equals(data.GetPassword()))
                {
                    nonAuthed.Remove(data.GetPlayer().Username.ToLower());
                    data.GetPlayer().SendMessage(GetPrefix() + Language.Translate("logged_in", player, new string[0]), MessageType.Raw);
                    data.GetPlayer().SetNoAi(false);
                    if(data.GetRank() == "owner" || data.GetRank() == "admin" || data.GetRank() == "dev" || data.GetRank() == "builder" || data.GetRank() == "mod" || data.GetRank() == "helper")
                    {
                       data.SetAdmin(true);
                       data.GetPlayer().PermissionLevel = PermissionLevel.Operator;
                       data.GetPlayer().SendAdventureSettings();
                    }
                    if(data.GetRank() == "user" || data.GetRank() == "vip")
                    {
                        data.SetAdmin(false);
                    }
                }
                else
                {
                    var custom = new NovaModalFormCustom("Authetication");
                    custom.AddElement(new NovaLabelElement("§cError"));
                    custom.AddElement(new NovaLabelElement("§cWrong password"));
                    custom.AddElement(new NovaInputElement("Password", "your password"));
                    string jsonData = custom.ToJson();
                    data.GetPlayer().SendModalData(jsonData, 4);
                }
            }
            return datapacket;
        }

        [Command(Name="setrank", Description = "core command")]
        public void SetRank(NovaPlayer player, string nick, string rank)
        {
            if (player.pData.IsAuthed())
            {
                if (player.Username == "Ragnok123")
                {
                    NovaCore.GetInstance().mysql.ExecuteQuery("UPDATE `nova_players` SET `rank` = '" + rank + "' WHERE `nickname` = '" + nick + "'");
                }
            }
        }

        [Command(Name ="gm", Description ="secret")]
        public void GameMode(NovaPlayer player, GameMode gamemode)
        {
            PlayerData data = player.pData;
            if (data.IsAuthed())
            {
                if (data.IsAdmin())
                {
                    player.SetGameMode(gamemode);
                }
            }
        }


        



    }


}
