namespace RootinTootinShootin
{
    class Ball : Projectile
    {
        public Ball(GameObject shooter, GameObject target, int speed) : base("ProjectileImages/Ball", shooter.Position, target.Position, speed)
        {
            
        }
    }
}