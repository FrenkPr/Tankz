using Aiv.Fast2D;
using OpenTK;
using System;

namespace Tankz
{
    class Player : Tank
    {
        public bool IsFirePressed;
        private bool isWeaponSwitcherPressed;
        private float moveSpeed;
        private float cannonRotationSpeed;
        private float maxCannonAngle;
        private float minCannonAngle;
        public int Id { get; private set; }
        public ProgressBar BulletChargeBar { get; }
        public float CurrentLoadingBulletChargeBarSpeed;
        private float loadingBulletChargeBarSpeed;
        private static StateMachine fsmCurrentPlayer;
        private static bool fsmInitialized;
        public WeaponsGUI_Item WeaponsHUD { get; private set; }
        private float moveAccumulator;
        private Vector2 shakeOffset;

        public Player(int id) : base()
        {
            Id = id;

            Y = PhysicsMngr.GroundY;

            moveSpeed = 5;
            cannonRotationSpeed = 2;
            CurrentLoadingBulletChargeBarSpeed = 0;
            loadingBulletChargeBarSpeed = 0.5f;

            IsActive = true;
            EnergyBar.IsActive = true;

            EnergyBar.Sprite.Camera = CameraMngr.GetCamera("GUI");
            EnergyBar.BarSprite.Camera = EnergyBar.Sprite.Camera;

            BulletType = BulletType.DefaultBullet;
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.EnemyBullet);

            minCannonAngle = (float)-Math.PI;
            maxCannonAngle = 0;

            BulletChargeBar = new ProgressBar("wpnBarFrame", "loadingBar", new Vector2(Game.PixelsToUnits(4)), 197, 18);
            BulletChargeBar.Scale(0);

            if (!fsmInitialized)
            {
                fsmCurrentPlayer = new StateMachine(this);

                fsmCurrentPlayer.AddState(StateType.Wait, new WaitState());
                fsmCurrentPlayer.AddState(StateType.Move, new MoveState());
                fsmCurrentPlayer.AddState(StateType.Shoot, new ShootState());

                fsmCurrentPlayer.GoTo(StateType.Move);

                fsmInitialized = true;
            }

            WeaponsHUD = new WeaponsGUI_Item(this);
        }

        private bool IsJoypadFirePressed()
        {
            if (Game.JoypadCtrls.Count != 0)
            {
                if (Game.JoypadCtrls[Id].IsFirePressed())
                {
                    return true;
                }
            }

            return false;
        }

        public void Input()
        {
            if (!IsActive)
            {
                return;
            }

            QuitGame();
            Move();
            RotateCannon();
            Shoot();
            NextWeapon();
        }

        public void QuitGame()
        {
            if (Game.KeyboardCtrls[Id].IsKeyPressed(KeyCode.Esc) || IsJoypadBtnPressed(JoypadValue.B))
            {
                Game.CurrentScene.IsPlaying = false;
            }
        }

        private void Move()
        {
            if (IsFirePressed)
            {
                RigidBody.CurrentMoveSpeed.X = 0;
                return;
            }

            float dirX = GetKeyboardMoveDirX() != 0 ? GetKeyboardMoveDirX() : GetJoypadMoveDirX();

            RigidBody.CurrentMoveSpeed.X = moveSpeed * dirX;


            if (dirX != 0)
            {
                moveAccumulator += Game.DeltaTime * 10;
                shakeOffset = new Vector2((float)Math.Cos(moveAccumulator) * 0.01f, (float)Math.Sin(moveAccumulator) * 0.01f);

                bodySprite.position += shakeOffset;
            }
        }

        private void RotateCannon()
        {
            float dirX = GetKeyCannonRotationDir() != 0 ? GetKeyCannonRotationDir() : GetJoypadCannonRotationDir();

            if (dirX != 0)
            {
                cannonSprite.Rotation += cannonRotationSpeed * dirX * Game.DeltaTime;
                cannonSprite.Rotation = MathHelper.Clamp(cannonSprite.Rotation, minCannonAngle, maxCannonAngle);
            }
        }

