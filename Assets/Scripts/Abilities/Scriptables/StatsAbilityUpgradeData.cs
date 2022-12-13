using UnityEngine;

[CreateAssetMenu(menuName = "SlimeIdler/Abilities/Stats ability upgrade data", fileName = "New stats ability upgrade data")]
public sealed class StatsAbilityUpgradeData : AbilityUpgradeData
{
    [Tooltip("This upgrade will be repeat until ability reaches its max level")]
    [SerializeField] private Upgrade _repeatingUpgrade;

    public Upgrade RepeatingUpgrade => _repeatingUpgrade;
}