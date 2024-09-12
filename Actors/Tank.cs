using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    abstract class Tank : Actor
    {
        private Texture cannonTexture;
        private Texture bodyTexture;
        protected Sprite cannonSprite;
        protected Sprite bodySprite;
        protected Vector2 cannonOffset { get => new Vector2(cannonSprite.Width); }
        protected Vector2 cannonForward { get => new Vector2((float)Math.Cos(cannonSprite.Rotation), (float)Math.Sin(cannonSprite.Rotation)); set => cannonSprite.Rotation = (float)Math.Atan2(value.Y, value.X); }
        public override float X { get => base.X; set { base.X = value; bodySprite.position.X = value; cannonSprite.position.X = value - cannonSprite.Width * 0.5f + 0.28f; } }
        public override float Y { get => base.Y; set { base.Y = value; bodySprite.position.Y = value - 0.2f; cannonSprite.position.Y = value - cannonSprite.Height * 0.5f - 0.3f; } }
        public override Vector2 Position { get => base.Position; set { base.Position = value; bodySprite.position = new Vector2(value.X, value.Y - 0.2f); cannonSprite.position = new Vector2(value.X - cannonSprite.Width * 0.5f + 0.28f, value.Y - cannonSprite.Height * 0.5f - 0.3f); } }

        public Tank() : base("tankTracks")
        {
            cannonTexture = TextureMngr.GetTexture("tankCannon");
            bodyTexture = TextureMngr.GetTexture("tankBody");

            cannonSprite = new Sprite(Game.PixelsToUnits(cannonTexture.Width), Game.PixelsToUnits(cannonTexture.Height));
            bodySprite = new Sprite(Game.PixelsToUnits(bodyTexture.Width), Game.PixelsToUnits(bodyTexture.Height));

            bodySprite.pivot = new Vector2(bodySprite.Width * 0.5f, bodySprite.Height * 0.5f);
            cannonSprite.pivot = new Vector2(0, cannonSprite.Height * 0.5f);
        }

        public void Shoot(float speedPercentage = 1)
        {
            Shoot(cannonSprite.position + cannonOffset * cannonForward, cannonForward, speedPercentage);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                cannonSprite.DrawTexture(cannonTexture);
                Sprite.DrawTexture(texture);  //tracks
                bodySprite.DrawTexture(bodyTexture);
            }
        }
    }
}
