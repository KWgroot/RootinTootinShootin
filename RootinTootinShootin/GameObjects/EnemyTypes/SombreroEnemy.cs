using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class SombreroEnemy : Enemy
    {
        public float currentTime = 0f, shootTimer = 1.5f;
        readonly GameObject target;
        GameObjectList bulletList;
        public Taco myBullet;
        readonly int bulletSpeedMod;

        public SombreroEnemy(GameObject target, GameObjectList bulletList, int bulletSpeedMod, float speedMod) : base("EnemyImages/Sombrero", target, 100, speedMod)
        {
            lookAtTarget = false;
            this.target = target;
            this.bulletList = bulletList;
            this.bulletSpeedMod = bulletSpeedMod;
            myBullet = new Taco(this, target, 150 / bulletSpeedMod);
            scoreValue = 15;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Angle += 0.2f;
            if (currentTime >= shootTimer)
            {
                if (visible)
                {                   
                    for (int iTacos = 0; iTacos < 4; iTacos++)
                    {
                        myBullet = new Taco(this, target, 150 / bulletSpeedMod);
                        bulletList.Add(myBullet);
                    }                    
                }

                currentTime -= shootTimer;
            }

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}