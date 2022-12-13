using UnityEngine;

[System.Serializable]
public abstract class Stat : IStat, IUpgradeable
{
    [Header("Debug settings")]
    [SerializeField] protected bool _isDebug;

    [Header("Stat settings")]
    [SerializeField] protected StatData _statData;

    protected float _value;
    protected float _minValue;
    protected float _maxValue;
    protected UpgradeList _upgrades;

    public float BaseValue => _statData.BaseValue;
    public virtual float Value => _value;
    public float MinValue => _minValue;
    public float MaxValue => _maxValue;
    public bool MaxValueIsInfinite => _statData.MaxValueIsInfinite;

    public UpgradeList Upgrades => _upgrades;


    public virtual void Initialize()
    {
        _upgrades = new UpgradeList();

        _value = _statData.BaseValue;
        _minValue = _statData.MinValue;
        _maxValue = _statData.MaxValue;

        if (_isDebug) Debug.Log(_statData.name + " initialized");
    }

    public Stat (StatData statData, UpgradeList upgradeList = null, bool isDebug = false)
    {
        _statData = statData;

        Initialize();

        if (upgradeList != null)
        {
            foreach (UpgradeData data in upgradeList.Upgrades)
            {
                if (data == null) continue;

                if (data.UpgradingMarkers.IsStrike(_statData.Markers))
                {
                    _upgrades.Add(data);
                }
            }

            if (_statData.MaxValueIsInfinite)
            {
                _maxValue = (_statData.MaxValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;
            }
        }

        _isDebug = isDebug;

        if (_isDebug) Debug.Log(_statData.name + " initialized");
    }

    public virtual bool Upgrade(Upgrade upgrade)
    {
        if (upgrade == null) return false;

        if (_isDebug) Debug.Log(_statData.name + " try get upgrade: " + upgrade.name);

        int upgrades = 0;

        if (_upgrades == null) _upgrades = new UpgradeList();

        foreach(UpgradeData data in upgrade.Upgrades)
        {
            if (data == null) continue;

            if (data.UpgradingMarkers.IsStrike(_statData.Markers))
            {
                if (_isDebug) Debug.Log(_statData.name + " getted upgrade: " + upgrade.name);

                upgrades++;

                _upgrades.Add(data);
            }
        }

        if (_statData.MaxValueIsInfinite)
        {
            _maxValue = (_statData.MaxValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;
        }

        return upgrades > 0;
    }

    public virtual bool DispelUpgrade(Upgrade upgrade)
    {
        if (upgrade == null) return false;

        if (_isDebug) Debug.Log(_statData.name + " try dispel upgrade: " + upgrade.name);

        int revealedUpgrades = 0;
        
        if (_upgrades == null) _upgrades = new UpgradeList();
        
        foreach (UpgradeData data in upgrade.Upgrades)
        {
            if (data == null) continue;

            if (_upgrades.Dispel(data))
            {
                if (_isDebug) Debug.Log(_statData.name + " dispelled upgrade: " + upgrade.name);

                revealedUpgrades++;
            }
        }

        if (_statData.MaxValueIsInfinite)
        {
            _maxValue = (_statData.MaxValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;
        }

        return revealedUpgrades > 0;
    }

    public void SetValue(float value)
    {
        _value = value;

        if (_isDebug) Debug.Log(_statData.name + " set value: " + _value);

        if (!_statData.MaxValueIsInfinite && _value > _statData.MaxValue)
        {
            if (_isDebug) Debug.Log(_statData.name + " set max value: " + _value);

            _value = _statData.MaxValue;
        }

        if (_value < _statData.MinValue)
        {
            if (_isDebug) Debug.Log(_statData.name + " set min value: " + _value);

            _value = _statData.MinValue;
        }
    }
}
