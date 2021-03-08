public interface IPool<T>
{
    T Allocate();
    void ResetMember(T obj);
}