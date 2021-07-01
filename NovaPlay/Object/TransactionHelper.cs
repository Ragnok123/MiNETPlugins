using System;
using System.Collections.Generic;
using System.Text;
using MiNET;
using MiNET.Items;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace NovaPlay.Object
{
    public class TransactionHelper
    {
        /*        [PacketHandler, Receive]
        public Package Train(McpeInventoryTransaction datapacket, NovaPlayer player)
        {
            PlayerData data = player.pData;
            var trans = datapacket.transaction;
            foreach (Type baseArray in transactionClasses)
            {
                foreach (MethodInfo method in baseArray.GetMethods())
                {
                    switch (trans.TransactionType)
                    {
                        case McpeInventoryTransaction.TransactionType.ItemUse:
                            switch ((McpeInventoryTransaction.ItemUseAction)trans.ActionType)
                            {
                                case McpeInventoryTransaction.ItemUseAction.Use:
                                    player.SendMessage("right click works");
                                    break;
                                case McpeInventoryTransaction.ItemUseAction.Destroy:
                                    player.SendMessage("break works");
                                    break;
                                case McpeInventoryTransaction.ItemUseAction.Place:
                                    player.SendMessage("place works");
                                    break;
                            }
                            break;
                        case McpeInventoryTransaction.TransactionType.ItemUseOnEntity:
                            switch ((McpeInventoryTransaction.ItemUseOnEntityAction)trans.ActionType)
                            {
                                case McpeInventoryTransaction.ItemUseOnEntityAction.Attack:
                                    player.SendMessage("damage works");
                                    break;
                            }
                            break;
                    }
                }
            }
            return datapacket;
        }*/
        public TransactionHelper(McpeInventoryTransaction trans, NovaPlayer p)
        {
            transaction = trans;
            Player = p;
        }
        public McpeInventoryTransaction transaction { get; set; }
        public NovaPlayer Player { get; set; }

        public Block GetBlock()
        {
            BlockCoordinates coords = new BlockCoordinates(transaction.transaction.Position);
            Block b = Player.Level.GetBlock(coords);
            return b;
        }

        public Item GetItem()
        {
            return Player.GetItemInHand().ToItem();
        }

        public string GetCustomName()
        {
            return Player.GetItemInHand().GetCustomName();
        }

        public void ProcessClickEvent()
        {
            var action = transaction.transaction.Transactions[0];
            Item item = action.OldItem;
            if (action.OldItem.Id == 0) item = action.NewItem;
            var ev = new PlayerInventoryClickArgs(Player, item);
            Player.CallInventoryClickEvent(ev);

        }

        public McpeInventoryTransaction.TransactionType GetAction()
        {
            return transaction.transaction.TransactionType;
        }

        public int BlockOrItemInteract()
        {
            return (int)McpeInventoryTransaction.ItemUseAction.Use;
        }

        public Entity GetEntity()
        {
            Player.Level.TryGetEntity(transaction.transaction.EntityId, out Entity entity);
            return entity;
        }

        public Player GetPlayer()
        {
            return (Player)GetEntity();
        }

    }
}
