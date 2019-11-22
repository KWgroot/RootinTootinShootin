using Microsoft.Xna.Framework;
using System;

namespace RootinTootinShootin
{
    class GlassPart : Particle
    {
        public GlassPart(Vector2 spawnPos, float timeToDie) : base("Particles/Glass", spawnPos, timeToDie)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
