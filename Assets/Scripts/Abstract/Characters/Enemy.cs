using UnityEngine;

public abstract class Enemy : CharacterBase, IPoolable, IUpdatable
{
    [Header("Enemy settings")]
    [SerializeField] private Currency _reward;
    [SerializeField] private float _currencyRewardMultiplier = 3f;
    [SerializeField] protected CharacterStats _stats;

    protected Player _player = null; 
    protected MonoPool<Enemy> _pool = null;

    public override CharacterStats Stats => _stats;
    public Currency Reward => _reward;

    protected void Awake()
    {
        _stats.Initialize();
        _healthBar.Initialize(_stats.Health);

        _stats.BaseWeapon.Initialize();
    }

    public virtual void Initialize(Player player, MonoPool<Enemy> pool = null)
    {
        _pool = pool;
        _player = player;

        _stats.Health.SetValue(_stats.Health.MaxHP);
        _healthBar.Initialize(_stats.Health);
    }

    public virtual void ResetObject()
    {
        _pool = null;
    }

    public virtual void OnUpdate()
    {
        Attack();
    }

    protected override void Attack()
    {
        _stats.BaseWeapon.OnUpdate();
        _stats.BaseWeapon.Attack();
    }

    [ContextMenu("Die")]
    public override void Die()
    {
        base.Die();
        _stats.BaseWeapon.Cleanup();

        EventBus.Publish<IEnemyKilledHandler>(handler => handler.OnEnemyKilled(this));

        _sounds.PlaySound(SoundTypes.Die);

        if (_pool != null)
        {
            if (_isDebug) Debug.Log(name + " returns to pool");
            
            _pool.Release(this);
        }
        else
        {
            if (_isDebug) Debug.Log("Destroy" + name);

            Destroy(gameObject);
        }
    }

    public override void GetUpgrade(Upgrade upgrade)
    {
        base.GetUpgrade(upgrade);
        _stats.BaseWeapon.Upgrade(upgrade);

        _reward = new Currency(_reward.CurrencyData, (int)(_reward.CurrencyValue * _currencyRewardMultiplier));
    }

    public override void DispelUpgrade(Upgrade upgrade)
    {
        base.DispelUpgrade(upgrade);

        _stats.BaseWeapon.DispelUpgrade(upgrade);
    }

    protected virtual void OnDisable()
    {
        EventBus.Publish<IObjectDisableHandler>(handler => handler?.OnObjectDisable(gameObject));
        if (_isDebug) Debug.Log(name + " disabled");
    }
}
