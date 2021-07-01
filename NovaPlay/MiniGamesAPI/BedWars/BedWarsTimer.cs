using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NovaPlay.TimerScheduler;

namespace NovaPlay.MiniGamesAPI.BedWars
{
    public class BedWarsTimer : RepeatingTimer
    {

        public BedWarsArena arena;
        public Timer timer;

        public BedWarsTimer(BedWarsArena arena)
        {
            this.arena = arena;
        }

        public override void OnRun(int currentTick)
        {
            base.OnRun(currentTick);
            arena.GameTimer();
        }

    }
}
