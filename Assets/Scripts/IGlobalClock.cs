using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.DayNightCycle
{
    interface IGlobalClock
    {
        void RequestAlarm(GlobalClock.AlarmHandler _callback, int _hour, int _minute, int _second, bool _loop = false, int _loopCount = 0);
        void RequestAlarm(GlobalClock.AlarmHandler _callback, GlobalClock.KeyTimeOfDay _tod, bool _loop = false, int _loopCount = 0);
        float GetSecondsSinceStartOfDay();
    }
}
