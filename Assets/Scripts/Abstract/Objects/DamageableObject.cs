using UnityEngine;

public abstract class DamageableObject : MonoBehaviour, IDamageable
{
    [Header("Debug settings")]
    [SerializeField] protected bool _isDebug;

    [Header("Sounds settings")]
    [SerializeField] protected SoundList _sounds;

    [Header("Health settings")]
    [SerializeField] protected bool _isImmortal;
    [SerializeField] protected HPBar _healthBar;

    public bool IsImmortal => _isImmortal;
    public abstract int HP { get; }
    public abstract int MaxHP { get; }

    public virtual void TakeDamage(int damage)
    {
        if (_isDebug) Debug.Log(name + " take " + damage + " damage");

        _healthBar?.UpdateHealth();

        if (HP <= 0 && !_isImmortal)
        {
            Die();
        }
        else
        {
            _sounds.PlaySound(SoundTypes.TakeDamage);
        }
    }

    public virtual void Die()
    {
        _sounds.PlaySound(SoundTypes.Die);

        if (_isDebug) Debug.Log(name + " dies");
    }
}
