using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : Spawner, IUpdatable, IFixedUpdatable
{
    [Tooltip("Means distance between player and spawned objects")]
    [SerializeField] protected float _maxDistanceForRespawn;

    protected int _maxUnitsOnScene;
    protected int _totalSpawned;

    protected Upgrade _currentUpgrade;
    protected List<Upgrade> _levelUpgrades;

    protected MonoPool<Enemy> _pool;

    protected Player _player;

    public virtual void OnUpdate() 
    {
       
    }

    public virtual void OnFixedUpdate() 
    {
        
    }

    protected virtual Vector3 GetMoveDirection(Vector3 playerPos, Vector3 enemyPos)
    {
        return new Vector3
                        (
                            playerPos.x - enemyPos.x,
                            0f
                        );
    }

    protected virtual void ClearPools()
    {

    }

    protected virtual void GetUpgrade()
    {

    }

    protected virtual void DispelUpgrades()
    {

    }
}
