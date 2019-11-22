using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootinTootinShootin
{
    class VictoryState : GameObjectList
    {
        public VictoryState()
        {
            this.Add(new SpriteGameObject("BackgroundImages/Victory"));
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.KeyPressed(Keys.Space))
            {
                GameEnvironment.GameStateManager.SwitchTo("StartingState");
            }
        }
    }
}
