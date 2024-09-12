using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK;

namespace Tankz
{
    class PlayScene : Scene
    {
        public Background Background;
        public List<Player> Players;
        private int currentPlayerIndex;
        public Player CurrentPlayer { get { return Players.Count > 0 ? Players[currentPlayerIndex] : null; } }
        public int NumPlayersAlive;
        public bool EndGame;
        public static List<Tile> Tiles { get; private set; }

        public PlayScene() : base()
        {

        }

        private void LoadAssets()
        {
            //textures loading
            TextureMngr.AddTexture("tankBody", "Assets/tanks_tankGreen_body1.png");
            TextureMngr.AddTexture("tankTracks", "Assets/tanks_tankTracks1.png");
            TextureMngr.AddTexture("tankCannon", "Assets/tanks_turret2.png");
            TextureMngr.AddTexture("frameLoadingBar", "Assets/loadingBar_frame.png");
            TextureMngr.AddTexture("loadingBar", "Assets/loadingBar_bar.png");
            TextureMngr.AddTexture("wpnBarFrame", "Assets/weapons_GUI_frame.png");
            TextureMngr.AddTexture("wpnSelectedGUI", "Assets/weapon_GUI_selection.png");
            TextureMngr.AddTexture("defaultBullet", "Assets/tank_bullet1.png");
            TextureMngr.AddTexture("missileBullet", "Assets/tank_bullet2.png");
            TextureMngr.AddTexture("defaultBulletGUIico", "Assets/bullet_ico.png");
            TextureMngr.AddTexture("missileBulletGUIico", "Assets/missile_ico.png");
            TextureMngr.AddTexture("crate", "Assets/crate.png");
            TextureMngr.AddTexture("explosion", "Assets/explosion.png");

            TextureMngr.AddTexture("bg_playground", "Assets/bg_playground.png", true);

            for (int i = 0; i < 2; i++)
            {
                TextureMngr.AddTexture($"bg_{i}", $"Assets/bg_{i}.png", true);
            }
            //end textures loading

            //sounds loading
            SoundMngr.AddClip("MissileBulletWhistle", "Assets/sounds/whistle.ogg");
            SoundMngr.AddClip("MissileBulletEngineStart", "Assets/sounds/engineStart.wav");

            SoundMngr.AddClip("defaultBulletShoot", "Assets/sounds/cannonShoot.wav");
            
            SoundMngr.AddClip("woodCrack_1", "Assets/sounds/wood_crack_1.ogg");
            SoundMngr.AddClip("woodCrack_2", "Assets/sounds/wood_crack_2.ogg");
        }

        public override void Start()
        {
            LoadAssets();

            FontMngr.AddFont("comics", "comics", "Assets/comics.png", 10, 10, 32);

            Background = new Background(2);

            CameraMngr.Init(null, new CameraLimits(Background.Position.X + Background.Width - 10, Background.Position.X, Game.OrthoHalfHeight, Game.OrthoHeight * 0.45f));

            CameraMngr.AddCamera("GUI", new Aiv.Fast2D.Camera());
            CameraMngr.AddCamera("MainBg", cameraSpeed: 0.7f);
            CameraMngr.AddCamera("Bg_0", cameraSpeed: 0.8f);
            CameraMngr.AddCamera("Bg_1", cameraSpeed: 0.85f);
            Background.InitCameras();
            DrawMngr.InitSceneCamera();

            Players = new List<Player>(Game.NumMaxPlayers);
            NumPlayersAlive = Game.NumMaxPlayers;

            CameraMngr.MainCamera.position.X = Background.Position.X;

            float playerPositionX = CameraMngr.MainCamera.position.X - Game.OrthoHalfWidth + 0.5f;
            float playerTextPositionX = 1 + 0.1f;
            float energyBarPositionX = 1;

            for (int i = 0; i < Game.NumMaxPlayers; i++)
            {
                Players.Add(new Player(i));
                Players[i].X = playerPositionX;
                Players[i].EnergyBar.Position = new Vector2(energyBarPositionX, 0.7f);
                Players[i].WeaponsHUD.Position = new Vector2(energyBarPositionX, 1.1f);
                Players[i].WeaponsHUD.InitItemsPosition();

                TextMngr.AddText($"player{i}", $"PLAYER {i + 1}", new Vector2(playerTextPositionX, 0.5f), 40, 40);

                playerPositionX += Game.OrthoHalfWidth;
                playerTextPositionX += 5;
                energyBarPositionX += 5;
            }

            CameraMngr.SetTarget(CurrentPlayer, false);

            BulletMngr.Init();
            //EnemyMngr.Init();

            //EnemyMngr.SpawnEnemy();

            Tiles = new List<Tile>();

            Tiles.Add(new Tile("crate", Players[0].Position + new Vector2(1.5f, 0)));
            Tiles.Add(new Tile("crate", Players[0].Position + new Vector2(1.5f, -1)));
            Tiles.Add(new Tile("crate", Players[0].Position + new Vector2(1.5f, -2)));
            Tiles.Add(new Tile("crate", Players[0].Position + new Vector2(1.5f, -3)));

            Players[1].Position = Tiles[3].Position;
            Players[1].Y -= 0.5f;

            //Game.Window.AddPostProcessingEffect(new BlackBandFX());
            //Game.Window.AddPostProcessingEffect(new GrayScaleFX());

            base.Start();
        }

        public override void Update()
        {
            //update
            PhysicsMngr.Update();
            UpdateMngr.Update();
            CameraMngr.Update();

            PhysicsMngr.CheckCollisions();

            //draw
            DrawMngr.Draw();
            DebugMngr.Draw();

            if (EndGame)
            {
                IsPlaying = false;

                Game.Window.Update();
                Thread.Sleep(2000);
            }
        }

        public void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
        }

        public override void OnExit()
        {
            //EnemyMngr.ClearAll();
            BulletMngr.ClearAll();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            FontMngr.ClearAll();
            DrawMngr.ClearAll();
            TextureMngr.ClearAll();
            SoundMngr.ClearAll();
            DebugMngr.ClearAll();

            Players = null;
            Background = null;
            NextScene = null;
            Tiles = null;
        }
    }
}
