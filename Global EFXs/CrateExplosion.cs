using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class CrateExplosion : GameObject
    {
        public Animation AnimationExplosion { get; }

        public CrateExplosion() : base("explosion", 13, layer: DrawLayer.Foreground)
        {
            AnimationExplosion = new Animation(this, 30, false);
        }

        public override void Update()
        {
            AnimationExplosion.Update();
        }
    }
}
