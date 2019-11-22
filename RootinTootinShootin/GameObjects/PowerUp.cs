using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootinTootinShootin
{
    class PowerUp : SpriteGameObject
    {
        public PowerUp(String assetName, int placeNumber) : base(assetName)
        {
            position = new Vector2(((GameEnvironment.Screen.X / 4) * placeNumber) - sprite.Width / 2, GameEnvironment.Screen.Y / 4);
        }
    }
}
