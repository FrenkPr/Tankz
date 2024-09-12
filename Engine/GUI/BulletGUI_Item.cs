using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class BulletGUI_Item : GUI_Item
    {
        private int numBullets;
        private int maxNumBullets;
        public bool IsInfinite { get; }
        public bool IsAvailable { get { return numBullets > 0; } }
        public TextObject numBulletsDisplay;
        private WeaponsGUI_Item owner;

        public BulletGUI_Item(WeaponsGUI_Item owner, string textureId, int numBullets, bool isInfinite, float width = 0, float height = 0) : base(textureId, width, height)
        {
            this.owner = owner;

            maxNumBullets = numBullets;
            this.numBullets = numBullets;
            IsInfinite = isInfinite;
            numBulletsDisplay = new TextObject(numBullets.ToString(), Vector2.Zero, 20, 20);

            if (IsInfinite)
            {
                numBulletsDisplay.IsActive = false;
            }
        }

        public void SubOneBullet()
        {
            numBullets = MathHelper.Clamp(numBullets - 1, 0, maxNumBullets);
            numBulletsDisplay.EditText(numBullets.ToString());

            if (!IsAvailable)
            {
                SetSpriteColor(new Vector4(1, 0, 0, 1));
                owner.NumWeaponsAvailable--;
            }
        }
    }
}
