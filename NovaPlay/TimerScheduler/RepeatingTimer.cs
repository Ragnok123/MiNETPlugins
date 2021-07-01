using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NovaPlay.TimerScheduler
{
    public class RepeatingTimer : TimerInterface
    {

        private Timer timer;
        private int secondTick;

        public void RunRepeatingTimer(int seconds)
        {
            timer.Elapsed += new ElapsedEventHandler(ElapsedTimer);
            timer.Interval = 1000 * seconds;
            timer.Start();
        }

        public void RunDelayedTimer(int seconds)
        {

        }

        public void DestroyTimer()
        {
            
        }

        private void ElapsedTimer(object state, ElapsedEventArgs arguments)
        {
            OnRun(secondTick);
        }

        public virtual void OnRun(int currentTick)
        {

        }

    }
}
