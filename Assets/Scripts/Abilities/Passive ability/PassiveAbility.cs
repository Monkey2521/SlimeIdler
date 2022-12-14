using UnityEngine;
using UnityEngine.UI;

public class PassiveAbility : AbilityContainer
{
    [SerializeField] protected AbilityStats _stats;

    [Space(5)]
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _valueText;
    [SerializeField] private Text _costText;

    public override AbilityStats Stats => _stats;

    [SerializeField] private Player _player;

    protected virtual void OnEnable()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("here");

        _nameText.text = _name;
        UpdateSlot();
    }

    private void UpdateSlot()
    {
        _abilityIcon.sprite = _icon;
        _levelText.text = "LVL " + ((int)_stats.Level.Value).ToString();

        float value = 0;

        foreach(UpgradeData data in _upgradeData.RepeatingUpgrade.Upgrades)
        {
            value += data.UpgradeValue;
        }

        _valueText.text = (value * (int)_stats.Level.Value).ToString();

        _costText.text = "100";
    }

    public void OnUpgradeClick()
    {
        Upgrade(CurrentUpgrade);

        _player.GetUpgrade(CurrentUpgrade);

        UpdateSlot();
    }
}
