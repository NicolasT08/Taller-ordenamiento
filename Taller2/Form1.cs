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
        List<int> dataTime = new List<int>();
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
                    stopwatch.Elapsed.Seconds.ToString() + ": " + stopwatch.Elapsed.Milliseconds.ToString(),
                    stopwatch.ElapsedMilliseconds.ToString(),
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
            dataTime.Add(stopwatch.Elapsed.Seconds);


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
            dataTime.Add(stopwatch.Elapsed.Seconds);
            foreach (var val in slightlyAscend)
                chart2.Series[0].Points.Add(val);


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
            dataTime.Add(stopwatch.Elapsed.Seconds);
            foreach (var val in slightlyDescend)
                chart3.Series[0].Points.Add(val);


            //ordenado
            stopwatch = new Stopwatch();
            stopwatch.Start();

            orderer = new List<int>(allRandoms);
            orderer.Sort();
            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.Seconds);
            foreach (var val in orderer)
                chart4.Series[0].Points.Add(val);
        }

        void sorts()
        {
            int[] dataForHeap = (int[])allRandoms.ToArray().Clone();
            int[] dataForBubble = (int[])allRandoms.ToArray().Clone();

            stopwatch = new Stopwatch();
            stopwatch.Start();

            HeapSort(dataForHeap);

            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.Seconds);
            foreach (var val in dataForHeap)
                chart5.Series[0].Points.Add(val);

            stopwatch = new Stopwatch();
            stopwatch.Start();

            var resultBubble = BubbleSort(dataForBubble);

            stopwatch.Stop();
            dataTime.Add(stopwatch.Elapsed.Seconds);
            foreach (var val in resultBubble)
                chart6.Series[0].Points.Add(val);
        }

        void clearCharts()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();
            chart6.Series[0].Points.Clear();
            allRandoms.Clear();
            slightlyAscend.Clear();
            slightlyDescend.Clear();
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
