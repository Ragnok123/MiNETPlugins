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
using MiNET.Plugins.Attributes;
using NovaPlay.Object.NovaItems;
using MiNET.Entities;
using NovaPlay.TimerScheduler;

namespace NovaPlay.MiniGamesAPI.BedWars
{
    public class BedWarsArena : GameAPI
    {
        public NovaCore novacore;
        public BedWarsTimer timermanager;

        public BedWarsArena(NovaCore novacore, string arenaname, string worldname, Dictionary<string, PlayerLocation> arenadata, Dictionary<string, BlockCoordinates> blocksdata)
        {
            this.novacore = novacore;
            this.arenaname = arenaname;
            this.worldname = worldname;
            this.arenadata = arenadata;
            this.arenacoordsdata = blocksdata;
            timermanager = new BedWarsTimer(this);
            TimerManager.ScheduleRepeatingTimer(timermanager, 1);
            this.gameType = "BedWars";
            this.gamePrefix = "§l§7[§cBed§fWars§7]";
            this.teams[0] = new Team(this, "red", "§c",1);
            this.teams[1] = new Team(this,"blue", "§b",2);
            this.teams[2] = new Team(this, "green", "§a",3);
            this.teams[3] = new Team(this, "yellow", "§e",4);
            _world = NovaCore.GetInstance().GetLevelByName(worldname);
            _world.BlockBreak += ManageBreak;
        }

        public void ManageBreak(object sender, BlockBreakEventArgs args)
        {
            PlayerData data = ((NovaPlayer)args.Player).pData;
            GamePlayer gamer = data.gData;
            if (data.IsInMinigame())
            {
                if(args.Block.Coordinates == arenacoordsdata["redBed"] || args.Block.Coordinates == arenacoordsdata["redBed2"])
                {
                    if (gamer.GetTeam().GetName() != "red")
                    {
                        gamer.baseData.GetPlayer().Level.BroadcastMessage(gamePrefix + " " + gamer.baseData.GetPlayer().NameTag + " §adestroyed bed from team §cRed");
                        args.Cancel = false;
                    }
                    else
                    {
                        gamer.baseData.GetPlayer().SendMessage(gamePrefix + " §cYou cant destroy your bed");
                    }
                }
                if (args.Block.Coordinates == arenacoordsdata["blueBed"] || args.Block.Coordinates == arenacoordsdata["blueBed2"])
                {
                    if (gamer.GetTeam().GetName() != "blue")
                    {
                        gamer.baseData.GetPlayer().Level.BroadcastMessage(gamePrefix + " " + gamer.baseData.GetPlayer().NameTag + " §adestroyed bed from team §bBlue");
                        args.Cancel = false;
                    }
                    else
                    {
                        gamer.baseData.GetPlayer().SendMessage(gamePrefix + " §cYou cant destroy your bed");
                    }
                }
                if (args.Block.Coordinates == arenacoordsdata["greenBed"] || args.Block.Coordinates == arenacoordsdata["greenBed2"])
                {
                    if (gamer.GetTeam().GetName() != "green")
                    {
                        gamer.baseData.GetPlayer().Level.BroadcastMessage(gamePrefix + " " + gamer.baseData.GetPlayer().NameTag + " §adestroyed bed from team Green");
                        args.Cancel = false;
                    }
                    else
                    {
                        gamer.baseData.GetPlayer().SendMessage(gamePrefix + " §cYou cant destroy your bed");
                    }
                }
                if (args.Block.Coordinates == arenacoordsdata["yellowBed"] || args.Block.Coordinates == arenacoordsdata["yellowBed2"])
                {
                    if (gamer.GetTeam().GetName() != "yellow")
                    {
                        gamer.baseData.GetPlayer().Level.BroadcastMessage(gamePrefix + " " + gamer.baseData.GetPlayer().NameTag + " §adestroyed bed from team §eYellow");
                        args.Cancel = false;
                    }
                    else
                    {
                        gamer.baseData.GetPlayer().SendMessage(gamePrefix + " §cYou cant destroy your bed");
                    }
                }
            }
        }

        public void GameTimer()
        {
            foreach (GamePlayer data in arenaplayers.Values)
            {
                data.baseData.GetPlayer().SendMessage("ahoj");
            }
        }


        [PacketHandler, Receive]
        public Packet Train(McpeInventoryTransaction datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            GamePlayer gamePlayer = data.gData;
            Entity targetAttribute = NovaCore.GetEntity(player.Level, datapacket.transaction.EntityId);
            var item = data.GetPlayer().Inventory.GetItemInHand();
            var trans = datapacket.transaction;
            switch (trans.TransactionType)
            {
                case McpeInventoryTransaction.TransactionType.ItemUseOnEntity:
                    switch ((McpeInventoryTransaction.ItemUseOnEntityAction)trans.ActionType)
                    {
                        case McpeInventoryTransaction.ItemUseOnEntityAction.Attack:
                            if (data.IsInMinigame())
                            {
                                if (targetAttribute is NovaPlayer && !data.IsInLobby())
                                {
                                    var entity = (NovaPlayer)NovaCore.GetEntity(player.Level, datapacket.transaction.EntityId);
                                    PlayerData hittedPlayerrr = entity.pData;
                                    GamePlayer hitted = hittedPlayerrr.gData;
                                    if (hittedPlayerrr.IsInMinigame())
                                    {
                                        if (this.gameStatus < 2)
                                        {
                                            return null;
                                        }
                                        else
                                        {
                                            if (gamePlayer.GetTeam().GetName() == hitted.GetTeam().GetName())
                                            {
                                                return null;
                                            }
                                            else
                                            {
                                                hitted.baseData.GetPlayer().HealthManager.Health = hitted.baseData.GetPlayer().HealthManager.Health - 4;
                                                if (hitted.baseData.GetPlayer().HealthManager.Health < 1)
                                                {
                                                    gamePlayer.baseData.GetPlayer().Level.BroadcastMessage(this.gamePrefix + " §aPlayer " + hitted.baseData.GetPlayer().NameTag + " §awas killed by " + gamePlayer.baseData.GetPlayer().NameTag);
                                                    if (hitted.CanRespawn())
                                                    {
                                                        hitted.baseData.GetPlayer().Teleport(arenadata[hitted.GetTeam().GetName() + "Pos"]);
                                                    }
                                                    else
                                                    {
                                                        LeaveTeam(hitted.GetTeam().GetId(), hitted);
                                                        RemovePlayer(hitted.baseData);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
            return datapacket;
        }

        [PacketHandler, Receive]
        public Packet HandleMove(McpeMovePlayer datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            GamePlayer gamer = data.gData;
            if (gamer != null)
            {
                if (gamer.IsInGame())
                {
                    if (this.gameStatus > 2)
                    {
                        if (gamer.baseData.GetPlayer().KnownPosition.Y < 10)
                        {
                            if (gamer.CanRespawn())
                            {
                                gamer.baseData.GetPlayer().HealthManager.Health = 20;
                                gamer.baseData.GetPlayer().Inventory.Clear();
                                gamer.baseData.GetPlayer().Teleport(arenadata[gamer.GetTeam().GetName() + "Pos"]);
                            }
                            else
                            {
                                LeaveTeam(gamer.GetTeam().GetId(), gamer);
                                RemovePlayer(gamer.baseData);
                            }
                        }
                    }
                }
            }
            return datapacket;
        }

    }
}
