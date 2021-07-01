using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNET.Items;
using fNbt;
using MiNET.Utils;

namespace NovaPlay.Object.NovaItems
{
    public class NovaCompass : ItemCompass
    {

        public string CustomName;

        public NbtCompound _extraData;
        public Dictionary<string, string> kek;

        public override NbtCompound ExtraData
        {
            get { UpdateExtraData(); return _extraData; }
            set { _extraData = value; }
        }

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
                            new NbtString("Left click"),
                            new NbtString("Right click"),
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
