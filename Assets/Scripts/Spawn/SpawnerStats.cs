using UnityEngine;

[System.Serializable]
public class SpawnerStats : IObjectStats
{
    [SerializeField] protected Cooldown _spawnInterval;
    [SerializeField] protected Cooldown _enemySpawnInterval;
    [SerializeField] protected SpawnNumber _spawnNumber;

    public Cooldown SpawnInterval => _spawnInterval;
    public Cooldown EnemySpawnInterval => _enemySpawnInterval;
    public SpawnNumber SpawnNumber => _spawnNumber;

    public void Initialize()
    {
        _spawnInterval.Initialize();
        _enemySpawnInterval.Initialize();
        _spawnNumber.Initialize();
    }

    public void GetUpgrade(Upgrade upgrade)
    {
        _spawnInterval.Upgrade(upgrade);
        _enemySpawnInterval.Upgrade(upgrade);
        _spawnNumber.Upgrade(upgrade);
    }

    public void DispelUpgrade(Upgrade upgrade)
    {
        _spawnInterval.DispelUpgrade(upgrade);
        _enemySpawnInterval.DispelUpgrade(upgrade);
        _spawnNumber.DispelUpgrade(upgrade);
    }
}
