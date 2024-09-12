using OpenTK;

namespace Tankz
{
    enum BulletType
    {
        DefaultBullet,
        MissileBullet,
        Length
    }

    abstract class Bullet : GameObject
    {
        public int Dmg;
        protected Vector2 moveSpeed;

        public BulletType Type { get; protected set; }

        protected SoundEmitter shootSound;

        public Bullet(string textureName, int width = 0, int height = 0) : base(textureName, 1, width, height, DrawLayer.MiddleGround)
        {
            RigidBody = new RigidBody(this, Vector2.Zero);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Tile);
            RigidBody.IsGroundAffected = false;

            moveSpeed = new Vector2(13);
        }

        public virtual void Shoot(Vector2 pos, Vector2 dir, float speedPercentage = 1)
        {
            Position = pos;

            RigidBody.CurrentMoveSpeed = moveSpeed * dir * speedPercentage;
            Forward = dir;

            if (RigidBody.Type == RigidBodyType.PlayerBullet)
            {
                CameraMngr.SetTarget(this);
            }
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            //on any collision type this bullet will be restored
            BulletMngr.RestoreBullet(this);

            if (collisionInfo.Collider is Tile tile)
            {
                tile.Explosion.Position = tile.Position;
                tile.Explosion.AnimationExplosion.Play();

                tile.WoodCrackSound.Play();

                PlayScene.Tiles.Remove(tile);
                DrawMngr.Remove(tile);
                PhysicsMngr.Remove(tile.RigidBody);
                tile.RigidBody = null;
            }
        }

        public virtual void Reset()
        {
            IsActive = false;

            if (CameraMngr.CurrentBehaviour is FollowTargetBehaviour followTargetBehaviour)
            {
                followTargetBehaviour.Target = null;
            }
        }

        protected override void CheckOutOfScreen()
        {
            Vector2 distToOutOfScreen = Position - CameraMngr.MainCamera.position;

            if (IsActive && distToOutOfScreen.LengthSquared > CameraMngr.HalfDiagonalSquared)
            {
                BulletMngr.RestoreBullet(this);
            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                Forward = RigidBody.CurrentMoveSpeed;
            }

            base.Update();
        }
    }
}
