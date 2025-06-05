using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Taller2
{
    class search
    {
        public static (bool found, double time) LinearSearch(List<int> data, int target)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool found = false;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == target)
                {
                    found = true;
                    break;
                }
            }

            stopwatch.Stop();
            return (found, stopwatch.Elapsed.TotalMilliseconds);
        }

        public static (bool found, double time) InterpolationSearch(List<int> data, int target)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool found = false;
            int left = 0;
            int right = data.Count - 1;

            while (left <= right && target >= data[left] && target <= data[right])
            {
                if (left == right)
                {
                    if (data[left] == target)
                        found = true;
                    break;
                }

                // Fórmula de interpolación
                int pos = left + (((right - left) * (target - data[left])) / (data[right] - data[left]));

                if (data[pos] == target)
                {
                    found = true;
                    break;
                }

                if (data[pos] < target)
                    left = pos + 1;
                else
                    right = pos - 1;
            }

            stopwatch.Stop();
            return (found, stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
