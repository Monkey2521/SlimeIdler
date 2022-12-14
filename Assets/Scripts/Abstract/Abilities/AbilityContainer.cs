using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityContainer : MonoBehaviour, IUpgradeable
{
    [Header("Debug settings")]
    [SerializeField] protected bool _isDebug;

    [Header("Settings")]
    [SerializeField] protected string _name;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected StatsAbilityUpgradeData _upgradeData;

    protected UpgradeList _upgrades;

    public string Name => _name;
    public UpgradeList Upgrades => _upgrades;
    public bool IsMaxLevel => !Stats.Level.MaxValueIsInfinite && Stats.Level.Value == Stats.Level.MaxValue;
    public Sprite Icon => _icon;

    public abstract AbilityStats Stats { get; }
    public AbilityUpgradeData UpgradeData => _upgradeData;
    public Upgrade CurrentUpgrade => _upgradeData.RepeatingUpgrade;

    public virtual void Initialize()
    {
        Stats.Initialize();

        _upgrades = new UpgradeList();
    }

    public virtual bool Upgrade(Upgrade upgrade)
    {
        if (upgrade == null) return false;

        if (upgrade.IsAbilityUpgrade && Stats.AbilityMarker.Equals(upgrade.AbilityMarker))
        {
            Stats.Level.LevelUp();

            return true;
        }

        return false;
    }

    public virtual void DispelUpgrade(Upgrade upgrade)
    {
        Stats.DispelUpgrade(upgrade);
    }

    public virtual void DispelUgrades(List<Upgrade> upgrades)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            DispelUpgrade(upgrade);
        }
    }

    public virtual void DestroyAbility()
    {
        Destroy(gameObject);
    }
}
