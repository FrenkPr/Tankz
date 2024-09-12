using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class WaitState : State
    {
        private Timer timeToNextPlayer;

        public WaitState()
        {
            timeToNextPlayer = new Timer(0.5f, 0.5f);
        }

        public override void OnEnter()
        {
            timeToNextPlayer.Reset();
        }

        public override void Update()
        {
            timeToNextPlayer.DecTime();

            if (timeToNextPlayer.Clock <= 0)
            {
                ((PlayScene)Game.CurrentScene).NextPlayer();
                fsm.Player = ((PlayScene)Game.CurrentScene).CurrentPlayer;

                fsm.GoTo(StateType.Move);
            }
        }
    }
}
