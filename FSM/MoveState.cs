using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class MoveState : State
    {
        private Timer timeToNextTurn;

        public MoveState()
        {
            timeToNextTurn = new Timer(16, 16);

            TextMngr.AddText("timeToNextPlayerTurn", (int)timeToNextTurn.Clock + "", new Vector2(Game.OrthoHalfWidth, 3));
        }

        public override void OnEnter()
        {
            timeToNextTurn.Reset();
            CameraMngr.SetTarget(fsm.Player,false);
            CameraMngr.MoveTo(fsm.Player.Position, 1);
        }

        public override void Update()
        {
            if (!(CameraMngr.CurrentBehaviour is FollowTargetBehaviour))
            {
                return;
            }

            TextMngr.EditText("timeToNextPlayerTurn", (int)timeToNextTurn.Clock + "");

            if (((FollowTargetBehaviour)CameraMngr.CurrentBehaviour).Target != fsm.Player)
            {
                fsm.GoTo(StateType.Shoot);

                return;
            }

            timeToNextTurn.DecTime();

            if (timeToNextTurn.Clock <= 0)
            {
                fsm.GoTo(StateType.Wait);

                return;
            }

            fsm.Player.Input();
        }

        public override void OnExit()
        {
            fsm.Player.RigidBody.CurrentMoveSpeed = Vector2.Zero;
            fsm.Player.IsFirePressed = false;
            fsm.Player.BulletChargeBar.IsActive = false;
            fsm.Player.CurrentLoadingBulletChargeBarSpeed = 0;
        }
    }
}
