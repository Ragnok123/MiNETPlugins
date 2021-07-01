using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NovaPlay.TimerScheduler
{
    public interface TimerInterface
    {
        void RunRepeatingTimer(int seconds);
        void RunDelayedTimer(int seconds);
        void DestroyTimer();
        void OnRun(int currentTick);
    }
}
