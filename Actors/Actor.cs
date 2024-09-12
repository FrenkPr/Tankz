using System;
using OpenTK;

namespace Tankz
{
    abstract class Actor : GameObject
    {
        protected int energy;
        protected int maxEnergy;
        public int CollisionPlayerToEnemyDmg;  //the damage it takes to the player in case of collision with enemy
        public BulletType BulletType;
        public bool IsAlive { get { return energy > 0; } }
        public ProgressBar EnergyBar;

        public Actor(string textureName, Vector2 moveSpeed = default, int numFrames = 1, int width = 0, int height = 0) : base(textureName, numFrames, width, height)
        {
            maxEnergy = 100;
            EnergyBar = new ProgressBar("frameLoadingBar", "loadingBar", new Vector2(Game.PixelsToUnits(4)));
            ResetEnergy();

            RigidBody = new RigidBody(this, moveSpeed);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Tile);

            //DebugMngr.AddItem(RigidBody.Collider);
        }

        public void Shoot(Vector2 position, Vector2 direction, float speedPercentage = 1)
        {
            Bullet bullet = BulletMngr.GetBullet(BulletType);

            if (bullet != null)
            {
                position += direction * bullet.HalfWidth;
                bullet.Shoot(position, direction, speedPercentage);
            }
        }

        public override void Update()
        {
            base.Update();

            if (IsActive)
            {
                if (!(this is Player))
                {
                    EnergyBar.Position = new Vector2(Position.X - EnergyBar.HalfWidth, Position.Y - HalfHeight - EnergyBar.Height - 0.1f);
                }
            }
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            OnTileCollision(collisionInfo);
        }

        protected virtual void OnTileCollision(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Collider is Tile))
            {
                return;
            }

            float deltaX_Offset = 0.01f;
            collisionInfo.Delta.X += deltaX_Offset;

            // Horizontal Collision
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                if (X < collisionInfo.Collider.X)
                {
                    // Collision from Left (inverse horizontal delta)
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                X += collisionInfo.Delta.X;
                RigidBody.CurrentMoveSpeed.X = 0.0f;
            }

            // Vertical Collision
            else
            {
                if (Y < collisionInfo.Collider.Y)
                {
                    // Collision from Top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                }

                else
                {
                    // Collision from Bottom
                    collisionInfo.Collider.Y -= collisionInfo.Delta.Y;
                    collisionInfo.Collider.RigidBody.CurrentMoveSpeed.Y = 0.0f;
                }

                Y += collisionInfo.Delta.Y;
                RigidBody.CurrentMoveSpeed.Y = 0.0f;
            }
        }

        public virtual void OnDie()
        {

        }

        public void ResetEnergy()
        {
            energy = maxEnergy;
            EnergyBar.Scale((float)energy / (float)maxEnergy);
        }

        public void AddDamage(int dmg)
        {
            energy -= dmg;
            EnergyBar.Scale((float)energy / (float)maxEnergy);
        }
    }
}
