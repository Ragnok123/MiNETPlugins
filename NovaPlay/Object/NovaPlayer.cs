using System;
using MiNET.Utils;
using MiNET;
using System.Net;
using MiNET.Net;
using System.Threading.Tasks;
using MiNET.Worlds;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Blocks;
using fNbt;
using System.Collections.Generic;
using log4net;
using NovaPlay.MiniGamesAPI;
using NovaPlay.Object.ModalForm;
using NovaPlay.Object.ModalForm.Elements;
using MiNET.Effects;
using NovaPlay.Object.Scoreboard;
using NovaPlay.Object.NovaItems;

namespace NovaPlay.Object
{
    public class NovaPlayer : Player
    {

        public static ILog Logger = LogManager.GetLogger(typeof(NovaPlayer));

        public PlayerData pData;
        public GamePlayer gData;
        public NovaScoreboard Scoreboard { get; set; }
        public List<NovaItem> NItems;

        public NovaPlayer(MiNetServer server, IPEndPoint endPoint) : base(server, endPoint)
        {
        }

        public NovaItem GetItemInHand()
        {
            return NItems[Inventory.InHandSlot];
        }

        public bool AddItem(NovaItem nitem, bool update)
        {
            for (int si = 0; si < Inventory.Slots.Count; si++)
            {
                Item existingItem = Inventory.Slots[si];

                if (existingItem is ItemAir || existingItem.Id == 0 || existingItem.Id == -1)
                {
                    NItems[si] = nitem;
                    Inventory.Slots[si] = nitem.ToItem();
                    if (update) Inventory.SendSetSlot(si);
                    return true;
                }
            }

            return false;
        }

        public void SetItem(int slot, NovaItem item)
        {
            NItems[slot] = item;
            Inventory.Slots[slot] = item.ToItem();
            Inventory.SendSetSlot(slot);
        }

        public void SendModalData(string data, uint ids)
        {
            McpeModalFormRequest packet = McpeModalFormRequest.CreateObject();
            packet.formId = ids;
            packet.data = data;
            SendPacket(packet);
        }

        public void SendServerSettings(NovaModalForm form, uint settingId)
        {
            if (form is NovaModalFormCustom)
            {
                McpeServerSettingsResponse packet = McpeServerSettingsResponse.CreateObject();
                packet.formId = settingId;
                packet.data = form.ToJson();
                SendPacket(packet);
            }
            else
            {
                Console.WriteLine("It can be only custom_form");
            }
        }


        public override void InitializePlayer()
        {
            base.InitializePlayer();
            if (Username.Contains("§"))
            {
                Disconnect("§6[§eNovaCore§6] \n§cColorful nicks are not allowed");
            }

            if (Username.Contains(" "))
            {
                Disconnect("§6[§eNovaCore§6] \n§cNicks with namespace are not allowed");
            }



        }

        public override void Disconnect(string reason, bool sendDisconnect = true)
        {
            base.Disconnect(reason, sendDisconnect);
            Task.Run(delegate
            {
                try
                {

                }
                catch (Exception exeption)
                {

                    Logger.Error("Playerexeption disconnect: " + exeption.Message);
                    Logger.Info("Playerexeption disconnect: " + exeption.Message);
                }
            });
        }

        public event EventHandler<PlayerInventoryClickArgs> InventoryClickEvent;

        public void CallInventoryClickEvent(PlayerInventoryClickArgs args)
        {
            InventoryClickEvent?.Invoke(this, args);
        }

        public override void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
        {
            base.HandleMcpeInventoryTransaction(message);
            TransactionHelper helper = new TransactionHelper(message, this);
            helper.ProcessClickEvent();
        }

        public new void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message)
        {
            base.HandleMcpeServerSettingsRequest(message);
            int defaultLanguage = 0;
            if (pData.GetLanguage() == "english")
            {
                defaultLanguage = 0;
            }
            if (pData.GetLanguage() == "russian")
            {
                defaultLanguage = 1;
            }
            var form = new NovaModalFormCustom("§l§e> §cSettings §e<");
            form.AddElement(new NovaLabelElement("§l§bWelcome on §cNova§8Play, §e" + Username + "§b! "));
            form.AddElement(new NovaLabelElement("§l§bHere you can customize settings on this server"));
            form.AddElement(new NovaDropdownElement("§l§eLanguage", new List<string> { "english", "russian" }, defaultLanguage));
            McpeServerSettingsResponse response = McpeServerSettingsResponse.CreateObject();
            response.formId = 12345;
            response.data = form.ToJson();
            SendPacket(response);
        }

        public NovaScoreboard GetScoreboard()
        {
            return Scoreboard;
        }

        public void SendScoreboard(NovaScoreboard board)
        {
            Scoreboard = board;
            McpeSetDisplayObjective pk1 = McpeSetDisplayObjective.CreateObject();
            pk1.objectiveName = board.GetObjective().Name;
            pk1.displayName = board.GetObjective().DisplayName;
            pk1.criteriaName = board.GetObjective().CriteriaToString();
            pk1.displaySlot = board.GetObjective().SlotToString();
            pk1.sortOrder = 1;
            SendPacket(pk1);

            Dictionary<string, NovaScore> fakeMap = new Dictionary<string, NovaScore>();
            foreach (KeyValuePair<string, NovaScore> e in board.GetObjective().Scores)
            {
                fakeMap.Add(e.Key, e.Value);
            }
            foreach (NovaScore score in fakeMap.Values)
            {

                Object.Scoreboard.ScorePacketInfo info = new Object.Scoreboard.ScorePacketInfo();
                info.scoreboardId = score.ScoreId;
                info.objectiveName = score.Objective.Name;
                info.score = score.Score;
                info.addType = 3;
                info.fakePlayer = score.FakePlayer;

                Scoreboard.ScorePacketInfos list = new Scoreboard.ScorePacketInfos();
                list.Add(info);


                McpeSetScore pk2 = McpeSetScore.CreateObject();
                pk2.type = (byte)score.AddOrRemove;
                pk2.scorePacketInfos = list;
                SendPacket(pk2);

                if (score.AddOrRemove == 1)
                {
                    String id = score.FakeId;
                    board.GetObjective().Scores.Remove(id);
                }
            }
        }

    }

    public class PlayerInventoryClickArgs : EventArgs
    {
        public Player Player { get; }
        public Item Item { get; set; } 
        public bool Cancel { get; set; }

        public PlayerInventoryClickArgs(Player player, Item item)
        {
            Player = player;
            Item = item;
        }
    }
}
