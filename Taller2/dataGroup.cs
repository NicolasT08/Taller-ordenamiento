using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taller2
{
    class dataGroup
    {
        public static Dictionary<string, double> CalculateStatistics(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0)
                return null;

            var stats = new Dictionary<string, double>();
            
            // Valor mínimo y máximo
            int min = numbers.Min();
            int max = numbers.Max();
            
            // Frecuencia de mínimo y máximo
            int minCount = numbers.Count(x => x == min);
            int maxCount = numbers.Count(x => x == max);
            
            // Suma total
            double sum = numbers.Sum();
            
            // Promedio
            double average = numbers.Average();
            
            // Mediana
            var sortedNumbers = numbers.OrderBy(x => x).ToList();
            double median;
            int mid = sortedNumbers.Count / 2;
            if (sortedNumbers.Count % 2 == 0)
                median = (sortedNumbers[mid - 1] + sortedNumbers[mid]) / 2.0;
            else
                median = sortedNumbers[mid];
            
            // Moda
            var mode = numbers.GroupBy(x => x)
                            .OrderByDescending(g => g.Count())
                            .First().Key;

            // Agregar resultados al diccionario
            stats.Add("Mínimo", min);
            stats.Add("Máximo", max);
            stats.Add("Frecuencia Mínimo", minCount);
            stats.Add("Frecuencia Máximo", maxCount);
            stats.Add("Promedio", average);
            stats.Add("Mediana", median);
            stats.Add("Suma Total", sum);
            stats.Add("Moda", mode);

            return stats;
        }
    }
}
