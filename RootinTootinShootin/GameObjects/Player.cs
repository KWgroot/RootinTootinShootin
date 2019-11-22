using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RootinTootinShootin
{
    class Player : RotatingSpriteGameObject
    {
        const int XBORDER = 65, YBORDER = 58;
        public Vector2 moveSpeed = new Vector2(5, 4), direction;
        public int health = 3;

        public Player() : base("PlayerImages/Slav")
        {
            id = "player";
            position = GameEnvironment.Screen.ToVector2() / 2;
            origin = Center;
            //Using velocity makes the character always move for some reason. So we wont use it here and instead use a set value for it.
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.IsKeyDown(Keys.W))
            {
                position.Y -= moveSpeed.Y;
                direction = new Vector2(0, 100);
            }
            if (inputHelper.IsKeyDown(Keys.A))
            {
                position.X -= moveSpeed.X;
                direction = new Vector2(100, 0);
            }
            if (inputHelper.IsKeyDown(Keys.S))
            {
                position.Y += moveSpeed.Y;
                direction = new Vector2(0, -100);
            }
            if (inputHelper.IsKeyDown(Keys.D))
            {
                position.X += moveSpeed.X;
                direction = new Vector2(-100, 0);
            }

            position.X = MathHelper.Clamp(position.X, XBORDER, GameEnvironment.Screen.X - XBORDER);
            position.Y = MathHelper.Clamp(position.Y, YBORDER, GameEnvironment.Screen.Y - YBORDER);
        }
    }
}
