using System.Collections.Generic;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz
{
    static class Game
    {
        public static Window Window { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static int WindowWidth { get { return Window.Width; } }
        public static int WindowHeight { get { return Window.Height; } }
        public static int HalfWindowWidth { get { return (int)(Window.Width * 0.5f); } }
        public static int HalfWindowHeight { get { return (int)(Window.Height * 0.5f); } }
        public static float OrthoWidth { get { return Window.OrthoWidth; } }
        public static float OrthoHeight { get { return Window.OrthoHeight; } }
        public static float OrthoHalfWidth { get { return Window.OrthoWidth * 0.5f; } }
        public static float OrthoHalfHeight { get { return Window.OrthoHeight * 0.5f; } }
        public static Scene CurrentScene;
        public static List<KeyboardController> KeyboardCtrls;
        public static List<JoypadController> JoypadCtrls;
        public static int NumMaxPlayers { get; private set; }
        private static float optimalUnitSize;
        private static float optimalScreenHeight;
        public static Vector2 MousePosition { get { return Window.MousePosition; } }
        public static bool IsMouseLeftPressed { get; private set; }
        public static Vector2 LastMousePositionClicked { get; private set; }
        public static Vector2 ScreenCenter { get => new Vector2(OrthoHalfWidth, OrthoHalfHeight); }

        public static void Init()
        {
            Window = new Window(1280, 720, "Tankz");
            Window.Position = Vector2.Zero;
            Window.SetDefaultViewportOrthographicSize(10);
            optimalScreenHeight = 1080;
            optimalUnitSize = optimalScreenHeight / Window.OrthoHeight;

            Window.SetIcon("Assets/TankzIco.ico");

            //System.Console.WriteLine(Window.CurrentViewportOrthographicSize + "\n" + Window.OrthoWidth + "\n" + Window.OrthoHeight);

            NumMaxPlayers = 2;

            List<KeyCode>[] keysConfig = new List<KeyCode>[NumMaxPlayers];

            //the order of keys config is: left, right, cannonRotationLeft, cannonRotationRight and fire

            //player 1 config
            keysConfig[0] = new List<KeyCode>
            {
                KeyCode.A,
                KeyCode.D,
                KeyCode.Up,
                KeyCode.Down,
                KeyCode.Space,
                KeyCode.Tab
            };

            //player 2 config
            keysConfig[1] = new List<KeyCode>
            {
                KeyCode.Left,
                KeyCode.Right,
                KeyCode.Q,
                KeyCode.E,
                KeyCode.Num2,
                KeyCode.Num1
            };

            string[] joypadsConnected = Window.Joysticks;
            JoypadCtrls = new List<JoypadController>(NumMaxPlayers);
            KeyboardCtrls = new List<KeyboardController>(NumMaxPlayers);

            for (int i = 0; i < NumMaxPlayers; i++)
            {
                KeyboardCtrls.Add(new KeyboardController(i, keysConfig[i]));

                //System.Console.WriteLine(joypadsConnected[i] + "pos: " + i);

                if (joypadsConnected[i] != null && joypadsConnected[i] != "Unmapped Controller")
                {
                    JoypadCtrls.Add(new PS4Controller(i));
                }
            }

            CurrentScene = new PlayScene();
        }

        public static void Run()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                //System.Console.WriteLine("Mouse X: " + (Window.MouseX) + "\nMouse Y: " + (Window.MouseY));

                //for (int i = 0; i < JoypadCtrls.Count; i++)
                //{
                //    System.Console.WriteLine(Window.JoystickDebug(i));
                //}

                if (CurrentScene.IsPlaying)
                {
                    if (Window.MouseLeft)
                    {
                        if (!IsMouseLeftPressed)
                        {
                            LastMousePositionClicked = Window.MousePosition;
                            IsMouseLeftPressed = true;
                        }
                    }

                    else if (IsMouseLeftPressed)
                    {
                        IsMouseLeftPressed = false;
                    }

                    if (Window.MousePosition == LastMousePositionClicked &&
                        Window.MouseX >= -0.1178782f &&
                        Window.MouseX <= 17.84246f &&
                        Window.MouseY >= -0.476787925f &&
                        Window.MouseY <= -0.01254705f &&
                        IsMouseLeftPressed &&
                        !Window.IsFullScreen())
                    {
                        Window.Update();
                        continue;
                    }

                    CurrentScene.Update();

                    //window update
                    Window.Update();
                }

                else
                {
                    CurrentScene.OnExit();

                    if (CurrentScene.NextScene != null)
                    {
                        CurrentScene = CurrentScene.NextScene;
                        CurrentScene.Start();
                    }

                    else
                    {
                        return;
                    }
                }
            }
        }

        public static float PixelsToUnits(float val)
        {
            return val / optimalUnitSize;
        }
    }
}
