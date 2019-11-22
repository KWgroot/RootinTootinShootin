using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class Wave : TextGameObject
    {
        public float currentTime = 0f, removeTimer = 3f;
        private Vector2 stringSize;

        public Wave() : base("Fonts/Wave")
        {
            text = "   Wave: 1   ";
            color = Color.Green;
            stringSize = spriteFont.MeasureString(text);
            position = new Vector2(((GameEnvironment.Screen.X / 2) - stringSize.X/2), 50);
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