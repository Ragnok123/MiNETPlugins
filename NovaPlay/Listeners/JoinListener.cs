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

using NovaPlay;
using NovaPlay.Object;
using NovaPlay.Object.NovaItems;
using NovaPlay.Object.ModalForm;
using NovaPlay.Object.ModalForm.Elements;

namespace NovaPlay.Listeners
{
    public class JoinListener
    {

        public NovaCore novacore;

        public JoinListener(NovaCore novacore)
        {
            this.novacore = novacore;
        }


        public void SpawnPlayer(PlayerData data)
        {
            NovaPlayer player = data.GetPlayer();
            data.GetPlayer().PermissionLevel = PermissionLevel.Member;
            data.GetPlayer().SendAdventureSettings();
            data.GetPlayer().Teleport(new PlayerLocation
            {
                X = data.GetPlayer().Level.SpawnPoint.X,
                Y = data.GetPlayer().Level.SpawnPoint.Y,
                Z = data.GetPlayer().Level.SpawnPoint.Z
            });
            data.GetPlayer().HungerManager = new NovaHungerManager(data.GetPlayer());
            ((NovaHungerManager)data.GetPlayer().HungerManager).InfiniteHunger = true;
            data.GetPlayer().HealthManager = new NovaHealthManager(data.GetPlayer());
            data.GetPlayer().SetItem(1, new NovaItem(new NovaCompass(), "§l§a> §eMenu §a<", 1, 0));
            data.GetPlayer().SendPlayerInventory();
            this.novacore.nonAuthed.Add(data.GetPlayer().Username.ToLower(), data.GetPlayer());
            data.SetLobby();
            data.GetPlayer().SetNoAi(true);
            if (data.IsRegistered())
            {
                data.GetPlayer().SetNameTag(data.GetPrefix());
                data.GetPlayer().SetDisplayName(data.GetPrefix());
                data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §aWelcome on server", MessageType.Raw);
                data.GetPlayer().SendMessage(NovaCore.GetPrefix() + " §aThis account is registered", MessageType.Raw);
                var custom = new NovaModalFormCustom("Authetication");
                custom.AddElement(new NovaLabelElement("§aWelcome on server"));
                custom.AddElement(new NovaLabelElement("§aThis account is registered. Please, log in."));
                custom.AddElement(new NovaInputElement("Password", "your password"));
                string jsonData = custom.ToJson();
                data.GetPlayer().SendModalData(jsonData, 2);
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
