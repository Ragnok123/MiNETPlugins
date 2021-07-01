using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET.Net;
using MiNET.Entities;
using MiNET.Plugins;
using NovaPlay.Object;
using System.Timers;

namespace NovaPlay.Entities
{

    public class NoDamageHealthManager : HealthManager
    {
        public NoDamageHealthManager(Entity entity) : base(entity)
        {
        }

        public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
        {
            //base.TakeHit(source, 0, cause);
        }

        public override void OnTick()
        {
        }
    }

    public class NovaBossBar 
    {
        public static int WITHER_ID = 52;
        private Plugin plugin;
        private Dictionary<string, NovaPlayer> players;
        public Dictionary<string, string> translations;
        public long id;
        private int health;
        private int maxHealth;
        private bool hasDataChanged;
        private bool hasAttributeChanged;
        private MetadataDictionary metadata;
        private McpeBossEvent permanentPacket;
        private McpeBossEvent permanentPacketUpdate;
        private McpeUpdateAttributes attributesPacket;
        private Random random;
        private BossBarTimer timer;

        public NovaBossBar(Plugin plugin)
        {
            this.players = new Dictionary<string, NovaPlayer>();
            this.translations = null;
            this.health = 1;
            this.maxHealth = 600;
            this.hasDataChanged = true;
            this.hasAttributeChanged = true;
            this.permanentPacket = McpeBossEvent.CreateObject();
            this.permanentPacketUpdate = McpeBossEvent.CreateObject();
            this.attributesPacket = McpeUpdateAttributes.CreateObject();
            this.random = new Random();
            this.plugin = plugin;
            this.id = 2147483491L;
            this.metadata = new MetadataDictionary();
            this.metadata[4] = new MetadataString("");
            this.metadata[0] = new MetadataLong(196640L);
            this.metadata[38] = new MetadataLong(-1L);
            this.metadata[54] = new MetadataFloat(0.0f);
            this.metadata[55] = new MetadataFloat(0.0f);
            this.metadata[39] = new MetadataFloat(0.0f);
            this.permanentPacket.bossEntityId = this.id;
            this.permanentPacket.eventType = 0;
            this.permanentPacketUpdate.eventType = 1;
            this.attributesPacket.runtimeEntityId = this.id;
            this.attributesPacket.attributes["minecraft:health"] = new PlayerAttribute()
            {
                Name = "minecraft:health",
                MinValue = 0,
                MaxValue = this.maxHealth,
                Value = this.health,
            };
            this.attributesPacket.Encode();
            timer = new BossBarTimer(this);
            timer.RunTimer();
       }

        public void AddPlayer(NovaPlayer p)
        {
            this.players.Add(p.Username.ToLower(), p);
            PlayerLocation pos = GetDirectionVector(p);
            McpeAddEntity pk = McpeAddEntity.CreateObject();
            pk.entityType = 52;
            pk.runtimeEntityId = this.id;
            pk.x = (float)pos.X;
            pk.y = (float)(pos.Y - 7.0);
            pk.z = (float)pos.Z;
            pk.speedX = 0.0f;
            pk.speedY = 0.0f;
            pk.speedZ = 0.0f;
            pk.yaw = 0.0f;
            pk.pitch = 0.0f;
            pk.metadata = this.metadata;
            McpeUpdateAttributes pk2 = McpeUpdateAttributes.CreateObject();
            pk2.runtimeEntityId = this.id;
            pk2.attributes["minecraft:health"] = new PlayerAttribute()
            {
                Name = "minecraft:health",
                MinValue = 0,
                MaxValue = this.maxHealth,
                Value = this.health,
            };
            p.SendPacket(pk);
            p.SendPacket(pk2);
            p.SendPacket(this.attributesPacket);
            p.SendPacket(this.permanentPacket);
        }

