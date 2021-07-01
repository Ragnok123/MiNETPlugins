using fNbt;
using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Items;

namespace NovaPlay.Object.NovaItems
{
    public class NovaItem
    {

        public string CustomName;
        public byte count;
        public int meta;
        public NbtCompound _extraData;
        public Item _item;

        public NovaItem(Item item, string name, byte count, short meta)
        {
            _item = item;
            this.CustomName = name;
            _item.Count = count;
            _item.Metadata = meta;
            UpdateExtraData();
            _item.ExtraData = _extraData;
        }

        public Item ToItem()
        {
            return _item;
        }

        //public override NbtCompound ExtraData
        //{
        //    get { UpdateExtraData(); return _extraData; }
        //    set { _extraData = value; }
        //}

        private void UpdateExtraData()
        {
            _extraData = new NbtCompound
            {
                {
                    new NbtCompound("display")
                    {
                        new NbtString("Name", this.CustomName),
                        new NbtList("Lore")
                        {
                            new NbtString("Right"),
                            new NbtString("Click"),
                        }
                    }
                }
            };
        }

        public string GetCustomName()
        {
            return this.CustomName;
        }

        public void SetCustomName(string customName)
        {
            this.CustomName = customName;
        }

    }
}
