using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    enum JoypadValue
    {
        PS4_X,
        Circle,
        Triangle,
        Square,
        A,
        B,
        Xbox_X,
        Y,
        Start,
        R1,
        R2,
        L1,
        L2,
        L3,
        R3
    }

    abstract class JoypadController : Controller
    {
        public JoypadController(int index) : base(index)
        {

        }

        public override float GetHorizontal()
        {
            return Game.Window.JoystickAxisLeft(controllerIndex).X;
        }

        public override float GetRotation()
        {
            return Game.Window.JoystickAxisRight(controllerIndex).X;
        }

        public abstract bool IsJoypadBtnPressed(JoypadValue value);
    }
}
