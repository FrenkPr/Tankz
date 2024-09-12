using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace Tankz
{
    enum MouseValue
    {
        LeftClick,
        RightClick
    }

    enum KeyCodeType
    {
        Left,
        Right,
        CannonRotationLeft,
        CannonRotationRight,
        Fire,
        NextWeapon,
        Length
    }

    struct KeyboardConfig
    {
        public Dictionary<KeyCodeType, KeyCode> KeyCode;

        public KeyboardConfig(List<KeyCode> keys)
        {
            KeyCode = new Dictionary<KeyCodeType, KeyCode>();

            for (int i = 0; i < keys.Count; i++)
            {
                KeyCode.Add((KeyCodeType)i, keys[i]);
            }
        }
    }

    class KeyboardController : Controller
    {
        private KeyboardConfig keys;

        public KeyboardController(int index, List<KeyCode> keys) : base(index)
        {
            this.keys = new KeyboardConfig(keys);
        }

        public override float GetHorizontal()
        {
            float direction = 0;

            if (IsValuePressed(keys.KeyCode[KeyCodeType.Left]) && IsValuePressed(keys.KeyCode[KeyCodeType.Right]))
            {
                direction = 0;
            }

            else
            {
                if (IsValuePressed(keys.KeyCode[KeyCodeType.Left]))
                {
                    direction = -1;
                }

                else if (IsValuePressed(keys.KeyCode[KeyCodeType.Right]))
                {
                    direction = 1;
                }
            }

            return direction;
        }

        public override float GetRotation()
        {
            float direction = 0;

            if (IsValuePressed(keys.KeyCode[KeyCodeType.CannonRotationLeft]) && IsValuePressed(keys.KeyCode[KeyCodeType.CannonRotationRight]))
            {
                direction = 0;
            }

            else
            {
                if (IsValuePressed(keys.KeyCode[KeyCodeType.CannonRotationLeft]))
                {
                    direction = -1;
                }

                else if (IsValuePressed(keys.KeyCode[KeyCodeType.CannonRotationRight]))
                {
                    direction = 1;
                }
            }

            return direction;
        }

        public override bool IsFirePressed()
        {
            return IsValuePressed(keys.KeyCode[KeyCodeType.Fire]);
        }

        public bool IsValuePressed(KeyCode value)
        {
            return Game.Window.GetKey(value);
        }

        public bool IsValuePressed(KeyCodeType type)
        {
            return IsValuePressed(keys.KeyCode[type]);
        }

        public bool IsValuePressed(MouseValue value)
        {
            bool res = false;

            switch (value)
            {
                case MouseValue.LeftClick:
                    res = Game.Window.MouseLeft;
                    break;

                case MouseValue.RightClick:
                    res = Game.Window.MouseRight;
                    break;
            }

            return res;
        }
    }
}
