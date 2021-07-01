using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using fNbt;
using log4net;
using MiNET;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

using NovaPlay;
using NovaPlay.Object;
using NovaPlay.Object.NovaItems;

namespace NovaPlay.Listeners
{
    public class LeaveListener
    {

        public NovaCore novacore;

        public LeaveListener(NovaCore novacore)
        {
            this.novacore = novacore;
        }

        public void DespawnPlayer(PlayerData data)
        {
            if (data.gData != null)
            {
                if (data.gData.api.IsInArena(data))
                {
                    data.gData.api.RemovePlayer(data);
                }
            }
            if (!data.IsAuthed())
            {
                this.novacore.nonAuthed.Remove(data.GetPlayer().Username);
            }
            if (data.IsInLobby())
            {
                data.UnsetLobby();
            }
        }

    }


}
