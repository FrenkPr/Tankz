using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class MissileBullet : Bullet
    {
        private float maxMoveSpeedX;
        private SoundEmitter engineStartSound;
        private float engineStartAngle;

        public MissileBullet() : base("missileBullet")
        {
            Dmg = 50;
            maxMoveSpeedX = 20;

            Type = BulletType.MissileBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.EnemyBullet);

            shootSound = new SoundEmitter(this, "MissileBulletWhistle");
            engineStartSound = new SoundEmitter(this, "MissileBulletEngineStart");

            shootSound.Volume = 1;
            engineStartSound.Volume = 1;

            engineStartAngle = -0.174533f; //-10 deg
        }

        public override void Shoot(Vector2 pos, Vector2 dir, float speedPercentage = 1)
        {
            base.Shoot(pos, dir, speedPercentage);

            shootSound.Play();
        }

        public override void Update()
        {
            base.Update();

            if (/*(Rotation > engineStartAngle || Rotation < -Math.PI - engineStartAngle)*/ RigidBody.CurrentMoveSpeed.Y > 0 && RigidBody.CurrentMoveSpeed.X != maxMoveSpeedX * Math.Sign(RigidBody.CurrentMoveSpeed.X))
            {
                RigidBody.CurrentMoveSpeed.X = maxMoveSpeedX * Math.Sign(RigidBody.CurrentMoveSpeed.X);

                if (!engineStartSound.IsPlaying())
                {
                    engineStartSound.RandomizePitch();
                    engineStartSound.Play();
                }
            }
        }
    }
}
