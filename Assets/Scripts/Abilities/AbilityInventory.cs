using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class AbilityInventory
{
    [SerializeField] private int _maxActiveAbilitiesCount;

    [SerializeField] private Transform _abilitiesParent;

    private List<Weapon> _weapons;
    private List<ProjectileWeapon> _projectileWeapons;
    private List<AbilityContainer> _abilities;


    public List<AbilityContainer> Abilities => _abilities;
    public List<Weapon> Weapons => _weapons;
    public List<ProjectileWeapon> ProjectileWeapons => _projectileWeapons;

    public int ActiveAbilitiesCount => _weapons.Count;
    public int MaxActiveAbilitiesCount => _maxActiveAbilitiesCount;

    public void Initialize()
    {
        _weapons = new List<Weapon>();
        _projectileWeapons = new List<ProjectileWeapon>();
        _abilities = new List<AbilityContainer>();
    }

    public AbilityContainer Add(AbilityContainer ability)
    {
        if (ability as Weapon != null && ActiveAbilitiesCount >= _maxActiveAbilitiesCount)
        { 
            return null; 
        }

        AbilityContainer newAbility = Object.Instantiate(ability, _abilitiesParent);

        newAbility.Initialize();

        _abilities.Add(newAbility);

        if (newAbility as Weapon != null) // add to weapon list
        {
            _weapons.Add(newAbility as Weapon); 

            if (newAbility as ProjectileWeapon != null)
            {
                _projectileWeapons.Add(newAbility as ProjectileWeapon);
            }
        }

        return newAbility;
    }

    public bool Remove(AbilityContainer ability)
    {
        AbilityContainer removingAbility = Find(ability);

        if (removingAbility != null)
        {
            if (removingAbility as Weapon != null)
            {
                _weapons.Remove(removingAbility as Weapon);
            }
            if (removingAbility as ProjectileWeapon != null)
            {
                _projectileWeapons.Remove(removingAbility as ProjectileWeapon);
            }

            bool removed = _abilities.Remove(removingAbility);

            if (removingAbility != null && removed)
            {
                removingAbility.DestroyAbility();
            }

            return removed;
        }
        else return false;
    }

    public AbilityContainer Find(AbilityContainer ability)
    {
        if (ability == null) return null;

        return _abilities.Find(item => item.Name == ability.Name);
    }
}
