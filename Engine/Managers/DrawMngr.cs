using Aiv.Fast2D;
using OpenTK;
using System.Collections.Generic;

namespace Tankz
{
    enum DrawLayer
    {
        Background,
        MiddleGround,
        Playground,
        Foreground,
        GUI,
        Length
    }

    static class DrawMngr
    {
        private static List<IDrawable>[] drawings;
        private static RenderTexture sceneTexture;
        public readonly static Sprite Scene;
        private static GrayScaleFX grayScaleFX;

        static DrawMngr()
        {
            drawings = new List<IDrawable>[(int)DrawLayer.Length];

            for (int i = 0; i < drawings.Length; i++)
            {
                drawings[i] = new List<IDrawable>();
            }

            sceneTexture = new RenderTexture(Game.WindowWidth, Game.WindowHeight);

            Scene = new Sprite(Game.OrthoWidth, Game.OrthoHeight);

            grayScaleFX = new GrayScaleFX();
        }

        public static void InitSceneCamera()
        {
            Scene.Camera = CameraMngr.GetCamera("GUI");
        }

        public static void Add(IDrawable item)
        {
            drawings[(int)item.DrawLayer].Add(item);
        }

        public static void Remove(IDrawable item)
        {
            drawings[(int)item.DrawLayer].Remove(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < drawings.Length; i++)
            {
                drawings[i].Clear();
            }
        }

        public static void Draw()
        {
            Game.Window.RenderTo(sceneTexture);

            for (int i = 0; i < drawings.Length; i++)
            {
                if (i == (int)DrawLayer.GUI)
                {
                    Game.Window.RenderTo(null);
                    Scene.DrawTexture(sceneTexture);
                    //sceneTexture.ApplyPostProcessingEffect(grayScaleFX);
                }

                for (int j = 0; j < drawings[i].Count; j++)
                {
                    drawings[i][j].Draw();
                }
            }
        }
    }
}
