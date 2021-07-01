using System;
using System.ComponentModel;
using System.Reflection;
using log4net;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET;
using MiNET.Plugins;
using MiNET.Sounds;
using MiNET.Items;
using MiNET.Entities.Projectiles;

namespace NovaPlay.Object
{
    public class NovaHealthManager : HealthManager
    {
        public NovaHealthManager(Entity entity) : base(entity)
        {
        }

        public override void Kill()
        {
            var player = Entity as Player;
            player.Teleport(new PlayerLocation(player.Level.SpawnPoint.X, player.Level.SpawnPoint.Y, player.Level.SpawnPoint.Z, 0, 0));
        }

        public override void TakeHit(Entity source, Item tool, int damage = 1, DamageCause cause = DamageCause.Unknown)
        {
            var player = source as Player;

        }



    }
}
