using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    abstract class Controller
    {
        protected int controllerIndex;

        public Controller(int index)
        {
            controllerIndex = index;
        }

        public abstract float GetHorizontal();
        public abstract float GetRotation();
        public abstract bool IsFirePressed();
    }
}
