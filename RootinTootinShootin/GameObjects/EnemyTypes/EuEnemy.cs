namespace RootinTootinShootin
{
    class EuEnemy : Enemy
    {
        public EuEnemy(GameObject target, float speedMod) : base("EnemyImages/EU", target, 225, speedMod)
        {
            scoreValue = 10;
        }
    }
}