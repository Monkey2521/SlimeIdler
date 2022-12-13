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

    public abstract void Move(Vector3 direction);

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
