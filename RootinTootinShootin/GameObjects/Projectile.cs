using Microsoft.Xna.Framework;
using System;

namespace RootinTootinShootin
{
    class Projectile : RotatingSpriteGameObject
    {
        public Vector2 targetPos, startPos;
        int speed;
        public bool atTarget = true;
        public bool shattered = false;
        public bool gone = false;

        public Projectile(String assetName, Vector2 startPos, Vector2 targetPos, int speed) : base(assetName)
        {
            position = startPos;
            this.targetPos = targetPos;
            this.startPos = startPos;
            this.speed = speed;
            origin = Center;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (atTarget)
            {
                velocity = Vector2.Normalize(Vector2.Subtract(targetPos, startPos)) * speed;
                removeIfTargetHit();
            }            
        }

        public void removeIfTargetHit()
        {
            if (!gone)
            {
                if (velocity.X > 0)
                {
                    if (position.X >= targetPos.X)
                    {
                        visible = false;
                        shattered = true;
                    }
                }
                else
                {
                    if (position.X <= targetPos.X)
                    {
                        visible = false;
                        shattered = true;
                    }
                }

                if (velocity.Y > 0)
                {
                    if (position.Y >= targetPos.Y)
                    {
                        visible = false;
                        shattered = true;
                    }
                }
                else
                {
                    if (position.Y <= targetPos.Y)
                    {
                        visible = false;
                        shattered = true;
                    }
                }
            }
        }
    }
}