using System;

namespace Client.Factory
{
    public interface IFactory<T> {
        void Create(Action<T> onComplete);
    }
}