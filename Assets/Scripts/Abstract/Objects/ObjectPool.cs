using System.Collections.Generic;

public abstract class ObjectPool<TObject> where TObject : class, IPoolable
{
    protected List<TObject> _objects;
    protected CleanupableList<TObject> _pulledObjects;

    public List<TObject> Objects => _objects;
    public CleanupableList<TObject> PulledObjects => _pulledObjects;
    public bool IsEmpty => _objects.Count == 0;

    protected abstract void CreateObject();

    public virtual TObject Pull()
    {
        if (IsEmpty)
        {
            CreateObject();
        }

        TObject obj = _objects[_objects.Count - 1];
        _objects.RemoveAt(_objects.Count - 1);
        _pulledObjects.Add(obj);

        return obj;
    }

    public virtual List<TObject> PullObjects(int count)
    {
        List<TObject> objects = new List<TObject>();
        
        for (int i = 0; i < count; i++)
        {
            objects.Add(Pull());
        }

        return objects;
    }

    public virtual void Release(TObject obj)
    {
        if (obj != null)
        {
            obj.ResetObject();
            _objects.Add(obj);
            _pulledObjects.Remove(obj, true);
        }
        else return;
    }

    public virtual void AddObject(TObject obj)
    {
        if (obj != null)
        {
            _objects.Add(obj);
        }
        else return;
    }

    public virtual void ClearPool()
    {
        _objects.Clear();
    }
}
