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
                Create();
            }
        }

        public T Allocate()
        {
            foreach (var item in Members.Where(item => !Unavailable.Contains(item)))
            {
                Unavailable.Add(item);
                item.PrewarmSetup();
                return item;
            }

            var newMember = Create();
            Unavailable.Add(newMember);
            return newMember;
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

        private T Create()
        {
            var member = _factory.Create();
            Members.Add(member);
            return member;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Members.GetEnumerator();
        }
    }
}