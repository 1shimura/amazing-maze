namespace Client.Factory
{
    public interface IFactory<T> {
        T Create();
    }
}