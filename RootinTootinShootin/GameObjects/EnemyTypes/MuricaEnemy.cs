namespace RootinTootinShootin
{
    class MuricaEnemy : Enemy
    {
        public MuricaEnemy(GameObject target, float speedMod) : base("EnemyImages/Murica", target, 125, speedMod)
        {
            scoreValue = 5;
        }
    }
}