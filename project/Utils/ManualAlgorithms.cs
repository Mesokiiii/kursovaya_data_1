using System;
using System.Collections.Generic;

namespace FootballLeague
{
    /// <summary>
    /// Ручная реализация алгоритмов сортировки, фильтрации и группировки
    /// Без использования LINQ и встроенных библиотечных функций
    /// </summary>
    public static class ManualAlgorithms
    {
        /// <summary>
        /// Проверяет, содержит ли коллекция хотя бы один элемент
        /// </summary>
        public static bool Any<T>(List<T> collection)
        {
            if (collection == null) return false;
            return collection.Count > 0;
        }

        /// <summary>
        /// Проверяет, содержит ли коллекция элемент, удовлетворяющий условию
        /// </summary>
        public static bool Any<T>(List<T> collection, Func<T, bool> predicate)
        {
            if (collection == null || predicate == null) return false;
            
            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Фильтрует коллекцию по условию
        /// </summary>
        public static List<T> Where<T>(List<T> collection, Func<T, bool> predicate)
        {
            List<T> result = new List<T>();
            if (collection == null || predicate == null) return result;
            
            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                    result.Add(collection[i]);
            }
            return result;
        }

        /// <summary>
        /// Находит первый элемент, удовлетворяющий условию, или возвращает default
        /// </summary>
        public static T FirstOrDefault<T>(List<T> collection, Func<T, bool> predicate)
        {
            if (collection == null || predicate == null) return default(T);
            
            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                    return collection[i];
            }
            return default(T);
        }

        /// <summary>
        /// Подсчитывает количество элементов, удовлетворяющих условию
        /// </summary>
        public static int Count<T>(List<T> collection, Func<T, bool> predicate)
        {
            if (collection == null || predicate == null) return 0;
            
            int count = 0;
            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Берет первые N элементов из коллекции
        /// </summary>
        public static List<T> Take<T>(List<T> collection, int count)
        {
            List<T> result = new List<T>();
            if (collection == null || count <= 0) return result;
            
            int limit = Math.Min(count, collection.Count);
            for (int i = 0; i < limit; i++)
            {
                result.Add(collection[i]);
            }
            return result;
        }

        /// <summary>
        /// Сортировка по возрастанию (QuickSort)
        /// </summary>
        public static List<T> OrderBy<T, TKey>(List<T> collection, Func<T, TKey> keySelector) 
            where TKey : IComparable<TKey>
        {
            if (collection == null || keySelector == null) return new List<T>();
            
            List<T> result = new List<T>(collection);
            QuickSort(result, keySelector, 0, result.Count - 1, true);
            return result;
        }

        /// <summary>
        /// Сортировка по убыванию (QuickSort)
        /// </summary>
        public static List<T> OrderByDescending<T, TKey>(List<T> collection, Func<T, TKey> keySelector) 
            where TKey : IComparable<TKey>
        {
            if (collection == null || keySelector == null) return new List<T>();
            
            List<T> result = new List<T>(collection);
            QuickSort(result, keySelector, 0, result.Count - 1, false);
            return result;
        }

        /// <summary>
        /// Многоуровневая сортировка
        /// </summary>
        public static List<T> ThenBy<T, TKey>(List<T> collection, Func<T, TKey> keySelector) 
            where TKey : IComparable<TKey>
        {
            return OrderBy(collection, keySelector);
        }

        /// <summary>
        /// Многоуровневая сортировка по убыванию
        /// </summary>
        public static List<T> ThenByDescending<T, TKey>(List<T> collection, Func<T, TKey> keySelector) 
            where TKey : IComparable<TKey>
        {
            return OrderByDescending(collection, keySelector);
        }

        /// <summary>
        /// Реализация QuickSort для сортировки
        /// </summary>
        private static void QuickSort<T, TKey>(List<T> list, Func<T, TKey> keySelector, int left, int right, bool ascending) 
            where TKey : IComparable<TKey>
        {
            if (left < right)
            {
                int pivotIndex = Partition(list, keySelector, left, right, ascending);
                QuickSort(list, keySelector, left, pivotIndex - 1, ascending);
                QuickSort(list, keySelector, pivotIndex + 1, right, ascending);
            }
        }

        private static int Partition<T, TKey>(List<T> list, Func<T, TKey> keySelector, int left, int right, bool ascending) 
            where TKey : IComparable<TKey>
        {
            TKey pivot = keySelector(list[right]);
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                TKey current = keySelector(list[j]);
                bool condition = ascending ? current.CompareTo(pivot) <= 0 : current.CompareTo(pivot) >= 0;
                
                if (condition)
                {
                    i++;
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }

            T temp2 = list[i + 1];
            list[i + 1] = list[right];
            list[right] = temp2;
            
            return i + 1;
        }

        /// <summary>
        /// Группировка элементов по ключу
        /// </summary>
        public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> collection, Func<T, TKey> keySelector)
        {
            Dictionary<TKey, List<T>> groups = new Dictionary<TKey, List<T>>();
            if (collection == null || keySelector == null) return groups;
            
            for (int i = 0; i < collection.Count; i++)
            {
                TKey key = keySelector(collection[i]);
                if (!groups.ContainsKey(key))
                {
                    groups[key] = new List<T>();
                }
                groups[key].Add(collection[i]);
            }
            
            return groups;
        }

