//Implement the data structure "hash table" in a class HashTable<K,T>. 
//Keep the data in array of lists of key-value pairs (LinkedList<KeyValuePair<K,T>>[])
//with initial capacity of 16. When the hash table load runs over 75%, perform resizing
//to 2 times larger capacity. Implement the following methods and properties: 
//Add(key, value), Find(key)value, Remove( key), Count, Clear(), this[], Keys. 
//Try to make the hash table to support iterating over its elements with foreach.

using System;
using System.Collections.Generic;
using System.Linq;

namespace _04.HashTableImplementing
{
    public class MyHashTable<K,T>
    {
        private LinkedList<KeyValuePair<K, T>>[] container;
        private int capacity;
        private int count;

        public int Capacity
        {
            get 
            {
                return this.capacity;
            }
            private set { }
        }

        public int Count
        {
            get
            {
                return this.count;
            }
            private set { }
        }

        public List<K> Keys
        {
            get
            {
                List<K> keys = new List<K>();

                foreach (var item in this.container)
                {
                    if (item != null)
                    {
                        var currentLinkedList = item.First;

                        while (currentLinkedList != null)
                        {
                            keys.Add(currentLinkedList.Value.Key);
                            currentLinkedList = currentLinkedList.Next;
                        }
                    }
                }

                return keys;
            }
        }

        public T this[K key]
        {
            get
            {
                return Find(key);
            }
            set
            {
                int index = key.GetHashCode() % this.container.Length;

                if (this.container[index] == null)
                {
                    this.container[index] = new LinkedList<KeyValuePair<K, T>>();
                }

                bool exist = false;
                var currentElement = this.container[index].First;

                while (currentElement != null)
                {
                    if (currentElement.Value.Key.Equals(key))
                    {
                        currentElement.Value = new KeyValuePair<K, T>(key, value);
                        exist = true;
                        break;
                    }

                    currentElement = currentElement.Next;
                }

                if (!exist)
                {
                    throw new ArgumentException("Key does not exist.");
                }
            }
        }

        public MyHashTable()
        {
            this.container = new LinkedList<KeyValuePair<K, T>>[16];
            this.count = 0;
            this.capacity = 0;
        }

        public T Find(K key)
        {
            if (key == null)
            {
                throw new ArgumentException("Key cannot be null.");
            }

            int index = Math.Abs(key.GetHashCode() % this.container.Length);

            if (this.container[index] == null)
            {
                throw new ArgumentException("There is no element with this key.");
            }
            else
            {
                foreach (var item in this.container[index])
                {
                    if (key.Equals(item.Key))
                    {
                        return item.Value;
                    }
                }
            }

            throw new ArgumentException("There is no element with this key.");
        }

        public void Add(K key, T value)
        {
            if (key == null || value == null)
            {
                throw new ArgumentException("Add has some invalid arguments. Key and value cannot be null");
            }

            if (this.Capacity >= this.container.Length*0.75)
            {
                ExpandContainerCapacity();
            }

            int index = Math.Abs(key.GetHashCode() % this.container.Length);
            int hashCOde = key.GetHashCode();

            if (this.container[index] == null)
            {
                this.container[index] = new LinkedList<KeyValuePair<K, T>>();
                this.capacity += 1;
            }

            var currentElement = this.container[index].First;

            while (currentElement != null)
            {
                if (currentElement.Value.Key.Equals(key))
                {
                    throw new ArgumentException("Key already exist.");
                }

                currentElement = currentElement.Next;
            }

            this.container[index].AddLast(new KeyValuePair<K, T>(key, value));
            this.count += 1;
        }

        public void Remove(K key) 
        {
            if (key == null)
            {
                 throw new ArgumentException("Key cannot be null.");
            }

            int index = Math.Abs(key.GetHashCode() % this.container.Length);

            if (this.container[index] == null)
            {
                throw new ArgumentException("Key does not exist.");
            }

            var currentElement = this.container[index].First;
            bool exist = false;
            while (currentElement != null)
            {
                if (currentElement.Value.Key.Equals(key))
                {
                    this.container[index].Remove(currentElement);
                    this.count -= 1;
                    exist = true;
                    break;
                }

                currentElement = currentElement.Next;
            }

            if (this.container[index].First == null)
            {
                this.capacity -= 1;
            }

            if (!exist)
            {
                throw new ArgumentException("Key does not exist.");
            }
        }

        public void Clear()
        {
            LinkedList<KeyValuePair<K,T>>[] clearedList = new LinkedList<KeyValuePair<K,T>>[16];
            this.container = clearedList;

            this.count = 0;
            this.capacity = 0;
        }

        private void ExpandContainerCapacity()
        {
            LinkedList<KeyValuePair<K, T>>[] expandedArray = new LinkedList<KeyValuePair<K, T>>[this.Capacity * 2];
            Array.Copy(this.container, expandedArray, this.Capacity);
            this.container = expandedArray;
        }
    }
}
