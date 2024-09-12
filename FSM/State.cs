using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    enum StateType
    {
        Wait,
        Move,
        Shoot
    }

    abstract class State
    {
        protected StateMachine fsm;
        
        public virtual void OnEnter(){}

        public virtual void OnExit(){}

        public abstract void Update();

        public void SetStateMachine(StateMachine fsm)
        {
            this.fsm = fsm;
        }
    }
}
