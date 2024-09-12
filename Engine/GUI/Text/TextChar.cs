using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class TextChar : GameObject
    {
        private Font font;
        public char Char;

        public TextChar(Font font, char textChar, Vector2 pos, float charWidth, float charHeight) : base(font.FontTextureID, 1, charWidth, charHeight, DrawLayer.GUI)
        {
            this.font = font;
            Position = pos;
            Char = textChar;
            //Sprite.pivot = Vector2.Zero;

            Sprite.Camera = CameraMngr.GetCamera("GUI");

            UpdateMngr.Remove(this);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                Vector2 offset = font.GetCharOffset(Char);

                Sprite.DrawTexture(font.FontTexture, (int)offset.X, (int)offset.Y, font.CharWidth, font.CharHeight);
            }
        }
    }
}
