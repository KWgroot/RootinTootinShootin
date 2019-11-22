using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class Sicklies : Projectile
    {
        Vector2 direction;

        public Sicklies(GameObject shooter, Vector2 direction, int speed) : base("ProjectileImages/Sicklies", shooter.Position, new Vector2(0, 0), 100)
        {
            this.direction = direction;
            origin = Center;
            atTarget = false;
            velocity = direction * speed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Angle += 0.2f;
            if (position.X > GameEnvironment.Screen.X - sprite.Width / 2 || position.X < 0 + sprite.Width / 2
                || position.Y > GameEnvironment.Screen.Y - sprite.Height / 2 || position.Y < 0 + sprite.Height / 2)
            {
                visible = false;
            }
        }
    }
}
