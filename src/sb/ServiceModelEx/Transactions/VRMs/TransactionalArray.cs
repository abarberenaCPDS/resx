 
 


using System;
using System.Collections;
using System.Collections.Generic;


namespace ServiceModelEx.Transactional
{
   public class TransactionalArray<T> : TransactionalCollection<T[],T>,ICloneable,IList
   {
      public TransactionalArray(T[] array) : base(array)
      {}
      public TransactionalArray(IEnumerable<T> collection) : base(ToArray(collection))
      {}
      static T[] ToArray(IEnumerable<T> collection)
      {
         int length = 0;
         foreach(T t in collection)
         {
            length++;
         }
         T[] array = new T[length];
         int index = 0;
         foreach(T t in collection)
         {
            array[index] = t;
            index++;
         }
         return array;
      }
      public TransactionalArray(int size) : base(new T[size])
      {}

      public T this[int index]
      {
         get
         {
            return Value[index];
         }
         set
         {
            Value[index] = value;
         }
      }
      public int Length
      {
         get
         {
            return Value.Length;
         }
      }
      public void CopyTo(ref TransactionalArray<T> array,int index)
      {
         T[] values = null;
         Value.CopyTo(values,index);
         array = new TransactionalArray<T>(values);
      }

      public bool IsFixedSize
      {
         get
         {
            return Value.IsFixedSize;
         }
      }
      public bool IsReadOnly
      {
         get
         {
            return Value.IsReadOnly;
         }
      }
      public object Clone()
      {
         return Value.Clone();
      }
      int IList.Add(object value)
      {
         return (Value as IList).Add(value);
      }
      void IList.Clear()
      {
         (Value as IList).Clear();
      }
      bool IList.Contains(object value)
      {
         return (Value as IList).Contains(value);
      }
      int IList.IndexOf(object value)
      {
         return (Value as IList).IndexOf(value);
      }
      void IList.Insert(int index,object value)
      {
         (Value as IList).Insert(index,value);
      }
      void IList.Remove(object value)
      {
         (Value as IList).Remove(value);
      }
      void IList.RemoveAt(int index)
      {
         (Value as IList).RemoveAt(index);
      }
      object IList.this[int index]
      {
         get
         {
            return (Value as IList)[index];
         }
         set
         {
            (Value as IList)[index] = value;
         }
      }
      int ICollection.Count
      {
         get
         {
            return (Value as IList).Count;
         }
      }
      public void CopyTo(Array array,int index)
      {
         Value.CopyTo(array,index);
      }
      public object SyncRoot
      {
         get
         {
            return (Value as IList).SyncRoot;
         }
      }
      public bool IsSynchronized
      {
         get
         {
            return (Value as IList).IsSynchronized;
         }
      }
   }
}

