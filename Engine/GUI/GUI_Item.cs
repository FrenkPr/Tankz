using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    abstract class GUI_Item : GameObject
    {
        public GUI_Item(string textureId, float width = 0, float height = 0) : base(textureId, 1, width, height, DrawLayer.GUI)
        {
            Sprite.Camera = CameraMngr.GetCamera("GUI");
            IsActive = true;
        }

        public void SetSpriteColor(Vector4 color)
        {
            Sprite.SetMultiplyTint(color);
        }
    }
}
