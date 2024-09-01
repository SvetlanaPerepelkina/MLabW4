using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace L4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker Bworker;
        public MainWindow()
        {
            InitializeComponent();
            Bworker = new BackgroundWorker();
            Bworker.WorkerReportsProgress = true;
            Bworker.ProgressChanged += worker_ProgressChanged;
            Bworker.RunWorkerCompleted += worker_RunWorkerCompleted;
         
        }
        int i = 0;
        double start;
        double stop;
        double N;
        Window1 window1 = new Window1();


        //=========================================
        private void Dispatcher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CalculateAsynk();
            }
            catch { new Exception(); }
        }

        private void worker_Click(object sender, RoutedEventArgs e)
        {

            pBar.Value = 0;
            window1.ShowDialog();
            Bworker.DoWork += worker_DoWork;
            Bworker.RunWorkerAsync(1000);
            dispatcher.IsEnabled = false;
            worker.IsEnabled = false;
        }


        public void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var step = Math.Round(N / 100);
            double h;
            double a = start;
            double b = stop;
            double x = 0;


            h = (b - a) / N;
            var sum = 0d;

            for (var i = 1; i <= N; i++)
            {
                x = a + i * h;
                sum += Math.Pow(x, 9);
                var result = h * sum;
                //Thread.Sleep(1);
                if (i % step == 0)
                {
                    if (Bworker != null && Bworker.WorkerReportsProgress)
                    {
                        Bworker.ReportProgress((int)(i / step));
                    }
                }
            }

            int max = (int)e.Argument;
            int result1 = 0;
            
            for (int i = 0; i < max; i++)
            {
                int progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                if (i % 42 == 0)
                {
                    result1++;
                   (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
                }
                else
                   (sender as BackgroundWorker).ReportProgress(progressPercentage);
               Thread.Sleep(1);

            }
            e.Result = result1;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Задача выполнена");
            dispatcher.IsEnabled = true;
            worker.IsEnabled = true;
        }

       
        public Task CalculateAsynk()
        {
            dispatcher.IsEnabled = false;
            worker.IsEnabled = false;

            window1.ShowDialog();
            var step = Math.Round(N / 100);
            start = Convert.ToInt32(window1.Start.Text);
            stop = Convert.ToInt32(window1.Stop.Text);
            N = Convert.ToInt32(window1.N.Text);

            double h;
            double a = start;
            double b = stop;
            double x = 0;
            
            h = (b - a) / N;
            var sum = 0d;
            int max = 100;

            return Task.Run(() =>
             {
                 //MessageBox.Show(" " + h + "\n" + a + "\n" + b);
                 int progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                 for (var i = 1; i <= N; i++)
                 {
                     x = a + i * h;
                     sum += Math.Pow(x, 9); ;
                     // MessageBox.Show(""  + x +"\n" + sum);
                     // if (i % step == 0)
                     {
                         Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => pBar.Value += (i / step)));
                         step++;
                     }

                 }
                 var result = (h * sum);
                 MessageBox.Show("Результат вычислений\t" + result);
             });
                
        }
    }
}
 

