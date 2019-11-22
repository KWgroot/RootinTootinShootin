using System;
using Microsoft.Xna.Framework;

namespace RootinTootinShootin
{
    class PlayingState : GameObjectList
    {
        Player thePlayer = new Player();
        PutinBoss finalBoss;
        Wave waveText = new Wave();
        Score scoreText = new Score();
        readonly Controls controls = new Controls();
        GameObjectList theProjectiles = new GameObjectList(), theEnemies = new GameObjectList(), thePowerUps = new GameObjectList(), theLives = new GameObjectList(), particles = new GameObjectList();
        readonly Crosshair theCrosshair = new Crosshair();
        private int waveCount = 8, enemySpawned = 0, enemyAmount = 10, enemySlain = 0, enemyType, scoreCount = 300, coinSpawn = 0, powerUpSpawn = 0, placeNumber = 1,
            vodkaSpeedMod = 1, enemyBulletSpeedMod = 1, shakingCount = 0;
        private const int FINALWAVE = 10;
        private const float BREAKTIMER = 10f;
        private float currentTime = 0f, spawnTimer = 1.4f, prevSpawnTimer, enemySpeedMod = 1, shakeTimeCount = 0f, shakeTimer;
        private bool resetTimer = false, powerUpsAdded = false, clearPowerUps = false, shakeScreen = false, smallShake = false, shakeStart = false;
        Vector2 originalPos, posMod;
        Random random = GameEnvironment.Random;

