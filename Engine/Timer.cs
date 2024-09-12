using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class Timer
    {
        public float Clock;
        private float maxClockTime;

        public Timer(float maxClockTime, float clockStartTime = 0)
        {
            this.maxClockTime = maxClockTime;
            Clock = clockStartTime;
        }

        public void DecTime()
        {
            Clock -= Game.DeltaTime;
        }

        public void Reset()
        {
            Clock = maxClockTime;
        }
    }
}
