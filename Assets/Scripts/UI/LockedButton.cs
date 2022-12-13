using UnityEngine;

public class LockedButton : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _locked;

    public void Click()
    {
        if (_locked)
        {
            _animator.SetTrigger("Unlock");
        }
        else
        {
            _animator.SetTrigger("LockedClick");
        }
    }

    public void Unlocked()
    {

    }
}
