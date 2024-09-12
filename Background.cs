using Aiv.Fast2D;
using OpenTK;

namespace Tankz
{
    class Background : IDrawable
    {
        private Texture mainBackgroundTexture;
        private Sprite mainBackgroundSprite;
        private Texture[] textures;
        private Sprite[] sprites;
        public Vector2 Position { get => mainBackgroundSprite.position; }
        public float Width { get => mainBackgroundSprite.Width; }
        public float Height { get => mainBackgroundSprite.Height; }
        public DrawLayer DrawLayer { get; private set; }
        private int mainBackgroundPixelsWidth;
        private int mainBackgroundPixelsHeight;
        private int[] spritesPixelsWidth;
        private int[] spritesPixelsHeight;

        public Background(int numTextures)
        {
            DrawLayer = DrawLayer.Background;

            int numMainBackgroundWidthMultiply = 4;

            mainBackgroundTexture = TextureMngr.GetTexture("bg_playground");
            
            mainBackgroundPixelsWidth = mainBackgroundTexture.Width * numMainBackgroundWidthMultiply;
            mainBackgroundPixelsHeight = mainBackgroundTexture.Height;
            
            mainBackgroundSprite = new Sprite(Game.PixelsToUnits(mainBackgroundTexture.Width * numMainBackgroundWidthMultiply), Game.PixelsToUnits(mainBackgroundTexture.Height));

            mainBackgroundSprite.position.Y = 4.6f;

            textures = new Texture[numTextures];
            sprites = new Sprite[numTextures];
            spritesPixelsWidth = new int[numTextures];
            spritesPixelsHeight = new int[numTextures];

            float[] positions = { 3.72f, -1.9f };
            int[] numSpritesWidthMultiply = { 6, 6 };

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = TextureMngr.GetTexture($"bg_{i}");

                spritesPixelsWidth[i] = textures[i].Width * numSpritesWidthMultiply[i];
                spritesPixelsHeight[i] = textures[i].Height;

                float width = Game.PixelsToUnits(textures[i].Width * numSpritesWidthMultiply[i]);
                float height = Game.PixelsToUnits(textures[i].Height);

                sprites[i] = new Sprite(width, height);
                sprites[i].position.Y = positions[i];
            }

            DrawMngr.Add(this);
        }

        public void InitCameras()
        {
            mainBackgroundSprite.Camera = CameraMngr.GetCamera("MainBg");

            for (int i = 0; i < textures.Length; i++)
            {
                sprites[i].Camera = CameraMngr.GetCamera($"Bg_{i}");
            }
        }

        public void Draw()
        {
            for (int i = textures.Length - 1; i >= 0; i--)
            {
                sprites[i].DrawTexture(textures[i], 0, 0, spritesPixelsWidth[i], spritesPixelsHeight[i]);
            }

            mainBackgroundSprite.DrawTexture(mainBackgroundTexture, 0, 0, mainBackgroundPixelsWidth, mainBackgroundPixelsHeight);
        }
    }
}