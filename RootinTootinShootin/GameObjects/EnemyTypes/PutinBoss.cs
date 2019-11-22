using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class PutinBoss : Enemy
    {
        public int health = 200;
        private float invulnerableTimer = 3f, currentTime = 0f;
        private int shakingCount = 0, shootTimer = 0;
        Vector2 posMod, originalPos;
        private bool vulnerable = true, shakeBoss = false;
        GameObjectList bulletList;
        public Sicklies myBullet;
        Random random = GameEnvironment.Random;

        public PutinBoss(GameObject target, GameObjectList bulletList) : base("EnemyImages/Putin", target, 0, 1)
        {
            position = GameEnvironment.Screen.ToVector2() / 2;
            originalPos = position;
            this.bulletList = bulletList;
            origin = Center;
            myBullet = new Sicklies(this, AngularDirection, 200);
        }

        public override void Update(GameTime gameTime)
        {
            /*
             * Boss Mechanics
             */
            if (health > 175)                        //FASE 1
            {
                if (vulnerable)
                {
                    Angle += 0.04f;
                    if (shootTimer % 10 == 0)
                    {
                        myBullet = new Sicklies(this, AngularDirection, 250);
                        bulletList.Add(myBullet);
                    }
                }                
            }
            else if (health > 150 && health < 175)   //FASE 2
            {
                if (vulnerable)
                {
                    Angle -= 0.04f;
                    if (shootTimer % 8 == 0)
                    {
                        myBullet = new Sicklies(this, AngularDirection, 250);
                        bulletList.Add(myBullet);
                    }
                }                
            }
            else if (health > 100 && health < 150)   //FASE 3
            {
                if (vulnerable)
                {
                    Angle += 0.06f;
                    if (shootTimer % 6 == 0)
                    {
                        myBullet = new Sicklies(this, AngularDirection, 300);
                        bulletList.Add(myBullet);
                        myBullet = new Sicklies(this, AngularDirection, 600);
                        bulletList.Add(myBullet);
                    }
                }
            }
            else if (health > 50 && health < 100)    //FASE 4
            {
                if (vulnerable)
                {
                    Angle -= 0.07f;
                    if (shootTimer % 6 == 0)
                    {
                        myBullet = new Sicklies(this, AngularDirection, 350);
                        bulletList.Add(myBullet);
                        myBullet = new Sicklies(this, AngularDirection, 700);
                        bulletList.Add(myBullet);
                    }
                }                
            }
            else if (health < 50)                   //FASE 5
            {
                if (vulnerable)
                {
                    Angle -= 0.08f;
                    if (shootTimer % 4 == 0)
                    {
                        myBullet = new Sicklies(this, AngularDirection, 400);
                        bulletList.Add(myBullet);
                        myBullet = new Sicklies(this, AngularDirection, 600);
                        bulletList.Add(myBullet);
                        myBullet = new Sicklies(this, AngularDirection, 800);
                        bulletList.Add(myBullet);
                    }
                }
            }

            if (!vulnerable)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                InvincibleTimer();
            }

            if (shakeBoss)
            {                
                if (shakingCount % 2 == 0)
                {
                    position = originalPos;
                    posMod = new Vector2(random.Next(-3, 3), random.Next(-2, 2));
                }
                shakingCount++;

                position += posMod;
            }

            shootTimer++;
        }

        public void Hit()
        {
            if (vulnerable)
            {
                if (health == 175)
                {
                    vulnerable = false;
                }

                if (health == 150)
                {
                    vulnerable = false;
                }

                if (health == 100)
                {
                    vulnerable = false;
                }

                if (health == 50)
                {
                    vulnerable = false;
                }
                health--;           
            }
        }

        public void InvincibleTimer()
        {
            shakeBoss = true;
            shootTimer = 0;

            if (currentTime >= invulnerableTimer)
            {
                position = originalPos;
                vulnerable = true;
                shakeBoss = false;
                currentTime = 0;
            }
        }
    }
}
