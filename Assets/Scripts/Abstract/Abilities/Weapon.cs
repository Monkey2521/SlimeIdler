using UnityEngine;

public abstract class Weapon : AbilityContainer, IUpdatable
{
    [Header("Sounds settings")]
    [SerializeField] protected SoundList _sounds;

    [Header("Ability settings")]
    [SerializeField] protected TargetDetector _targetDetector;

    protected float _attackIntervalTimer;
    protected bool _isReady;

    public override void Initialize()
    {
        base.Initialize();

        _isReady = true;

        _targetDetector.Initialize((Stats as WeaponAbilityStats).AttackRange);
    }

    public void Cleanup()
    {
        _targetDetector.Cleanup(true);
    }

    public virtual void Attack()
    {
        if (_isDebug) Debug.Log(name + " attacks");
    }

    public virtual void OnUpdate()
    {
        if (_isReady) return;

        _attackIntervalTimer -= Time.deltaTime;

        if (_attackIntervalTimer <= 0f)
        {
            _isReady = true;
        } 
    }
}
