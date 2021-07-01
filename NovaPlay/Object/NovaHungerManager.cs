using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;

namespace NovaPlay.Object
{
    public class NovaHungerManager : HungerManager
    {

        public NovaHungerManager(NovaPlayer player) : base(player)
        {

        }

        public bool InfiniteHunger;

        public override void OnTick()
        {
            base.OnTick();
            if (this.InfiniteHunger)
            {
                Player.HealthManager.Health = Player.HealthManager.MaxHealth;
                Hunger = MaxHunger;
            }
        }

    }
}
