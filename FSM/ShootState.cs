using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class ShootState : State
    {
        public override void Update()
        {
            if (!(CameraMngr.CurrentBehaviour is FollowTargetBehaviour))
            {
                return;
            }

            if (((FollowTargetBehaviour)CameraMngr.CurrentBehaviour).Target == null)
            {
                fsm.GoTo(StateType.Wait);
            }
        }
    }
}
