using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class StarShard : Projectile
    {
        public StarShard(GameObject origin, GameObject target, int speed) : base("ProjectileImages/StarShard", origin.Position, target.Position, speed)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Angle += 0.4f;
        }
    }
}