using Microsoft.Xna.Framework;
using System;

namespace RootinTootinShootin
{
    class Enemy : RotatingSpriteGameObject
    {
        Random random = GameEnvironment.Random;
        GameObject target;
        public Coin myCoin;
        public bool lookAtTarget = true;
        private readonly float speed;
        public int scoreValue;

        public Enemy(String assetName, GameObject target, int speed, float speedMod) : base(assetName)
        {
            this.target = target;
            this.speed = speed / speedMod;
            origin = Center;
            myCoin = new Coin(position);
            SetSpawn();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (lookAtTarget)
            {
                LookAt(target);
            }
            velocity = Vector2.Normalize(Vector2.Subtract(target.Position, position)) * speed;
        }

        public void SetSpawn()
        {
            int spawnValue = random.Next(0, 7);

            switch (spawnValue)
            {
                case 0:
                    position = new Vector2(0, 385);
                    break;

                case 1:
                    position = new Vector2(0, 160);
                    break;

                case 2:
                    position = new Vector2(GameEnvironment.Screen.X, 565);
                    break;

                case 3:
                    position = new Vector2(320, GameEnvironment.Screen.Y);
                    break;

                case 4:
                    position = new Vector2(880, GameEnvironment.Screen.Y);
                    break;

                case 5:
                    position = new Vector2(640, 0);
                    break;

                case 6:
                    position = new Vector2(1120, 0);
                    break;
            }
        }

        public void SpawnCoin(GameObjectList objectList)
        {
            myCoin = new Coin(position);
            objectList.Add(myCoin);
        }
    }
}
