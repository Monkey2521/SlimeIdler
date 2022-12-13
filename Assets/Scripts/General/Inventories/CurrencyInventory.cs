using UnityEngine;

[System.Serializable]
public class CurrencyInventory : Inventory
{
    [SerializeField] protected CurrencyData _currencyData;

    [SerializeField] protected Currency _baseValue;

    protected int _total;

    public CurrencyData CurrencyData => _currencyData;
    public int Total => _total;

    public CurrencyInventory(CurrencyData currencyData)
    {
        _currencyData = currencyData;
        _total = 0;
    }

    public virtual void Initialize()
    {
        _total = 0;

    }

    public virtual void Add(Currency currency)
    {
        if (currency.CurrencyData.Equals(_currencyData))
        {
            _total += currency.CurrencyValue;
        }
        else return;
    }

    public virtual bool Spend(Currency currency)
    {
        if (IsEnough(currency))
        {
            _total -= currency.CurrencyValue;

            return true;
        }
        else return false;
    }

    public bool IsEnough(Currency currency)
    {
        return currency.CurrencyValue <= _total && currency.CurrencyData.Equals(_currencyData);
    }

    public void GetUpgrade(Upgrade upgrade)
    {
        if (upgrade.Upgrades != null && upgrade.Upgrades.Count > 0)
        {
            foreach (UpgradeData data in upgrade.Upgrades)
            {
                if (data.UpgradingMarkers.IsStrike(_currencyData.Marker))
                {
                    _total = (int)((_total + data.UpgradeValue) * data.UpgradeMultiplier);
                }
            }
        }
    }
   
    #region Serialization
    public override SerializableData SaveData()
    {
        CurrencyInventoryData data = new CurrencyInventoryData();

        data.total = _total;

        return data;
    }

    public override void LoadData(SerializableData data)
    {
        if (data == null)
        {
            _total = _baseValue.CurrencyValue;

            return;
        }

        _total = (data as CurrencyInventoryData).total;
    }

    public override void ResetData()
    {
        _total = 0;
    }

    [System.Serializable]
    protected class CurrencyInventoryData : SerializableData
    {
        public int total;
    }
    #endregion
}
