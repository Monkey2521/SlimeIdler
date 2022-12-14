using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable, IFixedUpdatable
{
    [Header("Debug settings")]
    [SerializeField] protected bool _isDebug;

    [Header("Throw settings")]
    [SerializeField] protected TagList _targetTags;
    [SerializeField] protected SoundList _sounds;

    protected Vector3 _moveDirection;

    protected bool _onThrow;

    protected ProjectileSpeed _speed;
    protected Damage _damage;
    protected Radius _attackRange;
    protected Duration _lifeDuration;

    protected ProjectileWeapon _weapon;

    protected float _releaseTimer;

    public virtual void ResetObject()
    {
        _moveDirection = Vector3.zero;

        transform.position = _weapon.transform.position;

        _onThrow = false;
        _speed = null;
        _damage = null;
        _attackRange = null;
        _weapon = null;
    }

    public virtual void Initialize(ProjectileAbilityStats stats, ProjectileWeapon weapon)
    {
        _speed = stats.ProjectileSpeed;
        _damage = stats.Damage;
        _attackRange = stats.AttackRange;
        _lifeDuration = stats.ProjectileLifeDuration;

        _weapon = weapon;

        transform.localScale = new Vector3(stats.ProjectileSize.Value, stats.ProjectileSize.Value, stats.ProjectileSize.Value);
    }

    public virtual void Throw(Vector3 direction)
    {
        transform.LookAt(transform.position + direction);

        _moveDirection = direction.normalized;
        _releaseTimer = 0f;
        _onThrow = true;
    }

    public virtual void OnFixedUpdate()
    {
        if (_onThrow)
        {
            Move();

            _releaseTimer += Time.fixedDeltaTime;

            if (_releaseTimer >= _lifeDuration.Value)
            {
                _weapon.OnProjectileRelease(this);
            }
        }
    }

    protected virtual void Move()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, currentPosition + _moveDirection * _speed.Value, _speed.Value * Time.fixedDeltaTime);

        transform.position = newPosition;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        DamageableObject obj = other.GetComponent<DamageableObject>();

        if (obj != null && _targetTags.Contains(obj.tag))
        {
            if (_isDebug) Debug.Log(name + " find target");

            obj.TakeDamage((int)_damage.Value);

            _weapon.OnProjectileRelease(this);
        }
    }
}
