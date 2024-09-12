using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    enum CameraBehaviourType
    {
        FollowTarget,
        FollowPoint,
        MoveToPoint,
        Length
    }

    abstract class CameraBehaviour
    {
        protected Camera camera;
        protected Vector2 pointToFollow;
        protected float blendFactor;

        public CameraBehaviour(Camera camera)
        {
            this.camera = camera;
        }

        public virtual void Update()
        {
            camera.position = Vector2.Lerp(camera.position, pointToFollow, blendFactor);
        }
    }
}
