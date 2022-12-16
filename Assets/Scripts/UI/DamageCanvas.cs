using System.Collections.Generic;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    [SerializeField] private DamageUI _damageUIPrefab;

    List<DamageUI> _damages = new List<DamageUI>();

    public void ShowDamage(int damage)
    {
        DamageUI dmg;

        if (_damages.Count == 0)
        {
            dmg = Instantiate(_damageUIPrefab, transform);
        }
        else
        {
            dmg = _damages[_damages.Count-1];
            _damages.Remove(dmg);
        }

        dmg.Initialize(this, damage);
        dmg.gameObject.SetActive(true);
    }

    public void Release(DamageUI damageUI)
    {
        damageUI.gameObject.SetActive(false);
        _damages.Add(damageUI);
    }
}
