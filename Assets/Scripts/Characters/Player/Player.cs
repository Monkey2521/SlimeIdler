using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    [Header("Stats settings")]
    [SerializeField] protected List<Weapon> _startWeaponsList;
    [SerializeField] protected CharacterStats _stats;

    [Header("Inventory settings")]
    [SerializeField] protected AbilityInventory _abilityInventory;

    protected List<Upgrade> _upgrades;

    public override CharacterStats Stats => _stats;

    public AbilityInventory AbilityInventory => _abilityInventory;
    
    protected MainInventory _mainInventory;

    public void Initialize()
    {

        List<Upgrade> equipmentUpgrades = new List<Upgrade>();

        _stats.Initialize();

        _healthBar?.Initialize(_stats.Health);
        _abilityInventory.Initialize();

        _upgrades = new List<Upgrade>();

        GetAbility(_stats.BaseWeapon);
        
        foreach (Upgrade upgrade in equipmentUpgrades)
        {
            GetUpgrade(upgrade);
        }
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

    public override void Move(Vector3 direction) { }

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

        EventBus.Publish<IPlayerDieHandler>(handler => handler.OnPlayerDie());
    }
}
