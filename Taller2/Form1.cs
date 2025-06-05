using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taller2
{
    public partial class Form1 : Form
    {
        int size;
        int limitDown, limitUp;
        Stopwatch stopwatch;
        List<int> allRandoms = new List<int>();
        List<int> slightlyAscend = new List<int>();
        List<int> slightlyDescend = new List<int>();
        List<int> orderer = new List<int>();
        List<double> dataTime = new List<double>();
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearCharts();

            size = Convert.ToInt32(textBox1.Text);
            limitDown = Convert.ToInt32(textBox2.Text);
            limitUp = Convert.ToInt32(textBox3.Text);
            
            generateData();
            sorts();

            string[] etiquetas = new string[]
            {
                "random",
                "levemente Asc",
                "levemente desc",
                "ordenado",
                "HeapSort",
                "bubblesort"
            };

            for (int i = 0; i < dataTime.Count && i < etiquetas.Length; i++)
            {
                dataGridView1.Rows.Add(
                    etiquetas[i] + i,
                    dataTime[i].ToString("F5"),
                    (dataTime[i] * 1000).ToString("F2") + " ms",
                    size.ToString()
                );
            }
        }

        void generateData()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            //aleatorio
            for (int i = 0; i < size; i++)
            {
                int num = random.Next(limitDown, limitUp + 1);
                allRandoms.Add(num);
                chart1.Series[0].Points.Add(num);
            }

            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.TotalSeconds);

            // Calcular y mostrar estadísticas para datos aleatorios
            var randomStats = dataGroup.CalculateStatistics(allRandoms);
            DisplayStatisticsInGrid(dataGridAle, randomStats);

            //ordanado levemente ascendentemente
            stopwatch = new Stopwatch();
            stopwatch.Start();

            slightlyAscend = new List<int>();
            int blockSize = size / 5;
            for (int i = 0; i < 5; i++)
            {
                int min = limitDown + i * (limitUp - limitDown) / 5;
                int max = limitDown + (i + 1) * (limitUp - limitDown) / 5;
                List<int> block = new List<int>();
                for (int j = 0; j < blockSize; j++)
                    block.Add(random.Next(min, max + 1));

                block.Sort();
                slightlyAscend.AddRange(block);
            }
            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.TotalSeconds);
            foreach (var val in slightlyAscend)
                chart2.Series[0].Points.Add(val);

            // Calcular y mostrar estadísticas para datos levemente ascendentes
            var ascStats = dataGroup.CalculateStatistics(slightlyAscend);
            DisplayStatisticsInGrid(dataGridLOA, ascStats);

            //ordanado levemente descendentemente
            stopwatch = new Stopwatch();
            stopwatch.Start();

            slightlyDescend = new List<int>();
            for (int i = 4; i >= 0; i--)
            {
                int min = limitDown + i * (limitUp - limitDown) / 5;
                int max = limitDown + (i + 1) * (limitUp - limitDown) / 5;
                List<int> block = new List<int>();
                for (int j = 0; j < blockSize; j++)
                    block.Add(random.Next(min, max + 1));

                block.Sort((a, b) => b.CompareTo(a));
                slightlyDescend.AddRange(block);
            }
            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.TotalSeconds);
            foreach (var val in slightlyDescend)
                chart3.Series[0].Points.Add(val);

            // Calcular y mostrar estadísticas para datos levemente descendentes
            var descStats = dataGroup.CalculateStatistics(slightlyDescend);
            DisplayStatisticsInGrid(dataGridLOD, descStats);

            //ordenado
            stopwatch = new Stopwatch();
            stopwatch.Start();

            orderer = new List<int>(allRandoms);
            orderer.Sort();
            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.TotalSeconds);
            foreach (var val in orderer)
                chart4.Series[0].Points.Add(val);

            // Calcular y mostrar estadísticas para datos ordenados
            var orderedStats = dataGroup.CalculateStatistics(orderer);
            DisplayStatisticsInGrid(dataGridORD, orderedStats);
        }

        private void DisplayStatisticsInGrid(DataGridView grid, Dictionary<string, double> stats)
        {
            grid.Rows.Clear();
            grid.Columns.Clear();

            // Configurar columnas
            grid.Columns.Add("Estadística", "Estadística");
            grid.Columns.Add("Valor", "Valor");

            // Agregar filas con los datos
            foreach (var stat in stats)
            {
                grid.Rows.Add(stat.Key, stat.Value.ToString("F2"));
            }
        }

        void sorts()
        {
            int[] dataForHeap = (int[])allRandoms.ToArray().Clone();
            int[] dataForBubble = (int[])allRandoms.ToArray().Clone();
            int heapSortTime = 0;
            int bubbleSortTime = 0;

            stopwatch = new Stopwatch();
            stopwatch.Start();

            HeapSort(dataForHeap);

            stopwatch.Stop();
            heapSortTime = (int)stopwatch.ElapsedMilliseconds;
            dataTime.Add(stopwatch.Elapsed.TotalSeconds);
            foreach (var val in dataForHeap)
                chart5.Series[0].Points.Add(val);



            stopwatch = new Stopwatch();
            stopwatch.Start();

            var resultBubble = BubbleSort(dataForBubble);

            stopwatch.Stop();
            bubbleSortTime = (int)stopwatch.ElapsedMilliseconds;
            dataTime.Add(stopwatch.Elapsed.TotalSeconds);
            foreach (var val in resultBubble)
                chart6.Series[0].Points.Add(val);


            chart7.Series[0].Points.Clear();
            chart7.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart7.Series[0].Points.AddXY("HeapSort", heapSortTime);
            chart7.Series[0].Points.AddXY("BubbleSort", bubbleSortTime);
        }

        void clearCharts()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();
            chart6.Series[0].Points.Clear();
            chart7.Series[0].Points.Clear();
            dataGridView1.Rows.Clear();
            allRandoms.Clear();
            slightlyAscend.Clear();
            slightlyDescend.Clear();
            dataTime.Clear();
        }

        int[] BubbleSort(int[] inputArray)
        {
            int n = inputArray.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (inputArray[j] < inputArray[j + 1])
                    {
                        int temp = inputArray[j];
                        inputArray[j] = inputArray[j + 1];
                        inputArray[j + 1] = temp;
                    }
                }
            }
            return inputArray;
        }

        public void HeapSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                Heapify(arr, i, 0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputSearch.Text))
            {
                infoSearch.Text = "Por favor ingrese un valor a buscar";
                return;
            }

            if (!int.TryParse(inputSearch.Text, out int searchValue))
            {
                infoSearch.Text = "Por favor ingrese un número válido";
                return;
            }

            // Limpiar los DataGridViews y Charts
            ClearSearchGrids();
            ClearSearchCharts();

            // Configurar los DataGridViews
            ConfigureSearchGrids();

            // Realizar búsquedas en cada conjunto de datos
            var times = new Dictionary<string, (double linear, double interpol, bool found)>();
            bool foundInAny = false;
            
            var result1 = PerformSearches(allRandoms, dataGridTAL, "Aleatorios", searchValue);
            times["Aleatorios"] = result1;
            foundInAny |= result1.found;

            var result2 = PerformSearches(slightlyAscend, dataGridTLOA, "Levemente Ascendente", searchValue);
            times["Levemente Asc"] = result2;
            foundInAny |= result2.found;

            var result3 = PerformSearches(slightlyDescend, dataGridTLOD, "Levemente Descendente", searchValue);
            times["Levemente Desc"] = result3;
            foundInAny |= result3.found;

            var result4 = PerformSearches(orderer, dataGridTOA, "Ordenados", searchValue);
            times["Ordenados"] = result4;
            foundInAny |= result4.found;

            // Actualizar los gráficos
            UpdateSearchCharts(times);

            // Actualizar mensaje según si se encontró o no el valor
            if (!foundInAny)
            {
                infoSearch.Text = $"El valor {searchValue} no se encontró en ningún conjunto de datos";
            }
        }

        private void ClearSearchGrids()
        {
            dataGridTAL.Rows.Clear();
            dataGridTLOA.Rows.Clear();
            dataGridTLOD.Rows.Clear();
            dataGridTOA.Rows.Clear();
        }

        private void ClearSearchCharts()
        {
            chartBlineal.Series[0].Points.Clear();
            chartIS.Series[0].Points.Clear();
        }

        private void ConfigureSearchGrids()
        {
            string[] grids = { "dataGridTAL", "dataGridTLOA", "dataGridTLOD", "dataGridTOA" };
            foreach (string gridName in grids)
            {
                DataGridView grid = Controls.Find(gridName, true).FirstOrDefault() as DataGridView;
                if (grid != null)
                {
                    grid.Columns.Clear();
                    grid.Columns.Add("Algoritmo", "Algoritmo");
                    grid.Columns.Add("Tiempo", "Tiempo (ms)");
                    grid.Columns["Tiempo"].DefaultCellStyle.Format = "F6";
                }
            }
        }

        private void UpdateSearchCharts(Dictionary<string, (double linear, double interpol, bool found)> times)
        {
            // Configurar gráfico de búsqueda lineal
            chartBlineal.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chartBlineal.Series[0].Name = "";
            chartBlineal.Series[0].IsVisibleInLegend = false;
            chartBlineal.Legends[0].Enabled = false;
            chartBlineal.ChartAreas[0].AxisX.Title = "Conjunto de Datos";
            chartBlineal.ChartAreas[0].AxisY.Title = "Tiempo (ms)";

            // Configurar gráfico de búsqueda por interpolación
            chartIS.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chartIS.Series[0].Name = "";
            chartIS.Series[0].IsVisibleInLegend = false;
            chartIS.Legends[0].Enabled = false;
            chartIS.ChartAreas[0].AxisX.Title = "Conjunto de Datos";
            chartIS.ChartAreas[0].AxisY.Title = "Tiempo (ms)";

            // Agregar datos a los gráficos
            foreach (var kvp in times)
            {
                chartBlineal.Series[0].Points.AddXY(kvp.Key, kvp.Value.linear);
                chartIS.Series[0].Points.AddXY(kvp.Key, kvp.Value.interpol);
            }
        }

        private (double linear, double interpol, bool found) PerformSearches(List<int> data, DataGridView grid, string dataType, int searchValue)
        {
            // Búsqueda Lineal
            var linearResult = search.LinearSearch(data, searchValue);
            grid.Rows.Add("Lineal", linearResult.time.ToString("F6"));

            // Búsqueda por Interpolación
            var interpolationResult = search.InterpolationSearch(data, searchValue);
            grid.Rows.Add("Interpolar", interpolationResult.time.ToString("F6"));

            // Actualizar el mensaje de información si se encontró el valor
            if (linearResult.found || interpolationResult.found)
            {
                infoSearch.Text = $"Valor {searchValue} encontrado en {dataType}";
            }

            return (linearResult.time, interpolationResult.time, linearResult.found || interpolationResult.found);
        }

        private void Heapify(int[] arr, int n, int i)
        {
            int smallest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && arr[left] < arr[smallest])
                smallest = left;

            if (right < n && arr[right] < arr[smallest])
                smallest = right;

            if (smallest != i)
            {
                int swap = arr[i];
                arr[i] = arr[smallest];
                arr[smallest] = swap;

                Heapify(arr, n, smallest);
            }
        }

    }
}
