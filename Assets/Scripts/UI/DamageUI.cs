using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour, IPoolable
{
    [SerializeField] private Text _damageValueText;
    [SerializeField] private Animator _animator;

    private DamageCanvas _pool;

    public void ResetObject()
    {
        _pool = null;
    }

    public void Initialize(DamageCanvas pool, int damageValue)
    {
        _animator.SetTrigger("Show");
        _pool = pool;
        _damageValueText.text = damageValue.ToString();
    }

    public void ReturnToPool()
    {
        _pool.Release(this);
    }

    private void OnDisable()
    {
        ReturnToPool();
    }
}
