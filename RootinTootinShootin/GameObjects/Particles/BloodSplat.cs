using Microsoft.Xna.Framework;
using System;

namespace RootinTootinShootin
{
    class BloodSplat : Particle
    {
        public BloodSplat(Vector2 spawnPos, float timeToDie) : base("Particles/Blood", spawnPos, timeToDie)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
