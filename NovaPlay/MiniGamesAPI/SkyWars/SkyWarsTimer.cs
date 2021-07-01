using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NovaPlay.MiniGamesAPI.SkyWars
{
    public class SkyWarsTimer
    {

        public SkyWarsArena arena;
        public Timer timer;

        public SkyWarsTimer(SkyWarsArena arena)
        {
            this.arena = arena;
        }

        public void RunTimer()
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(QueueTimer);
            timer.Interval = 1000;
            timer.Start();
        }

        public void QueueTimer(object state, ElapsedEventArgs arguments)
        {
            arena.GameTimer();
        }

    }
}
