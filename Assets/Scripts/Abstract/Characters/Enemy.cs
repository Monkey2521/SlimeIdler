using UnityEngine;

public abstract class Enemy : CharacterBase, IPoolable, IUpdatable
{
    [Header("Enemy settings")]
    [SerializeField] protected CapsuleCollider _selfCollider;
    [SerializeField] protected CharacterStats _stats;

    protected Player _player = null; 
    protected MonoPool<Enemy> _pool = null;

    public CapsuleCollider Collider => _selfCollider;
    public override CharacterStats Stats => _stats;

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

        EventBus.Publish<IEnemyKilledHandler>(handler => handler.OnEnemyKilled(this));

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
