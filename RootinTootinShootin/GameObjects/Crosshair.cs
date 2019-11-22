using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RootinTootinShootin
{
    class Crosshair : SpriteGameObject
    {
        public Crosshair() : base("PlayerImages/Crosshair")
        {
            Mouse.SetPosition(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
            origin = Center;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            position = inputHelper.MousePosition;

            position.X = MathHelper.Clamp(position.X, 1, GameEnvironment.Screen.X-1);
            position.Y = MathHelper.Clamp(position.Y, 1, GameEnvironment.Screen.Y-1);
        }
    }
}