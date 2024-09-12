using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class FollowPointBehaviour:CameraBehaviour
    {
        public float CameraSpeed = 5;

        public FollowPointBehaviour(Camera camera, Vector2 point) : base(camera)
        {
            pointToFollow = point;
        }

        public virtual void SetPointToFollow(Vector2 point)
        {
            pointToFollow = point;
        }

        public override void Update()
        {
            blendFactor = Game.DeltaTime * CameraSpeed;
            base.Update();
        }
    }
}
