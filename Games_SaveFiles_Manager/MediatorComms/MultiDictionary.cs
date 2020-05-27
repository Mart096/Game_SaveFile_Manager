﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games_SaveFiles_Manager.MediatorComms
{
    public class MultiDictionary<T, K> : Dictionary<T, List<K>>
    {
        #region Methods

        private void EnsureKey(T key)
        {
            if (!ContainsKey(key))
            {
                this[key] = new List<K>(1);
            }
            else
            {
                if (this[key] == null)
                    this[key] = new List<K>(1);
            }
        }

        public void AddValue(T key, K newItem)
        {
            EnsureKey(key);
            this[key].Add(newItem);
        }

        public void AddValues(T key, IEnumerable<K> newItems)
        {
            EnsureKey(key);
            this[key].AddRange(newItems);
        }

        public bool RemoveValue(T key, K value)
        {
            if (!ContainsKey(key))
                return false;

            this[key].Remove(value);

            if (this[key].Count == 0)
                this.Remove(key);

            return true;
        }

        public bool RemoveAllValue(T key, Predicate<K> match)
        {
            if (!ContainsKey(key))
                return false;

            this[key].RemoveAll(match);

            if (this[key].Count == 0)
                this.Remove(key);

            return true;
        }

        #endregion
    }
}
