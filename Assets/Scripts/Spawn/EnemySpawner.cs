using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawner : Spawner, IGameStartHandler, IGameOverHandler, IUpdatable, IFixedUpdatable
{
    [Header("Spawner settings")]
    [SerializeField] protected Enemy _defaultEnemyPrefab;
    [SerializeField] protected float _spawnDeltaPosition;
    [SerializeField] protected SpawnerStats _stats;

    [SerializeField] protected int _timeForUpgrade;
    [SerializeField] protected Upgrade _repeatingUpgrade;
    [SerializeField] protected Player _player;

    protected MonoPool<Enemy> _pool;
    
    protected bool _onSpawn;
    protected float _spawnTimer;
    protected float _upgradeTimer;

    protected bool _spawning;
    protected float _spawnIntervalTimer;
    protected int _spawnCount;

    protected int _upgradeCount;

    #region unity
    protected void OnEnable()
    {
        LoadData();
        EventBus.Subscribe(this);
    }
    
    protected void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    protected void Update()
    {
        OnUpdate();
    }

    protected void FixedUpdate()
    {
        OnFixedUpdate();
    }
    #endregion

    public virtual void OnUpdate() 
    {
        if (_onSpawn)
        {
            if (_spawning)
            {
                if (_spawnIntervalTimer <= 0f)
                {
                    Spawn(GetSpawnPosition());
                }
                else
                {
                    _spawnIntervalTimer -= Time.deltaTime;
                }
            }
            else
            {
                _spawnTimer -= Time.deltaTime;

                if (_spawnTimer <= 0)
                {
                    _spawning = true;
                    _spawnIntervalTimer = 0;
                    _spawnCount = 0;
                }
            }

            _upgradeTimer -= Time.deltaTime;

            if (_upgradeTimer <= 0)
            {
                GetUpgrade();
                _upgradeTimer = _timeForUpgrade;
            }

            for (int i = 0; i < _pool.PulledObjects.Count; i++)
            {
                _pool.PulledObjects[i]?.OnUpdate();
            }
        }
    }

    public virtual void OnFixedUpdate() 
    {
        for (int i = 0; i < _pool.PulledObjects.Count; i++)
        {
            _pool.PulledObjects[i]?.OnFixedUpdate();
            if (_pool.PulledObjects[i] != null)
                _pool.PulledObjects[i].Move(GetMoveDirection(_player.transform.position, _pool.PulledObjects[i].transform.position));
        }
    }

    protected override void Spawn(Vector3 position)
    {
        Enemy enemy = _pool.Pull();
        enemy.Initialize(_player, _pool);

        enemy.transform.position = position;

        _spawnIntervalTimer = _stats.EnemySpawnInterval.Value;
        _spawnCount++;

        if (_spawnCount >= (int)_stats.SpawnNumber.Value)
        {
            _spawning = false;
            _spawnTimer = _stats.SpawnInterval.Value;
            _spawnCount = 0;
        }
    }

    protected virtual Vector3 GetMoveDirection(Vector3 playerPos, Vector3 enemyPos)
    {
        return new Vector3
                        (
                            playerPos.x - enemyPos.x,
                            0f,
                            0f
                        );
    }
    
    protected virtual Vector3 GetSpawnPosition()
    {
        return transform.position + new Vector3
                                        (
                                            0f,
                                            0f,
                                            Random.Range(-_spawnDeltaPosition, _spawnDeltaPosition)
                                        );
    }

    protected virtual void GetUpgrade()
    {
        _upgradeCount++;

        for (int i = 0; i < _pool.PulledObjects.Count; i++)
        {
            _pool.PulledObjects[i]?.GetUpgrade(_repeatingUpgrade);
        }
        for (int i = 0; i < _pool.Objects.Count; i++)
        {
            _pool.Objects[i]?.GetUpgrade(_repeatingUpgrade);
        }

        _stats.GetUpgrade(_repeatingUpgrade);

        SaveData();
    }

    public void OnGameStart()
    {      
        _stats.Initialize();

        _pool = new MonoPool<Enemy>(_defaultEnemyPrefab, 50, transform);

        int upgrades = _upgradeCount;
        _upgradeCount = 0;

        for (int i = 0; i < upgrades; i++)
        {
            GetUpgrade();
        }

        _onSpawn = true;
        _spawning = false;
        _spawnTimer = 0f;
        _spawnCount = 0;
        _upgradeTimer = _timeForUpgrade;
    }

    public void OnGameOver()
    {
        _onSpawn = false;
        _spawning = false;
        _spawnTimer = 0f;
        _spawnCount = 0;
        _upgradeTimer = _timeForUpgrade;

        _upgradeCount -= 3;

        if (_upgradeCount < 0)
        {
            _upgradeCount = 0;
        }

        SaveData();

        _pool.ClearPool();
    }

    protected void LoadData()
    {
        if (File.Exists(DataPath.DefaultPath + name + ".dat"))
        {
            if (DataPath.Load(DataPath.DefaultPath + name + ".dat") is EnemySpawnerData data)
            {
                _upgradeCount = data.upgradeCount;
            }
        }
    }

    protected void SaveData()
    {
        EnemySpawnerData data = new EnemySpawnerData();

        data.upgradeCount = _upgradeCount;

        DataPath.Save(DataPath.DefaultPath + name + ".dat", data);
    }

    [ContextMenu("Reset data")]
    protected void ResetData()
    {
        if (File.Exists(DataPath.DefaultPath + name + ".dat"))
        {
            File.Delete(DataPath.DefaultPath + name + ".dat");
        }
    }

    [System.Serializable]
    private class EnemySpawnerData : SerializableData
    {
        public int upgradeCount;
    }
}
