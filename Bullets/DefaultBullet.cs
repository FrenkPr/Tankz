using OpenTK;

namespace Tankz
{
    class DefaultBullet : Bullet
    {
        public DefaultBullet() : base("defaultBullet")
        {
            Dmg = 25;

            Type = BulletType.DefaultBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.EnemyBullet);

            shootSound = new SoundEmitter(this, "defaultBulletShoot");
        }

        public override void Shoot(Vector2 pos, Vector2 dir, float speedPercentage = 1)
        {
            base.Shoot(pos, dir, speedPercentage);

            shootSound.Volume = speedPercentage;
            shootSound.RandomizePitch();

            shootSound.Play();
        }
    }
}
