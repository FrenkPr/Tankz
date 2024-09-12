using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz
{
    struct CameraLimits
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    static class CameraMngr
    {
        public static Camera MainCamera;
        private static Dictionary<string, Tuple<Camera, float>> subCameras;

        private static CameraBehaviour[] behaviours;
        public static CameraBehaviour CurrentBehaviour { get; private set; }

        //public static GameObject Target;
        //private static float cameraSpeed;
        public static CameraLimits CameraLimits;
        public static float HalfDiagonalSquared { get => MainCamera.pivot.LengthSquared; }

        public static void Init(GameObject target, CameraLimits cameraLimits)
        {
            MainCamera = new Camera(Game.OrthoHalfWidth, Game.OrthoHalfHeight);
            MainCamera.pivot = Game.ScreenCenter;
            //Target = target;
            //cameraSpeed = 5;
            CameraLimits = cameraLimits;

            subCameras = new Dictionary<string, Tuple<Camera, float>>();

            behaviours = new CameraBehaviour[(int)CameraBehaviourType.Length];

            behaviours[(int)CameraBehaviourType.FollowTarget] = new FollowTargetBehaviour(MainCamera, target);
            behaviours[(int)CameraBehaviourType.FollowPoint] = new FollowPointBehaviour(MainCamera, Vector2.Zero);
            behaviours[(int)CameraBehaviourType.MoveToPoint] = new MoveToPointBehaviour(MainCamera);

            CurrentBehaviour = behaviours[(int)CameraBehaviourType.FollowTarget];
        }

        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            subCameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (subCameras.ContainsKey(cameraName))
            {
                return subCameras[cameraName].Item1;
            }

            return null;
        }

        public static float GetCameraSpeed(string cameraName)
        {
            if (subCameras.ContainsKey(cameraName))
            {
                return subCameras[cameraName].Item2;
            }

            return 0;
        }

        public static void SetTarget(GameObject target, bool changeBehaviour = true)
        {
            FollowTargetBehaviour followTargetBehaviour = (FollowTargetBehaviour)behaviours[(int)CameraBehaviourType.FollowTarget];
            followTargetBehaviour.Target = target;

            if (changeBehaviour)
            {
                CurrentBehaviour = followTargetBehaviour;
            }
        }

        public static void SetPointToFollow(Vector2 point, bool changeBehaviour = true)
        {
            FollowPointBehaviour followPointBehaviour = (FollowPointBehaviour)behaviours[(int)CameraBehaviourType.FollowPoint];
            followPointBehaviour.SetPointToFollow(point);

            if (changeBehaviour)
            {
                CurrentBehaviour = followPointBehaviour;
            }
        }

        public static void MoveTo(Vector2 point, float time)
        {
            CurrentBehaviour = behaviours[(int)CameraBehaviourType.MoveToPoint];
            ((MoveToPointBehaviour)CurrentBehaviour).MoveTo(point, time);
        }

        public static void OnMovementEnd()
        {
            CurrentBehaviour = behaviours[(int)CameraBehaviourType.FollowTarget];
        }

        public static void Update()
        {
            //if (Target == null)
            //{
            //    return;
            //}

            Vector2 oldCameraPos = MainCamera.position;
            
            //MainCamera.position = Vector2.Lerp(MainCamera.position, Target.Position, cameraSpeed * Game.DeltaTime);
            CurrentBehaviour.Update();
            FixMainCameraOutOfBounds();
            
            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            UpdateSubCameras(cameraDelta);
        }

        private static void UpdateSubCameras(Vector2 cameraDelta)
        {
            if (cameraDelta != Vector2.Zero)
            {
                //camera moved
                foreach (var camera in subCameras)
                {
                    camera.Value.Item1.position += cameraDelta * camera.Value.Item2;  //camera position += delta * cameraSpeed
                }
            }
        }

        private static void FixMainCameraOutOfBounds()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }
    }
}
