using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class Tile : GameObject
    {
        public CrateExplosion Explosion { get; }
        public RandomizeSoundEmitter WoodCrackSound { get; }

        public Tile(string textureId, Vector2 pos) : base(textureId)
        {
            Position = pos;
            Y -= HalfHeight;
            IsActive = true;
            RigidBody = new RigidBody(this, Vector2.Zero);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.AddCollisionType(RigidBodyType.Tile);

            Explosion = new CrateExplosion();

            UpdateMngr.Remove(this);

            WoodCrackSound = new RandomizeSoundEmitter(this);

            WoodCrackSound.AddClip("woodCrack_1");
            WoodCrackSound.AddClip("woodCrack_2");

            WoodCrackSound.Volume = 1;

            //DebugMngr.AddItem(RigidBody.Collider);
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            OnOtherTileCollision(collisionInfo);
        }

        protected virtual void OnOtherTileCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if (X < collisionInfo.Collider.X)
                {
                    // Collision from Left (inverse horizontal delta)
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                X += collisionInfo.Delta.X;
                collisionInfo.Collider.X -= collisionInfo.Delta.X;
                RigidBody.CurrentMoveSpeed.X = 0;
                collisionInfo.Collider.RigidBody.CurrentMoveSpeed.X = 0;
            }

            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Collision from Top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                }

                Y += collisionInfo.Delta.Y + 0.002f * Math.Sign(collisionInfo.Delta.Y);
                collisionInfo.Collider.Y -= collisionInfo.Delta.Y;
                RigidBody.CurrentMoveSpeed.Y = 0;
                collisionInfo.Collider.RigidBody.CurrentMoveSpeed.Y = 0;
            }
        }
    }
}