        /// <summary>
        /// Проекция (преобразование) элементов коллекции
        /// </summary>
        public static List<TResult> Select<T, TResult>(List<T> collection, Func<T, TResult> selector)
        {
            List<TResult> result = new List<TResult>();
            if (collection == null || selector == null) return result;
            
            for (int i = 0; i < collection.Count; i++)
            {
                result.Add(selector(collection[i]));
            }
            return result;
        }

        /// <summary>
        /// Многоуровневая сортировка с несколькими критериями
        /// </summary>
        public static List<T> MultiSort<T>(List<T> collection, List<Comparison<T>> comparisons)
        {
            if (collection == null || comparisons == null || comparisons.Count == 0) 
                return new List<T>();
            
            List<T> result = new List<T>(collection);
            
            // Используем стабильную сортировку слиянием для многоуровневой сортировки
            MergeSort(result, comparisons, 0, result.Count - 1);
            
            return result;
        }

        private static void MergeSort<T>(List<T> list, List<Comparison<T>> comparisons, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                MergeSort(list, comparisons, left, middle);
                MergeSort(list, comparisons, middle + 1, right);
                Merge(list, comparisons, left, middle, right);
            }
        }

        private static void Merge<T>(List<T> list, List<Comparison<T>> comparisons, int left, int middle, int right)
        {
            int leftSize = middle - left + 1;
            int rightSize = right - middle;
            
            List<T> leftArray = new List<T>(leftSize);
            List<T> rightArray = new List<T>(rightSize);
            
            for (int i = 0; i < leftSize; i++)
                leftArray.Add(list[left + i]);
            for (int j = 0; j < rightSize; j++)
                rightArray.Add(list[middle + 1 + j]);
            
            int iIndex = 0, jIndex = 0, kIndex = left;
            
            while (iIndex < leftSize && jIndex < rightSize)
            {
                int compareResult = CompareMulti(leftArray[iIndex], rightArray[jIndex], comparisons);
                
                if (compareResult <= 0)
                {
                    list[kIndex] = leftArray[iIndex];
                    iIndex++;
                }
                else
                {
                    list[kIndex] = rightArray[jIndex];
                    jIndex++;
                }
                kIndex++;
            }
            
            while (iIndex < leftSize)
            {
                list[kIndex] = leftArray[iIndex];
                iIndex++;
                kIndex++;
            }
            
            while (jIndex < rightSize)
            {
                list[kIndex] = rightArray[jIndex];
                jIndex++;
                kIndex++;
            }
        }

        private static int CompareMulti<T>(T a, T b, List<Comparison<T>> comparisons)
        {
            for (int i = 0; i < comparisons.Count; i++)
            {
                int result = comparisons[i](a, b);
                if (result != 0)
                    return result;
            }
            return 0;
        }
    }
}
