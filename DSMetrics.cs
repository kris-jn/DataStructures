using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public abstract class SortingMetrics<T> : ISorting<T>
        where T : IComparable
    {
        public long Swaps { get; set; }
        public long Comparisons { get; set; }

        public T[] Sort(T[] data)
        {
            Reset();
            return MetricSort(data);
        }

        public abstract T[] MetricSort(T[] data);

        private void Reset()
        {
            Swaps = 0;
            Comparisons = 0;
        }

        protected void Swap(T[] data, int left, int right)
        {
            var temp = data[left];
            data[left] = data[right];
            data[right] = temp;
            Swaps++;
        }

        protected void Assign(T[] data, int index, T value)
        {
            data[index] = value;
            Swaps++;
        }

        protected bool GreaterThan(T[] data, int left, int right)
        {
            if (Compare(data[left], data[right]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool LessThan(T[] data, int left, int right)
        {
            if (Compare(data[left], data[right]) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected int Compare(T t1, T t2)
        {
            return t1.CompareTo(t2);
        }

    }

    public class BubbleSort<T> : SortingMetrics<T>
        where T : IComparable
    {
        public override T[] MetricSort(T[] data)
        {
            bool again = false;
            do
            {
                again = false;
                for (int i = 1; i < data.Length; i++)
                {
                    if (GreaterThan(data, i - 1, i))
                    {
                        Swap(data, i - 1, i);
                        again = true;
                    }
                }
            } while (again);

            return data;
        }
    }

    public class InsertionSort<T> : SortingMetrics<T>
        where T : IComparable
    {
        public override T[] MetricSort(T[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = i; j < data.Length; j++)
                {
                    if (LessThan(data, i - 1, i))
                    {
                        Swap(data, j, j - 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return data;
        }
    }

    public class MergeSort<T> : SortingMetrics<T>
        where T : IComparable
    {
        public override T[] MetricSort(T[] data)
        {
            if (data.Length == 1)
            {
                return data;
            }

            int leftSize = data.Length / 2;
            int rightSize = data.Length - leftSize;
            T[] left = new T[leftSize];
            T[] right = new T[rightSize];

            Array.Copy(data, 0, left, 0, leftSize);
            Array.Copy(data, leftSize, right, 0, rightSize);

            MetricSort(left);
            MetricSort(right);
            return Merge(data, left, right);
        }

        private T[] Merge(T[] data, T[] left, T[] right)
        {
            int leftIndex = 0;
            int rightIndex = 0;
            int targetIndex = 0;
            var remaining = left.Length + right.Length;

            while (targetIndex < data.Length)
            {
                if (leftIndex >= left.Length)
                {
                    Assign(data, targetIndex, right[rightIndex++]);
                }
                else if (rightIndex >= right.Length)
                {
                    Assign(data, targetIndex, left[leftIndex++]);
                }
                else if (Compare(left[leftIndex], right[rightIndex]) < 0)
                {
                    Assign(data, targetIndex, left[leftIndex++]);
                }
                else
                {
                    Assign(data, targetIndex, right[rightIndex++]);
                }

                targetIndex++;
                remaining--;
            }

            return data;
        }

    }

    public class QuickSort<T> : SortingMetrics<T>
        where T : IComparable
    {
        public override T[] MetricSort(T[] data)
        {
            return QuickSortData(data, 0, data.Length - 1);

        }

        private Random pvtRandom = new Random();
        private T[] QuickSortData(T[] data, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = pvtRandom.Next(left, right);
                int newPivot = Partition(data, left, right, pivotIndex);
                QuickSortData(data, left, newPivot - 1);
                QuickSortData(data, newPivot + 1, right);
            }
            return data;
        }

        private int Partition(T[] data, int left, int right, int pivotIndex)
        {
            T pivot = data[pivotIndex];
            int storeIndex = left;
            for (int i = left; i < right; i++)
            {
                if (Compare(data[i], pivot) < 0)
                {
                    Swap(data, i, storeIndex++);
                }
            }
            Swap(data, storeIndex, right);
            return storeIndex;
        }
    }
}
