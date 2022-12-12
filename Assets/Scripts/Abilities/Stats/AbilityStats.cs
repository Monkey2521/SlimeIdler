using UnityEngine;

[System.Serializable]
public class AbilityStats : IObjectStats
{
    [SerializeField] protected AbilityMarker _abilityMarker;
    [SerializeField] protected Level _level;

    public AbilityMarker AbilityMarker => _abilityMarker;
    public Level Level => _level;

    public virtual void Initialize()
    {
        _level.Initialize();
    }

    public virtual void GetUpgrade(Upgrade upgrade) 
    {

    }

    public virtual void DispelUpgrade(Upgrade upgrade)
    {

    }
}
