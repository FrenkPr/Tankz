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

            if (IsKeyPressed(keys.KeyCode[KeyCodeType.Left]) && IsKeyPressed(keys.KeyCode[KeyCodeType.Right]))
            {
                direction = 0;
            }

            else
            {
                if (IsKeyPressed(keys.KeyCode[KeyCodeType.Left]))
                {
                    direction = -1;
                }

                else if (IsKeyPressed(keys.KeyCode[KeyCodeType.Right]))
                {
                    direction = 1;
                }
            }

            return direction;
        }

        public override float GetRotation()
        {
            float direction = 0;

            if (IsKeyPressed(keys.KeyCode[KeyCodeType.CannonRotationLeft]) && IsKeyPressed(keys.KeyCode[KeyCodeType.CannonRotationRight]))
            {
                direction = 0;
            }

            else
            {
                if (IsKeyPressed(keys.KeyCode[KeyCodeType.CannonRotationLeft]))
                {
                    direction = -1;
                }

                else if (IsKeyPressed(keys.KeyCode[KeyCodeType.CannonRotationRight]))
                {
                    direction = 1;
                }
            }

            return direction;
        }

        public override bool IsFirePressed()
        {
            return IsKeyPressed(keys.KeyCode[KeyCodeType.Fire]);
        }

        public bool IsKeyPressed(KeyCode value)
        {
            return Game.Window.GetKey(value);
        }

        public bool IsKeyPressed(KeyCodeType type)
        {
            return IsKeyPressed(keys.KeyCode[type]);
        }

        public bool IsMouseButtonPressed(MouseValue value)
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
