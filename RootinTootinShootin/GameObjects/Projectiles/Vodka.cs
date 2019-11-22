using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class Vodka : Projectile
    {
        public Vodka(GameObject shooter, GameObject target, int speed) : base("ProjectileImages/Vodka", shooter.Position, target.Position, speed)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Angle += 0.4f;
        }
    }
}