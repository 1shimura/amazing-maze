using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client.Factory;

namespace Client
{
    public class Pool<T> : IEnumerable where T : IResettable
    {
        public List<T> Members = new List<T>();
        public HashSet<T> Unavailable = new HashSet<T>();

        private IFactory<T> _factory;

        public Pool(IFactory<T> factory, int poolSize = 0)
        {
            _factory = factory;

            for (var i = 0; i < poolSize; i++)
            {
                Create(null);
            }
        }

        public void Allocate(Action<T> onComplete)
        {
            foreach (var item in Members.Where(item => !Unavailable.Contains(item)))
            {
                Unavailable.Add(item);
                item.PrewarmSetup();
                onComplete?.Invoke(item);
            }

            Create(item =>
            {
                Unavailable.Add(item);
                onComplete?.Invoke(item);
            });
        }

        public void Release(T member)
        {
            member.Reset();
            Unavailable.Remove(member);
        }

        public void ReleaseAll()
        {
            foreach (var unavailableMember in Unavailable)
            {
                unavailableMember.Reset();
            }

            Unavailable.Clear();
        }

        private void Create(Action<T> onCreate)
        {
            _factory.Create(item =>
            {
                Members.Add(item);
                onCreate?.Invoke(item);
            });
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Members.GetEnumerator();
        }
    }
}