        public PlayingState()
        {
            this.Add(new SpriteGameObject("BackgroundImages/PlayingBackground"));
            this.Add(particles);
            this.Add(thePlayer);
            finalBoss = new PutinBoss(thePlayer, theProjectiles);
            this.Add(theProjectiles);
            this.Add(theEnemies);
            this.Add(thePowerUps);
            this.Add(theLives);
            this.Add(waveText);
            this.Add(scoreText);
            this.Add(controls); 
            this.Add(theCrosshair);

            for (int iLives = 0; iLives < thePlayer.health; iLives++)
            {
                theLives.Add(new Life(iLives+1));
            }

            originalPos = this.position;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.MouseLeftButtonPressed())
            {
                theProjectiles.Add(new Vodka(thePlayer, theCrosshair, 500 * vodkaSpeedMod));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            thePlayer.LookAt(theCrosshair);

            if (currentTime >= spawnTimer)
            {
                HandleWaves();
                currentTime -= spawnTimer;
                if (resetTimer)
                {
                    spawnTimer = prevSpawnTimer;
                    resetTimer = false;
                    if (waveCount != FINALWAVE)
                    {
                        waveText.Text = "Wave: " + waveCount.ToString();
                    }
                    else
                    {
                        waveText.Text = "FINAL WAVE!\nKILL PUTIN!";
                    }
                    waveText.currentTime = 0f;
                }
            }
            else if (resetTimer)
            {                
                if (currentTime >= spawnTimer - 0.5f)
                {
                    //CLEAR EVERYTHING BEFORE NEW ROUND
                    theEnemies.Children.Clear();
                    theProjectiles.Children.Clear();
                    thePowerUps.Children.Clear();
                    powerUpsAdded = false;

                    if (waveCount == FINALWAVE)
                    {
                        shakeScreen = true;
                    }
                }
                else
                {
                    if (!powerUpsAdded)
                    {
                        if (currentTime >= spawnTimer - 13.5f)
                        {
                            for (int iPowerUps = 0; iPowerUps < 3; iPowerUps++)
                            {
                                AddPowerUps();
                            }

                            powerUpsAdded = true;
                        }                            
                    }                    
                }
            }

            foreach (PowerUp powerUp in thePowerUps.Children)
            {
                if (thePlayer.CollidesWith(powerUp) && scoreCount >= 400)
                {
                    scoreCount -= 400;

                    if (powerUp is BulletSpeedPowerUp)
                    {
                        vodkaSpeedMod *= 2;
                        powerUp.Visible = false;
                        if (scoreCount < 400)
                        {
                            clearPowerUps = true;
                        }                        
                    }
                    else if (powerUp is EnemyBulletSpeedDown)
                    {
                        enemyBulletSpeedMod *= 2;
                        powerUp.Visible = false;
                        if (scoreCount < 400)
                        {
                            clearPowerUps = true;
                        }
                    }
                    else if (powerUp is SpeedPowerUp)
                    {
                        thePlayer.moveSpeed *= 2;
                        powerUp.Visible = false;
                        if (scoreCount < 400)
                        {
                            clearPowerUps = true;
                        }
                    }
                    else if (powerUp is EnemySpeedDown)
                    {
                        enemySpeedMod *= 1.5f;
                        powerUp.Visible = false;
                        if (scoreCount < 400)
                        {
                            clearPowerUps = true;
                        }
                    }
                    else if (powerUp is HealthPowerUp)
                    {
                        if (thePlayer.health < 3)
                        {
                            thePlayer.health++;
                        }                        
                        powerUp.Visible = false;
                        if (scoreCount < 400)
                        {
                            clearPowerUps = true;
                        }
                    }
                }
                else if (thePlayer.CollidesWith(powerUp) && scoreCount < 400)
                {
                    //Insufficient funds pop-up text
                }
            }

            if (clearPowerUps)
            {
                thePowerUps.Children.Clear();
                clearPowerUps = false;
            }

            foreach (Enemy enemy in theEnemies.Children)
            {
                if (thePlayer.CollidesWith(enemy))
                {
                    if (!(enemy is PutinBoss))
                    {
                  
                        enemySlain++;
                        enemy.Visible = false;
                        KillPlayer();
                       
                    }
                    else
                    {
                        KillPlayer();
                        thePlayer.Position += thePlayer.direction;
                    }                        
                }

                foreach (Projectile projectile in theProjectiles.Children)
                {
                    if (projectile is Vodka)
                    {
                        if (projectile is Vodka && projectile.shattered)
                        {
                            particles.Add(new GlassPart(projectile.targetPos, 50));
                            projectile.gone = true;
                            projectile.shattered = false;
                        }

                        if (enemy.CollidesWith(projectile))
                        {
                            if (!(enemy is PutinBoss))
                            {
                                particles.Add(new BloodSplat(enemy.Position, 500));
                                projectile.targetPos = projectile.Position;
                                enemy.Visible = false;
                                projectile.Visible = false;
                                enemySlain++;
                                scoreCount += enemy.scoreValue;
                                SpawnCoin(enemy);
                                smallShake = true;
                            }
                            else
                            {
                                projectile.Visible = false;
                                finalBoss.Hit();
                                if (finalBoss.health == 0)
                                {
                                    enemySlain++;
                                }
                            }
                        }
                    }

                    if (projectile is Ball || projectile is StarShard || projectile is Taco || projectile is Sicklies)
                    {
                        if (thePlayer.CollidesWith(projectile))
                        {
                            KillPlayer();
                            projectile.Visible = false;
                        }
                    }
                }

                if (enemy.myCoin.CollidesWith(thePlayer))
                {
                    scoreCount += 10;
                    enemy.myCoin.Visible = false;
                    enemy.myCoin.removed = true;
                }
            } 

            foreach (Life life in theLives.Children)
            {
                if (life.myNumber > thePlayer.health)
                {
                    life.Visible = false;
                }
                else
                {
                    life.Visible = true;
                }
            }

            if (thePlayer.health == 0)
            {
                ResetGame();
                GameEnvironment.GameStateManager.SwitchTo("GameOverState");
            } //End the game

            if (enemySlain == enemyAmount)
            {
                enemySpawned = 0;
                enemySlain = 0;

                if (enemyAmount < 30)
                {
                    enemyAmount += (waveCount * 2);
                }
                
                waveCount++;
                if (spawnTimer > 0.8f)
                {
                    spawnTimer -= 0.1f;
                }

                prevSpawnTimer = spawnTimer;
                spawnTimer = BREAKTIMER;
                resetTimer = true;
                waveText.Text = "Wave Cleared!\n Squat Time!";
                waveText.currentTime = 0f;
                ResetPowerUps();
                scoreCount += 50;

                if (waveCount-1 == FINALWAVE)
                {
                    ResetGame();
                    GameEnvironment.GameStateManager.SwitchTo("VictoryState");
                }
            } //A reset after every round

            if (shakeScreen)
            {
                if (!shakeStart)
                {
                    shakeTimer = shakeTimeCount;
                    shakeStart = true;
                }

                if (shakingCount % 2 == 0)
                {
                    position = originalPos;
                    posMod = new Vector2(random.Next(-3, 3), random.Next(-2, 2)) * ((shakeTimer + 0.5f) - shakeTimeCount)*10;
                }
                shakingCount++;

                position += posMod;
                
                if (shakeTimeCount >= shakeTimer + 0.5f)
                {
                    shakeScreen = false;
                    shakeStart = false;
                    position = originalPos;
                    shakingCount = 0;
                }
            } //Shake the screen

            if (smallShake)
            {
                if (!shakeStart)
                {
                    shakeTimer = shakeTimeCount;
                    shakeStart = true;
                }

                if (shakingCount % 2 == 0)
                {
                    position = originalPos;
                    posMod = new Vector2(random.Next(-1, 1), random.Next(-1, 1)) * ((shakeTimer + 0.5f) - shakeTimeCount) * 5;
                }
                shakingCount++;

                position += posMod;

                if (shakeTimeCount >= shakeTimer + 0.5f)
                {
                    smallShake = false;
                    shakeStart = false;
                    position = originalPos;
                    shakingCount = 0;
                }
            } //Shake the screen

            shakeTimeCount += (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            scoreText.Text = "$: " + scoreCount.ToString() + ",-";
        }

        private void KillPlayer()
        {
            thePlayer.health--;
            shakeScreen = true;
        }

        public void SpawnCoin(Enemy enemy)
        {
            coinSpawn = random.Next(1, 7);

            switch (coinSpawn)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    enemy.SpawnCoin(this);
                    break;
                case 5:
                    enemy.SpawnCoin(this);
                    break;
                case 6:
                    enemy.SpawnCoin(this);
                    break;
            }

            coinSpawn = 0;
        }