        public void RemovePlayer(NovaPlayer p)
        {
            this.RemovePlayer(p.Username);
            McpeRemoveEntity pk = McpeRemoveEntity.CreateObject();
            pk.entityIdSelf = this.id;
            p.SendPacket(pk);
            McpeBossEvent pk2 = McpeBossEvent.CreateObject();
            pk2.bossEntityId = this.id;
            pk2.eventType = 2;
            p.SendPacket(pk2);
        }

        public void RemovePlayer(string name)
        {
            this.players.Remove(name.ToLower());
        }

        public void Update()
        {
            foreach (KeyValuePair<string, NovaPlayer> lel in this.players)
            {
                NovaPlayer pla = lel.Value;
                this.Update(pla);
            }
        }

        public void Update(NovaPlayer player)
        {
            this.Update(player, false);
        }

        public void Update(NovaPlayer p, bool respawn)
        {
            PlayerLocation pos = GetDirectionVector(p);
            McpeMoveEntity pk2 = McpeMoveEntity.CreateObject();
            pk2.runtimeEntityId = this.id;
            pk2.position.X = (float)pos.X;
            pk2.position.Y = (float)(pos.Y - 30.0);
            pk2.position.Z = (float)pos.Z;
            pk2.position.Yaw = (float)p.KnownPosition.Yaw;
            pk2.position.HeadYaw = (float)p.KnownPosition.Yaw;
            pk2.position.Pitch = (float)p.KnownPosition.Pitch;
            p.SendPacket(pk2);
            p.SendPacket(this.permanentPacket);
            p.SendPacket(this.attributesPacket);
        }

        public void SetHealth(int health)
        {
            this.health = Math.Max(health, 1);
        }

        public void SetMaxHealth(int health)
        {
            this.maxHealth = Math.Max(health, 1);
        }

        public void UpdateText(string text)
        {
            if (!text.Equals(this.metadata[4]))
            {
                this.metadata[4] = new MetadataString(text);
            }
        }

        public void UpdateInfo()
        {
            McpeSetEntityData pk = McpeSetEntityData.CreateObject();
            pk.runtimeEntityId = this.id;
            pk.metadata = this.metadata;
            if (this.translations != null)
            {
                foreach(KeyValuePair<string, NovaPlayer> lel in this.players)
                {
                    NovaPlayer pla = lel.Value;
                    pk.metadata[4] = new MetadataString(this.translations[pla.pData.GetLanguage()]);
                    pla.SendPacket(pk);
                }
            }
            else
            {
                pk.Encode();
                foreach(KeyValuePair<string, NovaPlayer> lel in this.players)
                {
                    lel.Value.SendPacket(pk);
                }
            }
            this.attributesPacket.attributes["minecraft:absorption"] = new PlayerAttribute()
            {
                MaxValue = this.maxHealth,
                Value = this.health
            };
            this.attributesPacket.Encode();
            foreach (KeyValuePair<string, NovaPlayer> lel in this.players)
            {
                lel.Value.SendPacket(this.attributesPacket);
            }
        }

        public PlayerLocation GetDirectionVector(NovaPlayer p)
        {
            double pitch = 1.5707963267948966;
            double yaw =p.KnownPosition.Yaw * 3.141592653589793 / 180.0;
            double x = Math.Sin(pitch) * Math.Cos(yaw);
            double z = Math.Sin(pitch) * Math.Sin(yaw);
            double y = Math.Cos(pitch);
            return new PlayerLocation(x, y, z);
        }

    }

    public class BossBarTimer
    {

        public NovaBossBar bar;
        public Timer timer;

        public BossBarTimer(NovaBossBar bar)
        {
            this.bar = bar;
        }

        public void RunTimer()
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(UpdateTimer);
            timer.Interval = 1000 * 10;
            timer.Start();
        }

        public void DestroyTimer()
        {
            timer.Stop();
        }

        public void UpdateTimer(object state, ElapsedEventArgs arguments)
        {
            bar.Update();
        }

    }
}
