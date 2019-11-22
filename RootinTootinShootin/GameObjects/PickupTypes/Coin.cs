using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootinTootinShootin
{
    class Coin : RotatingSpriteGameObject
    {
        private float currentTime = 0f;
        private readonly float timeVisible = 10f;
        private float blinkTimer = 0f;
        public bool blinkEnabled = true, removed = false;

        public Coin(Vector2 spawnPos) : base("PickupImages/Coin")
        {
            position = spawnPos;
            origin = Center;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Player player = GameWorld.Find("player") as Player;

            if ((player.Position - position).Length() < 200)
            {
                this.velocity = (player.Position - this.position);
                this.velocity.Normalize();
                this.velocity *= 400;
            }
            else
                velocity = Vector2.Zero;

            Angle += 0.1f;

            if (currentTime >= timeVisible)
            {
                visible = false;
            }
            else if (currentTime >= timeVisible - 5 && currentTime < timeVisible - 2)            
            {
                //MEDIUM BLINK
                Blink(0.4f);
            }
            else if (currentTime >= timeVisible - 2 && currentTime < timeVisible)
            {
                //FAST BLINK
                Blink(0.1f);
            }

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Blink(float interval)
        {
            if (!removed)
            {
                if (blinkEnabled)
                {
                    blinkTimer = currentTime;
                    blinkEnabled = false;
                }

                if (currentTime >= blinkTimer + interval && currentTime < blinkTimer + (interval * 2))
                {
                    visible = true;
                }
                else if (currentTime >= blinkTimer + (interval * 2))
                {
                    visible = false;
                    blinkEnabled = true;
                }
            }
        }
    }
}
