using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class ChinaStar : Enemy
    {
        readonly GameObject target;
        GameObjectList shards;
        public StarShard myShards;
        private bool shardAdded = false;
        readonly int bulletSpeedMod;

        public ChinaStar(GameObject target, GameObjectList shardList, int bulletSpeedMod, float speedMod) : base("EnemyImages/ChinaStar", target, 100, speedMod)
        {
            lookAtTarget = false;
            this.target = target;
            this.shards = shardList;
            this.bulletSpeedMod = bulletSpeedMod;
            scoreValue = 20;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Angle += 0.15f;

            if (!visible && !shardAdded)
            {
                myShards = new StarShard(this, target, 400 / bulletSpeedMod);
                shards.Add(myShards);
                shardAdded = true;
            }
        }
    }
}