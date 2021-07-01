using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNET.Sounds;
using MiNET.Blocks;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Items;
using MiNET.Net;
using fNbt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NovaPlay.Object.ModalForm;
using NovaPlay.Object.ModalForm.Elements;
using Newtonsoft.Json.Linq;
using log4net;

namespace NovaPlay.Object
{
    public class NovaGadgets
    {

        public PlayerData playerdata;
        public string category;
        public Dictionary<string, NovaPlayer> inMenuEpta = new Dictionary<string, NovaPlayer>();
        public static ILog Log = LogManager.GetLogger(typeof(NovaGadgets));

        public NovaGadgets(PlayerData data)
        {
            this.playerdata = data;
        }

        public bool IsInMenu()
        {
            Player player = this.playerdata.GetPlayer();
            return this.inMenuEpta.ContainsKey(player.Username);
        }

        public void OpenMenu()
        {
            NovaPlayer player = this.playerdata.GetPlayer();
            var custom = new NovaModalFormSimple("§l§eGADGETS", "");
            custom.AddButton(new NovaButtonElement("Gadget one"));
            player.SendModalData(custom.ToJson(), (uint) ModalFormHandler.FormIds.GadgetsOpen);
        }


    }
}
