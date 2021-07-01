using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPlay.TimerScheduler
{
    public class TimerManager
    {

        public Dictionary<int, TimerInterface> timers = new Dictionary<int, TimerInterface>();

        public static void ScheduleDelayedTask(TimerInterface iface, int seconds)
        {
            iface.RunDelayedTimer(seconds);
        }

        public static void ScheduleRepeatingTimer(TimerInterface iface, int seconds)
        {
            iface.RunRepeatingTimer(seconds);
        }

    }
}
