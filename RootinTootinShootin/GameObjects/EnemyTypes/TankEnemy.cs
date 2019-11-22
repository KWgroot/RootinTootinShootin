using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class TankEnemy : Enemy
    {
        public float currentTime = 0f, shootTimer = 1.5f;
        readonly GameObject target;
        GameObjectList bulletList;
        public Ball myBullet;
        readonly int bulletSpeedMod;

        public TankEnemy(GameObject target, GameObjectList bulletList, int bulletSpeedMod, float speedMod) : base("EnemyImages/Tank", target, 50, speedMod)
        {
            this.target = target;
            this.bulletList = bulletList;
            this.bulletSpeedMod = bulletSpeedMod;
            myBullet = new Ball(this, target, 300 / bulletSpeedMod);
            scoreValue = 15;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentTime >= shootTimer)
            {
                if (visible)
                {
                    myBullet = new Ball(this, target, 300 / bulletSpeedMod);
                    bulletList.Add(myBullet);
                }

                currentTime -= shootTimer;
            }

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}