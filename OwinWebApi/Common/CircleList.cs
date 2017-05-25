using System;
using System.Collections.Generic;

namespace Common
{
    public class CircleList<T>
    {
        readonly List<T> _list = new List<T>();

        public IList<T> Items => _list;

        public Int32 Index { get; set; }

        public void AddRange(IList<T> items)
        {
            _list.AddRange(items);
        }

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Remove(T item)
        {
            _list.Remove(item);
        }

        public T Current => _list[Index];

        public T Next
        {
            get
            {
                if (_list.Count.Equals(0))
                {
                    return default(T);
                }
                Index++;
                Index = Index % _list.Count;
                return _list[Index];
            }
        }

        public T Previous
        {
            get
            {
                if (_list.Count.Equals(0))
                {
                    return default(T);
                }
                Index--;
                if (Index < 0)
                {
                    Index = _list.Count - 1;
                }
                Index = Index % _list.Count;
                return _list[Index];
            }
        }
    }
}
