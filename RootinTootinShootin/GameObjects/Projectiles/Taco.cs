using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class Taco : Projectile
    {
        private static int tacoNumber = 0;
        private static bool first = true;

        public Taco(GameObject origin, GameObject target, int speed) : base("ProjectileImages/Taco", origin.Position, target.Position, speed)
        {
            atTarget = false;
            switch (tacoNumber)
            {
                case 1:
                    velocity = new Vector2(speed, speed);
                    tacoNumber++;
                    break;

                case 2:
                    velocity = new Vector2(-speed, speed);
                    tacoNumber++;
                    break;

                case 3:
                    velocity = new Vector2(speed, -speed);
                    tacoNumber++;
                    break;

                case 4:
                    velocity = new Vector2(-speed, -speed);
                    tacoNumber = 1;
                    break;
            }

            if (first)
            {
                tacoNumber++;
                first = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Angle += 0.2f;

            if (position.X > GameEnvironment.Screen.X - sprite.Width/2 || position.X < 0 + sprite.Width/2 
                || position.Y > GameEnvironment.Screen.Y - sprite.Height/2 || position.Y < 0 + sprite.Height/2)
            {
                visible = false;
            }
        }
    }
}