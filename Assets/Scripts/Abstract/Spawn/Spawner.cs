using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] protected bool _isDebug;

    protected abstract void Spawn(Vector3 position);
}
