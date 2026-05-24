namespace miniRPG.GameEngine.Components;

public class EnemyComponent
{
    public float CurrentHealth;
    public float MaxHealth;
    public float Damage;
    public float PlayerDamage;
    public bool IsInAttackRange;
    public bool IsPlayerDetected;
    public int ExperienceReward;

    public float AttackRange;
    public float DetectRange;
    public float ChaseSpeed;
    public float StopDistance;

    public float HealthBarHeight;
    public float AttackCooldown;
    public float TimeSinceLastAttack;
}