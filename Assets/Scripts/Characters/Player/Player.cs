using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : CharacterBase
{
    [Header("Stats settings")]
    [SerializeField] protected CharacterStats _stats;
    [SerializeField] protected Text _damageText;

    [Header("Inventory settings")]
    [SerializeField] protected AbilityInventory _abilityInventory;

    protected List<Upgrade> _upgrades;

    public override CharacterStats Stats => _stats;
    public AbilityInventory AbilityInventory => _abilityInventory;

    public void Initialize()
    {
        _stats.Initialize();

        _healthBar?.Initialize(_stats.Health);

        if (_abilityInventory.Abilities != null)
        {
            while (_abilityInventory.Abilities.Count > 0)
            {
                _abilityInventory.Remove(_abilityInventory.Abilities[0]);
            }
        }

        _abilityInventory.Initialize();

        _upgrades = new List<Upgrade>();

        GetAbility(_stats.BaseWeapon);

        _damageText.text = (_abilityInventory.Weapons[0].Stats as WeaponAbilityStats).Damage.Value.ToString();
    }

    private void Update()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();

        foreach (ProjectileWeapon weapon in _abilityInventory.ProjectileWeapons)
        {
            weapon.OnFixedUpdate();
        }
    }

    protected override void Attack()
    {
        foreach(Weapon weapon in _abilityInventory.Weapons)
        {
            weapon.OnUpdate();
            weapon.Attack();
        }
    }

    public override void GetUpgrade(Upgrade upgrade)
    {
        base.GetUpgrade(upgrade);

        for (int index = 0; index < _abilityInventory.Abilities.Count; index++)
        {
            _abilityInventory.Abilities[index].Upgrade(upgrade);
        }

        _upgrades.Add(upgrade);

        _damageText.text = (_abilityInventory.Weapons[0].Stats as WeaponAbilityStats).Damage.Value.ToString();
    }

    public AbilityContainer GetAbility(AbilityContainer ability)
    {
        AbilityContainer abilityContainer = _abilityInventory.Find(ability);

        if (abilityContainer != null)
        {
            if (abilityContainer.IsMaxLevel)
            {
                if (_isDebug) Debug.Log("This ability is max level!");

                return abilityContainer;
            }

            if (_isDebug) Debug.Log("Ability already in inventory. Upgrade it");

            GetUpgrade(abilityContainer.CurrentUpgrade);

            return abilityContainer;
        }
        else
        {
            if (_isDebug) Debug.Log("Add new ability");

            AbilityContainer newAbility = _abilityInventory.Add(ability);

            if (newAbility != null)
            {
                foreach (Upgrade upgrade in _upgrades)
                {
                    newAbility.Upgrade(upgrade);
                }
                
                GetUpgrade(newAbility.CurrentUpgrade);
            }
            else if (_isDebug) Debug.Log("Adding ability error!");

            return newAbility;
        }
    }

    public override void DispelUpgrade(Upgrade upgrade)
    {
        base.DispelUpgrade(upgrade);

        foreach(AbilityContainer ability in _abilityInventory.Abilities)
        {
            ability.DispelUpgrade(upgrade);
        }
    }

    [ContextMenu("Die")]
    public override void Die()
    {
        base.Die();

        EventBus.Publish<IGameOverHandler>(handler => handler.OnGameOver());
    }
}
