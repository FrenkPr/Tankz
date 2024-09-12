using System;
using System.Collections.Generic;
using OpenTK;
using Aiv.Fast2D;

namespace Tankz
{
    enum RigidBodyType { Player = 1, PlayerBullet = 2, Enemy = 4, EnemyBullet = 8, Tile = 16 }

    class RigidBody
    {
        public GameObject GameObject;
        public Collider Collider;
        public bool IsCollisionAffected;
        public bool IsGravityAffected;
        public bool IsGroundAffected;
        public RigidBodyType Type;
        protected uint collisionMask;
        public bool IsActive { get { return GameObject.IsActive; } }
        public Vector2 Position { get { return GameObject.Position; } set { GameObject.Position = value; } }
        public Vector2 CurrentMoveSpeed;
        public Vector2 MoveSpeed;

        public RigidBody(GameObject owner, Vector2 speed)
        {
            GameObject = owner;
            CurrentMoveSpeed = speed;
            MoveSpeed = speed;
            IsCollisionAffected = true;
            IsGravityAffected = true;
            IsGroundAffected = true;

            PhysicsMngr.Add(this);
        }

        public bool Collides(RigidBody otherBody, ref CollisionInfo collisionInfo)
        {
            return Collider.Collides(otherBody.Collider, ref collisionInfo);
        }

        public void Update()
        {
            if (IsActive)
            {
                if (IsGravityAffected)
                {
                    CurrentMoveSpeed.Y += PhysicsMngr.Gravity * Game.DeltaTime;
                }

                Position += CurrentMoveSpeed * Game.DeltaTime;  //updates game object position

                if (IsGroundAffected)
                {
                    CheckOutOfGround();
                }
            }
        }

        private void CheckOutOfGround()
        {
            if (Position.Y > PhysicsMngr.GroundY - GameObject.HalfHeight)
            {
                Position = new Vector2(Position.X, PhysicsMngr.GroundY - GameObject.HalfHeight);
                CurrentMoveSpeed.Y = 0;
            }
        }

        public void AddCollisionType(RigidBodyType type)
        {
            collisionMask |= (uint)type;
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }
    }
}
