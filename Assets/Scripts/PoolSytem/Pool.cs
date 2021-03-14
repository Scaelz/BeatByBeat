using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : IPool<T> where T : IRevertable, IActivatable
{
    Queue<T> members;
    IFactory<T> factory;
    int prewarm;

    public Pool(IFactory<T> factory)
    {
        this.factory = factory;
        members = new Queue<T>();
    }

    public T Create()
    {
        T newMember = factory.Create();
        members.Enqueue(newMember);
        return newMember;
    }

    public T Allocate()
    {
        if (members.Count == 0)
        {
            Create();
        }
        T result = members.Dequeue();
        result.Activate();
        return result;
    }

    public void ResetMember(T obj)
    {
        obj.Revert();
        if (!members.Contains(obj))
            members.Enqueue(obj);
    }
}
