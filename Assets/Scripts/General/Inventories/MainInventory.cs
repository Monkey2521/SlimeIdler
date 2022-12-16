using System.IO;
using UnityEngine;

public class MainInventory : MonoBehaviour, IEnemyKilledHandler
{
    [Header("Debug settings")]
    [SerializeField] private bool _isDebug;

    [Header("Inventories settings")]
    [SerializeField] private CurrencyInventory _coinInventory;
    [SerializeField] private CurrencyInventory _gemsInventory;
    //[SerializeField] private EquipmentInventory _equipmentInventory;

    public CurrencyInventory CoinInventory => _coinInventory;
    public CurrencyInventory GemsInventory => _gemsInventory;
    //public EquipmentInventory EquipmentInventory => _equipmentInventory;

    private void OnEnable()
    {
        EventBus.Subscribe(this);

        _coinInventory.Initialize();
        _gemsInventory.Initialize();

        //_equipmentInventory.Initialize();

        LoadData();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);

        SaveData();
    }

    public void OnEnemyKilled(Enemy enemy)
    {
        Add(enemy.Reward);
    }

    #region Serialization
    [ContextMenu("Load data")]
    private void LoadData()
    {
        _coinInventory.LoadData(DataPath.Load(DataPath.CoinsInventory));
        _gemsInventory.LoadData(DataPath.Load(DataPath.GemsInvneotry));
        //_equipmentInventory.LoadData(DataPath.Load(DataPath.EquipmentInventory));
    }

    [ContextMenu("Save data")]
    public void SaveData()
    {
        DataPath.Save(DataPath.CoinsInventory, _coinInventory.SaveData());
        DataPath.Save(DataPath.GemsInvneotry, _gemsInventory.SaveData());
        //DataPath.Save(DataPath.EquipmentInventory, _equipmentInventory.SaveData());
    }

    [ContextMenu("Reset data")]
    private void ResetData()
    {
        if (File.Exists(DataPath.CoinsInventory))
        {
            File.Delete(DataPath.CoinsInventory);
            _coinInventory.ResetData();

            if (_isDebug) Debug.Log("Reset CoinsInventory");
        }
        
        if (File.Exists(DataPath.GemsInvneotry))
        {
            File.Delete(DataPath.GemsInvneotry);
            _gemsInventory.ResetData();

            if (_isDebug) Debug.Log("Reset GemsInvneotry");
        }
        
        /*if (File.Exists(DataPath.EquipmentInventory))
        {
            File.Delete(DataPath.EquipmentInventory);
            _equipmentInventory.ResetData();

            if (_isDebug) Debug.Log("Reset EquipmentInventory");
        }*/
    }
    #endregion

    #region Currencies
    public bool EnoughResources(Currency currency)
    {
        CurrencyInventory inventory = FindInventory(currency.CurrencyData);

        if (inventory == null) return false;

        return inventory.IsEnough(currency);
    }

    private bool EnoughResources(Currency currency, out CurrencyInventory inventory)
    {
        inventory = FindInventory(currency.CurrencyData);

        if (inventory == null) return false;

        return inventory.IsEnough(currency);
    }

    public void Add(Currency currency)
    {
        CurrencyInventory inventory = FindInventory(currency.CurrencyData);

        inventory?.Add(currency);

        SaveData();
    }

    public bool Spend(Currency currency)
    {
        if (EnoughResources(currency, out CurrencyInventory inventory))
        {
            inventory.Spend(currency);

            SaveData();

            return true;
        }
        else return false;
    }

    public CurrencyInventory FindInventory(CurrencyData data)
    {
        CurrencyInventory inventory;

        if (data.Equals(_coinInventory.CurrencyData))
        {
            inventory = _coinInventory;
        }
        else if (data.Equals(_gemsInventory.CurrencyData))
        {
            inventory = _gemsInventory;
        }
        else
        {
            Debug.Log("Missing data!");

            return null;
        }

        return inventory;
    }
    #endregion

    #region test
    [ContextMenu("Add coins")]
    private void AddCoins()
    {
        _coinInventory.Add(new Currency(_coinInventory.CurrencyData, 5000));
    }
    
    [ContextMenu("Spend coins")]
    private void SpendCoins()
    {
        _coinInventory.Spend(new Currency(_coinInventory.CurrencyData, 312));
    }


    [ContextMenu("Add gems")]
    private void AddGems()
    {
        _gemsInventory.Add(new Currency(_gemsInventory.CurrencyData, 50));
    }

    [ContextMenu("Spend gems")]
    private void SpendGems()
    {
        _gemsInventory.Spend(new Currency(_gemsInventory.CurrencyData, 8));
    }
    #endregion

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
