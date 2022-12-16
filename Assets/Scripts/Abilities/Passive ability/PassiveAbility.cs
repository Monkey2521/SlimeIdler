using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAbility : AbilityContainer, IGameStartHandler, IGameOverHandler
{
    [SerializeField] protected Currency _upgradeCurrency;
    [SerializeField] protected float _currencyCostMultiplier = 1.25f;
    [SerializeField] protected AbilityStats _stats;

    [Space(5)]
    [SerializeField] protected Image _abilityIcon;
    [SerializeField] protected Text _levelText;
    [SerializeField] protected Text _nameText;
    [SerializeField] protected Text _valueText;
    [SerializeField] protected Text _costText;
    [SerializeField] protected Button _upgradeButton;

    public override AbilityStats Stats => _stats;
    public Currency UpgradeCurrency => _upgradeCurrency;

    [SerializeField] protected Player _player;
    [SerializeField] protected MainInventory _mainInventory;
    [SerializeField] protected MainMenu _mainMenu;

    protected virtual void OnEnable()
    {
        base.Initialize();

        LoadData();

        _nameText.text = _name;

        UpdateSlot();

        EventBus.Subscribe(this);
    }

    protected void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void OnGameStart()
    {
        Initialize();
    }

    public void OnGameOver()
    {
        _upgradeButton.interactable = false;
    }

    public override void Initialize()
    {
        for (int i = 0; i < (int)_stats.Level.Value; i++)
        {
            _player.GetUpgrade(CurrentUpgrade);
        }

        if (!IsMaxLevel)
            _upgradeButton.interactable = true;
    }

    protected void UpdateSlot()
    {
        _abilityIcon.sprite = _icon;
        _levelText.text = "LVL " + ((int)_stats.Level.Value).ToString() + 
            (_stats.Level.MaxValueIsInfinite ? "" : " (max: " + ((int)_stats.Level.MaxValue).ToString() + ")");

        float value = 0;

        foreach(UpgradeData data in _upgradeData.RepeatingUpgrade.Upgrades)
        {
            value += data.UpgradeValue;
        }

        _valueText.text = (value * (int)_stats.Level.Value).ToString();

        if (IsMaxLevel)
        {
            _upgradeButton.interactable = false;
            _costText.text = "MAX";
        }
        else
        {
            _costText.text = "Upgrade\n" + _upgradeCurrency.CurrencyValue;
        }
    }

    public void OnUpgradeClick()
    {
        if (_mainInventory.Spend(_upgradeCurrency))
        {
            Upgrade(CurrentUpgrade);

            _player.GetUpgrade(CurrentUpgrade);
            _upgradeCurrency = new Currency(_upgradeCurrency.CurrencyData, (int)(_upgradeCurrency.CurrencyValue * _currencyCostMultiplier));

            UpdateSlot();

            SaveData();
        }
        else
        {
            _mainMenu.ShowPopupMessage("Not enough resources!");
        }
    }

    protected void LoadData()
    {
        if (File.Exists(DataPath.DefaultPath + name + ".dat"))
        {
            if (DataPath.Load(DataPath.DefaultPath + name + ".dat") is PassiveSkillData data)
            {
                for (int i = 0; i < data.level; i++)
                {
                    Upgrade(CurrentUpgrade);
                    _upgradeCurrency = new Currency(_upgradeCurrency.CurrencyData, (int)(_upgradeCurrency.CurrencyValue * _currencyCostMultiplier));
                }
            }
        }
    }

    protected void SaveData()
    {
        PassiveSkillData data = new PassiveSkillData();

        data.level = (int)_stats.Level.Value;

        DataPath.Save(DataPath.DefaultPath + name + ".dat", data);
    }

    [ContextMenu("Reset data")]
    protected void ResetData()
    {
        if (File.Exists(DataPath.DefaultPath + name + ".dat"))
        {
            File.Delete(DataPath.DefaultPath + name + ".dat");
        }
    }

    [System.Serializable]
    protected class PassiveSkillData : SerializableData
    {
        public int level;
    }
}