        public void ResetGame()
        {
            thePlayer.health = 3;
            waveCount = 1;
            enemySpawned = 0;
            enemyAmount = 10;
            enemySlain = 0;
            scoreCount = 0;
            coinSpawn = 0;
            powerUpSpawn = 0;
            placeNumber = 1;
            vodkaSpeedMod = 1;
            enemyBulletSpeedMod = 1;
            currentTime = 0f;
            spawnTimer = 1.4f;
            enemySpeedMod = 1;            
            position = originalPos;
            shakingCount = 0;
            shakeScreen = false;
            shakeStart = false;
            resetTimer = false;
            powerUpsAdded = false;
            clearPowerUps = false;

            foreach (Enemy coin in theEnemies.Children)
            {
                coin.myCoin.Visible = false;
                coin.myCoin.removed = true;
            }

            theEnemies.Children.Clear();
            thePowerUps.Children.Clear();
            theProjectiles.Children.Clear();

            foreach (Life life in theLives.Children)
            {
                life.Visible = true;
            }

            thePlayer.Position = GameEnvironment.Screen.ToVector2() / 2;

            waveText.Text = "";
        }

        public void ResetPowerUps()
        {
            vodkaSpeedMod = 1;
            enemyBulletSpeedMod = 1;
            thePlayer.moveSpeed = new Vector2(5, 4);
            enemySpeedMod = 1;
        }

        public void AddPowerUps()
        {
            powerUpSpawn = random.Next(1, 6);

            switch (powerUpSpawn)
            {
                case 1:
                    thePowerUps.Add(new SpeedPowerUp(placeNumber));
                    break;

                case 2:
                    thePowerUps.Add(new BulletSpeedPowerUp(placeNumber));
                    break;

                case 3:
                    thePowerUps.Add(new EnemyBulletSpeedDown(placeNumber));
                    break;

                case 4:
                    thePowerUps.Add(new EnemySpeedDown(placeNumber));
                    break;

                case 5:
                    if (thePlayer.health != 3)
                    {
                        thePowerUps.Add(new HealthPowerUp(placeNumber));
                    }
                    else
                    {
                        powerUpSpawn = random.Next(1, 5);
                        switch (powerUpSpawn)
                        {
                            case 1:
                                thePowerUps.Add(new SpeedPowerUp(placeNumber));
                                break;

                            case 2:
                                thePowerUps.Add(new BulletSpeedPowerUp(placeNumber));
                                break;

                            case 3:
                                thePowerUps.Add(new EnemyBulletSpeedDown(placeNumber));
                                break;

                            case 4:
                                thePowerUps.Add(new EnemySpeedDown(placeNumber));
                                break;
                        }
                        break;
                    }
                    break;
            }
            placeNumber++;
            if (placeNumber > 3)
            {
                placeNumber = 1;
            }
        }

        public void HandleWaves()
        {
            switch (waveCount)
            {
                case 1:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 7);
                        
                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 2:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 3:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 4:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 5:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 6:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;
                        }

                        enemySpawned++;
                    }               
                    break;

                case 2:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 2:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 3:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 5:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 6:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 7:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 8:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 9:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;
                        }

                        enemySpawned++;
                    }
                    break;

                case 3:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 2:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 3:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 5:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 6:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 8:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 9:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;
                        }

                        enemySpawned++;
                    }
                    break;

                case 4:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 2:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 3:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 5:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 6:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 8:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 9:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;
                        }

                        enemySpawned++;
                    }
                    break;

                case 5:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 2:                                
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 3:                                
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 5:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 6:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 8:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 9:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;
                        }

                        enemySpawned++;
                    }
                    break;

                case 6:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 2:

                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 3:

                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 5:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 6:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 8:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 9:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;
                        }

                        enemySpawned++;
                    }
                    break;

                case 7:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 2:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 3:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 5:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 6:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                break;

                            case 8:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                break;

                            case 9:
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                break;
                        }
                        enemySpawned++;
                    }
                    break;

                case 8:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 10);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 2:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 3:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 5:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 6:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 8:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 9:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;
                        }
                        enemySpawned++;
                    }
                    break;

                case 9:
                    if (enemySpawned < enemyAmount)
                    {
                        enemyType = random.Next(1, 11);

                        switch (enemyType)
                        {
                            case 1:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 2:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 3:
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 4:
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 5:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 6:
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 7:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new EuEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 8:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new MuricaEnemy(thePlayer, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 9:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new ChinaStar(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;

                            case 10:
                                theEnemies.Add(new SombreroEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                theEnemies.Add(new TankEnemy(thePlayer, theProjectiles, enemyBulletSpeedMod, enemySpeedMod));
                                enemySlain--;
                                break;
                        }
                        enemySpawned++;
                    }
                    break;

                case FINALWAVE:

                    enemyAmount = 1;
                    if (enemySpawned < enemyAmount)
                    {
                        theEnemies.Add(finalBoss);
                        enemySpawned++;
                    }                        

                    break;
            }
        }
    }
}