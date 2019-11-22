using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootinTootinShootin
{
    class Life : SpriteGameObject
    {
        public int myNumber;

        public Life(int lifeNumber) : base("PlayerImages/Life")
        {
            position = new Vector2(GameEnvironment.Screen.X - lifeNumber * sprite.Width - 15, GameEnvironment.Screen.Y - sprite.Height);
            myNumber = lifeNumber;
        }
    }
}
