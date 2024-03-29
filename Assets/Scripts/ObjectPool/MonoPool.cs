using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonoPool<TObject> : ObjectPool<TObject> where TObject : MonoBehaviour, IPoolable
{
    protected TObject _prefab;
    protected Transform _parent;

    public TObject Prefab => _prefab;
    public Transform Parent => _parent;

    public MonoPool(TObject prefab, int capacity, Transform poolParent = null)
    {
        _prefab = prefab;

        _parent = new GameObject(prefab.name + " pool").transform;

        if (poolParent != null)
        {
            _parent.parent = poolParent;
        }

        _objects = new List<TObject>(capacity);
        _pulledObjects = new CleanupableList<TObject>();

        for (int i = 0; i < capacity; i++)
        {
            CreateObject();
        }
    }

    protected override void CreateObject()
    {
        if (_parent == null)
        {
            _parent = new GameObject(_prefab.name + " pool").transform;
        }

        TObject obj = Object.Instantiate(_prefab, _parent);

        obj.gameObject.SetActive(false);

        _objects.Add(obj);
    }

    public override void Release(TObject obj)
    {
        if (obj != null) obj.gameObject.SetActive(false);

        base.Release(obj);
    }

    public override TObject Pull()
    {
        TObject obj = base.Pull();

        if (obj == null)
        {
            return null;
        }

        obj.gameObject.SetActive(true);

        return obj;
    }

    public TObject PullDisabled()
    {
        return base.Pull();
    }

    public override List<TObject> PullObjects(int count)
    {
        List<TObject> objects = base.PullObjects(count);

        foreach (TObject obj in objects)
        {
            obj.gameObject.SetActive(true);
        }

        return objects;
    }

    public List<TObject> PullObjectsDisabled(int count)
    {
        return base.PullObjects(count);
    }

    public override void ClearPool()
    {
        if (_parent != null)
        {
            Object.Destroy(_parent.gameObject);

            _parent = null;

            base.ClearPool();
        }
    }
}
