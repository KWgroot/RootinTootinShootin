using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class Score : TextGameObject
    {
        public Score() : base("Fonts/Score")
        {
            text = 0.ToString();
            position = new Vector2(30, 40);
        }
    }
}