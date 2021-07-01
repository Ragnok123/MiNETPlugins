using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NovaPlay.TimerScheduler
{
    public class DelayedTimer : TimerInterface
    {

        private Timer timer;
        private int secondTick;

        public void RunRepeatingTimer(int seconds)
        {
            
        }
        public void RunDelayedTimer(int seconds)
        {
            timer.Elapsed += new ElapsedEventHandler(ElapsedTimer);
            timer.Interval = 1000 * seconds;
            secondTick = seconds;
            timer.Start();
        }

        public void DestroyTimer()
        {
            timer.Stop();
        }

        private void ElapsedTimer(object state, ElapsedEventArgs arguments)
        {
            OnRun(secondTick);
            secondTick--;
            if(secondTick == 0)
            {
                DestroyTimer();
            }
        }

        public virtual void OnRun(int currentTick)
        {

        }

    }
}
