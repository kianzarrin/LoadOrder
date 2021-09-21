namespace LoadOrderTool.Helpers {
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Hashtable<TKey, TValue> where TValue : class {
        Hashtable hashtable_;

        public Hashtable() { hashtable_ = new Hashtable(); }
        public Hashtable(IDictionary<TKey, TValue> d) { hashtable_ = new Hashtable(d as IDictionary); }
        public Hashtable(int capacity) { hashtable_ = new Hashtable(capacity); }

        public virtual TValue this[TKey key] {
            get => (TValue)hashtable_[key];
            set => hashtable_[key] = value;
        }

        public virtual ICollection<TKey> Keys => hashtable_.Keys.Cast<TKey>().ToArray();
        public virtual ICollection<TValue> Values => hashtable_.Values.Cast<TValue>().ToArray();
        public virtual void Add(TKey key, TValue value) => hashtable_.Add(key, value);
        public virtual bool Contains(TKey key) => hashtable_.Contains(key);
        public virtual bool ContainsKey(TKey key) => hashtable_.ContainsKey(key);
        public virtual bool ContainsValue(TValue value) => hashtable_.ContainsValue(value);
        public virtual void Remove(TKey key) => hashtable_.Remove(key);
        public virtual int Count => hashtable_.Count;
    }
}