        public void Shoot()
        {
            if (WeaponsHUD.NumWeaponsAvailable <= 0)
            {
                return;
            }

            if (Game.KeyboardCtrls[Id].IsFirePressed() || IsJoypadFirePressed())
            {
                if (!IsFirePressed)
                {
                    IsFirePressed = true;
                    BulletChargeBar.Position = cannonSprite.position - new Vector2(BulletChargeBar.HalfWidth, BulletChargeBar.Height + cannonSprite.Width + 0.2f);
                    BulletChargeBar.IsActive = true;
                }

                CurrentLoadingBulletChargeBarSpeed += loadingBulletChargeBarSpeed * Game.DeltaTime;
                BulletChargeBar.Scale(CurrentLoadingBulletChargeBarSpeed);

                if (CurrentLoadingBulletChargeBarSpeed >= 1 || CurrentLoadingBulletChargeBarSpeed <= 0)
                {
                    loadingBulletChargeBarSpeed = -loadingBulletChargeBarSpeed;
                }
            }

            else if (IsFirePressed)
            {
                Shoot(CurrentLoadingBulletChargeBarSpeed);

                if (!WeaponsHUD.SelectedWeapon.IsInfinite)
                {
                    WeaponsHUD.SelectedWeapon.SubOneBullet();

                    if (!WeaponsHUD.SelectedWeapon.IsAvailable)
                    {
                        SwitchWeapon();
                    }
                }

                IsFirePressed = false;

                BulletChargeBar.IsActive = false;
                BulletChargeBar.Scale(0);

                loadingBulletChargeBarSpeed = Math.Abs(loadingBulletChargeBarSpeed);
                CurrentLoadingBulletChargeBarSpeed = 0;
            }
        }

        public void NextWeapon()
        {
            if (IsFirePressed || WeaponsHUD.NumWeaponsAvailable <= 0)
            {
                return;
            }

            if (Game.KeyboardCtrls[Id].IsKeyPressed(KeyCodeType.NextWeapon) || IsJoypadBtnPressed(JoypadValue.Square))
            {
                if (!isWeaponSwitcherPressed)
                {
                    SwitchWeapon();

                    isWeaponSwitcherPressed = true;
                }
            }

            else if (isWeaponSwitcherPressed)
            {
                isWeaponSwitcherPressed = false;
            }
        }

        public void SwitchWeapon()
        {
            WeaponsHUD.NextWeapon();
        }

        private float GetKeyCannonRotationDir()
        {
            return Game.KeyboardCtrls[Id].GetRotation();
        }

        private float GetJoypadCannonRotationDir()
        {
            return Game.JoypadCtrls.Count != 0 ? Game.JoypadCtrls[Id].GetRotation() : 0;
        }

        private float GetKeyboardMoveDirX()
        {
            return Game.KeyboardCtrls[Id].GetHorizontal();
        }

        private float GetJoypadMoveDirX()
        {
            return Game.JoypadCtrls.Count != 0 ? Game.JoypadCtrls[Id].GetHorizontal() : 0;
        }

        private bool IsJoypadBtnPressed(JoypadValue value)
        {
            return Game.JoypadCtrls.Count != 0 ? Game.JoypadCtrls[Id].IsJoypadBtnPressed(value) : false;
        }

        protected override void CheckOutOfScreen()
        {
            //horizontal collisions
            if (Position.X - HalfWidth < CameraMngr.CameraLimits.MinX - Game.OrthoHalfWidth)
            {
                X = CameraMngr.CameraLimits.MinX - Game.OrthoHalfWidth + HalfWidth;
            }

            else if (Position.X + HalfWidth > CameraMngr.CameraLimits.MaxX + Game.OrthoHalfWidth)
            {
                X = CameraMngr.CameraLimits.MaxX + Game.OrthoHalfWidth - HalfWidth;
            }

            //vertical collisions
            if (Position.Y - HalfHeight < CameraMngr.CameraLimits.MinY - Game.OrthoHalfHeight)
            {
                Y = CameraMngr.CameraLimits.MinY - Game.OrthoHalfHeight + HalfHeight;

                if (RigidBody.IsGravityAffected && RigidBody.CurrentMoveSpeed.Y < 0)
                {
                    RigidBody.CurrentMoveSpeed.Y *= -1;
                }
            }
        }

        public override void OnDie()
        {
            IsActive = false;
            EnergyBar.IsActive = false;

            ((PlayScene)Game.CurrentScene).NumPlayersAlive--;

            if (((PlayScene)Game.CurrentScene).NumPlayersAlive <= 0)
            {
                ((PlayScene)Game.CurrentScene).EndGame = true;
            }
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            base.OnCollision(collisionInfo);

            if (collisionInfo.Collider.RigidBody.Type == RigidBodyType.EnemyBullet)
            {
                AddDamage(((Bullet)collisionInfo.Collider).Dmg);
                BulletMngr.RestoreBullet((Bullet)collisionInfo.Collider);
            }

            else if (collisionInfo.Collider is Enemy enemy)
            {
                AddDamage(enemy.CollisionPlayerToEnemyDmg);
                enemy.OnDie();
            }

            if (!IsAlive)
            {
                OnDie();
            }
        }

        public override void Update()
        {
            if (IsActive && fsmCurrentPlayer.Player == this)
            {
                fsmCurrentPlayer.Update();
            }

            base.Update();

            //System.Console.WriteLine("Player position: " + Position);
        }
    }
}
