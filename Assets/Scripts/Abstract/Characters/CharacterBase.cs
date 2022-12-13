using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : DamageableObject, IFixedUpdatable
{
    public abstract CharacterStats Stats { get; }

    public override int HP => (int)Stats.Health.Value;
    public override int MaxHP => Stats.Health.MaxHP;

    public virtual void OnFixedUpdate()
    {
        Stats.Health.Heal(Stats.Regeneration.Value * Time.fixedDeltaTime);

        if (_healthBar != null) _healthBar.UpdateHealth();
    }

    public virtual void Move(Vector3 direction)
    {
        Vector3 pos = transform.position;

        transform.position = Vector3.MoveTowards(pos, pos + direction * Stats.Velocity.Value, Stats.Velocity.Value * Time.fixedDeltaTime);
    }

    protected abstract void Attack();

    public override void TakeDamage(int damage)
    {
        Stats.Health.TakeDamage(damage);

        base.TakeDamage(damage);
    }

    public virtual void GetUpgrade(Upgrade upgrade)
    {
        Stats.GetUpgrade(upgrade);
    }

    public virtual void DispelUpgrade(Upgrade upgrade)
    {
        Stats.DispelUpgrade(upgrade);
    }

    public virtual void DispelUpgrades(List<Upgrade> upgrades)
    {
        foreach(Upgrade upgrade in upgrades)
        {
            DispelUpgrade(upgrade);
        }
    }
}
