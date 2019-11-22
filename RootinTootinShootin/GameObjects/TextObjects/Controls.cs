using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class Controls : TextGameObject
    {
        public float currentTime = 0f, removeTimer = 5f;
        private Vector2 stringSize;

        public Controls() : base("Fonts/Wave")
        {
            text = "Controls:\nMove mouse to aim.\nLMB to shoot.\nWASD to move.";
            color = Color.ForestGreen;
            stringSize = spriteFont.MeasureString(text);
            position = new Vector2(((GameEnvironment.Screen.X / 2) - stringSize.X / 2), GameEnvironment.Screen.Y - spriteFont.Texture.Height * 1.3f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentTime >= removeTimer)
            {
                visible = false;
            }
            else
            {
                visible = true;
            }

            stringSize = spriteFont.MeasureString(text);
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}