using UnityEngine;

[System.Serializable]
public class ProjectileAbilityStats : WeaponAbilityStats
{
    [Header("Projectiles settings")]
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected Radius _projectileSize;

    [Space(5)]
    [SerializeField] protected ProjectileSpeed _projectileSpeed;
    [SerializeField] protected ProjectileNumber _projectileNumber;
    [SerializeField] protected Cooldown _projectilesSpawnInterval;

    public Projectile Projectile => _projectilePrefab;
    public Radius ProjectileSize => _projectileSize;
    public ProjectileNumber ProjectileNumber => _projectileNumber;
    public Cooldown ProjectilesSpawnInterval => _projectilesSpawnInterval;
    public ProjectileSpeed ProjectileSpeed => _projectileSpeed;

    public override void Initialize()
    {
        base.Initialize();

        _projectileSize.Initialize();
        _projectileNumber.Initialize();
        _projectilesSpawnInterval.Initialize();
        _projectileSpeed.Initialize();
    }

    public override void GetUpgrade(Upgrade upgrade)
    {
        base.GetUpgrade(upgrade);

        _projectileSize.Upgrade(upgrade);
        _projectileNumber.Upgrade(upgrade);
        _projectilesSpawnInterval.Upgrade(upgrade);
        _projectileSpeed.Upgrade(upgrade);
    }

    public override void DispelUpgrade(Upgrade upgrade)
    {
        base.DispelUpgrade(upgrade);

        _projectileSize.DispelUpgrade(upgrade);
        _projectileNumber.DispelUpgrade(upgrade);
        _projectilesSpawnInterval.DispelUpgrade(upgrade);
        _projectileSpeed.DispelUpgrade(upgrade);
    }
}
