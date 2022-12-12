using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : AbilityContainer
{
    [SerializeField] protected StatsAbilityUpgradeData _upgradeData;
    [SerializeField] protected AbilityStats _stats;

    public override AbilityUpgradeData UpgradeData => _upgradeData;
    public override AbilityStats Stats => _stats;
    public override CurrentUpgrade CurrentUpgrade => _upgradeData.RepeatingUpgrade;
}
