using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace AlqoGreetBot.Common
{
    public class CustomStack<T> : Stack<T>, INotifyCollectionChanged 
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CustomStack(int capacity) : base(capacity)
        {
        }

        public new void Push(T item)
        {
            base.Push(item);
            this.CollectionChanged.Invoke(this, null);
        }
    }
}
