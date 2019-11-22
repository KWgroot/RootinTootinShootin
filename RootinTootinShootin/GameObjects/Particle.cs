using Microsoft.Xna.Framework;
using System;

namespace RootinTootinShootin
{
    class Particle : RotatingSpriteGameObject
    {
        bool alive = true;
        float lifeTimer = 0.0f, timeToDie = 0.0f;

        public Particle(string assetName, Vector2 startPos, float timeToDie) : base(assetName)
        {
            position = startPos;            
            origin = Center;
            this.timeToDie = timeToDie;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (alive)
            {
                lifeTimer++;
                if (lifeTimer == timeToDie)
                {
                    alive = false;
                }
            }
            else
            {
                this.visible = false;
            }
        }
    }
}
