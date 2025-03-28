 
 


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ServiceModelEx
{
   public static class CollectionExtensions
   {
      public static void ForEach<T>(this IEnumerable<T> collection,Action<T> action)
      {
         if(collection == null)
         {
            throw new ArgumentNullException("collection");
         }
         if(action == null)
         {
            throw new ArgumentNullException("action");
         }
         foreach(T item in collection)
         {
            action(item);
         }
      }
      public static IEnumerable<U> ConvertAll<T,U>(this IEnumerable<T> collection,Converter<T,U> converter)
      {
         if(collection == null)
         {
            throw new ArgumentNullException("collection");
         }
         if(converter == null)
         {
            throw new ArgumentNullException("converter");
         }
         foreach(T item in collection)
         {
            yield return converter(item);
         }
      }
      public static IEnumerable<T> Complement<T>(this IEnumerable<T> collection1,IEnumerable<T> collection2) 
      {
         foreach(T item in collection1)
         {
            if(collection2.Contains(item) == false)
            {
               yield return item;
            }
         }
      }
      public static IEnumerable<T> Except<T>(IEnumerable<T> collection1,IEnumerable<T> collection2) where T : IEquatable<T>
      {
         IEnumerable<T> complement1 = Complement(collection1,collection2);
         IEnumerable<T> complement2 = Complement(collection2,collection1);
         return complement1.Union(complement2);
      }
      public static IEnumerable<T> Sort<T>(this IEnumerable<T> collection)
      {
         if(collection == null)
         {
            throw new ArgumentNullException("collection");
         }
         List<T> list = new List<T>(collection);
         list.Sort();

         foreach(T item in list)
         {
            yield return item;
         }
      }
      public static int FindIndex<T>(this IEnumerable<T> collection,T value) where T : IEquatable<T>
      {
         if(collection == null)
         {
            throw new ArgumentNullException("collection");
         }
         using(IEnumerator<T> iterator = collection.GetEnumerator())
         {
            int index = 0;

            while(iterator.MoveNext())
            {
               if(iterator.Current.Equals(value) == false)
               {
                  index++;
               }
               else
               {
                  return index;
               }
            }
            return -1;
         }
      }

      public static U[] UnsafeToArray<T,U>(this IEnumerable collection,Converter<T,U> converter)
      {
         if(collection == null)
         {
            throw new ArgumentNullException("collection");
         }
         if(converter == null)
         {
            throw new ArgumentNullException("converter");
         }

         return collection.UnsafeToArray<T>().ConvertAll(converter);      }

      public static T[] UnsafeToArray<T>(this IEnumerable collection)
      {
         if(collection == null)
         {
            throw new ArgumentNullException("collection");
         }
         return Collection.UnsafeToArray<T>(collection);
      }
   }
}