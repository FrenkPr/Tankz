using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class WeaponsGUI_Item : GUI_Item
    {
        public SelectedWeaponGUI_Item SelectedWpnGUIframe { get; }
        private int selectedWpnGUIindex;
        private List<BulletGUI_Item> bulletsGUI;
        public BulletGUI_Item SelectedWeapon { get { return bulletsGUI[selectedWpnGUIindex]; } }
        public int NumWeaponsAvailable;
        private Player owner;

        public WeaponsGUI_Item(Player owner) : base("wpnBarFrame", height: 44)
        {
            this.owner = owner;

            Sprite.pivot = Vector2.Zero;

            bulletsGUI = new List<BulletGUI_Item>((int)BulletType.Length);
            Tuple<string, int, bool>[] bulletContents = new Tuple<string, int, bool>[(int)BulletType.Length];

            //bulletContents contains: textureId, numBullets, IsInfinite
            bulletContents[0] = new Tuple<string, int, bool>("defaultBulletGUIico", 1, true);
            bulletContents[1] = new Tuple<string, int, bool>("missileBulletGUIico", 3, false);

            for (int i = 0; i < bulletContents.Length; i++)
            {
                bulletsGUI.Add(new BulletGUI_Item(this, bulletContents[i].Item1, bulletContents[i].Item2, bulletContents[i].Item3));
            }

            SelectedWpnGUIframe = new SelectedWeaponGUI_Item("wpnSelectedGUI");
            selectedWpnGUIindex = 0;

            NumWeaponsAvailable = bulletsGUI.Count;
        }

        public void InitItemsPosition()
        {
            Vector2 bulletsOffset = new Vector2(0.1f, 0.05f);
            Vector2 numBulletsOffset = new Vector2(0, 0.1f);
            Vector2 itemPos = Position + bulletsOffset;

            itemPos.X += bulletsGUI[0].HalfWidth;
            itemPos.Y += bulletsGUI[0].HalfHeight;

            for (int i = 0; i < bulletsGUI.Count; i++)
            {
                bulletsGUI[i].Position = itemPos;
                bulletsGUI[i].numBulletsDisplay.EditPosition(itemPos + numBulletsOffset);

                itemPos.X += bulletsGUI[i].Width + bulletsOffset.X;
            }

            SelectedWpnGUIframe.Position = SelectedWeapon.Position;
        }

        public void NextWeapon()
        {
            if (NumWeaponsAvailable <= 0)
            {
                SelectedWpnGUIframe.IsActive = false;
                return;
            }

            do
            {
                owner.BulletType = (BulletType)(((int)owner.BulletType + 1) % (int)BulletType.Length);
                selectedWpnGUIindex = (selectedWpnGUIindex + 1) % bulletsGUI.Count;
                SelectedWpnGUIframe.Position = SelectedWeapon.Position;
            }
            while (!SelectedWeapon.IsAvailable);
        }
    }
}
