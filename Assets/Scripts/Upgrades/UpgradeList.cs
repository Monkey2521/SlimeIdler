using System.Collections.Generic;
using UnityEngine;

public class UpgradeList
{
    private List<UpgradeData> _upgrades;

    private float _upgradesValue;
    private float _upgradesMultiplier;

    public List<UpgradeData> Upgrades => _upgrades;
    public float UpgradesValue => _upgradesValue;
    public float UpgradesMultiplier => _upgradesMultiplier;

    public UpgradeList()
    {
        _upgrades = new List<UpgradeData>();

        _upgradesValue = 0;
        _upgradesMultiplier = 1;
    }

    public void Add(UpgradeData upgrade)
    {
        _upgrades.Add(upgrade);

        CalculateUpgrades();
    }

    public bool Dispel(UpgradeData upgrade)
    {
        if (_upgrades.Remove(upgrade))
        {
            CalculateUpgrades();

            return true;
        }
        else return false;
    }

    public void DispelAll()
    {
        _upgrades.Clear();

        CalculateUpgrades();
    }

    private void CalculateUpgrades()
    {
        float value = 0, multiplier = 1;

        foreach(UpgradeData upgrade in _upgrades)
        {
            value += upgrade.UpgradeValue;
            multiplier += (upgrade.UpgradeMultiplier - 1);
        }

        _upgradesValue = value;
        _upgradesMultiplier = multiplier;
    }
}
