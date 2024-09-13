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
            double step = Math.Round(N / 100);
            start = Convert.ToInt32(window1.Start.Text);
            stop = Convert.ToInt32(window1.Stop.Text);
            N = Convert.ToInt32(window1.N.Text);
            double a = start;
            double b = stop;
        }

        int i = 0;
        
        double start;
        double stop;
        double N;
        double result = 0;
        double h;
        double x = 0;
        Window1 window1 = new Window1();

        //=========================================
        private void Dispatcher_Click(object sender, RoutedEventArgs e)
        {
            dispatcher.IsEnabled = false;
            worker.IsEnabled = false;
            CalculateAsynk();
        }

        private void worker_Click(object sender, RoutedEventArgs e)
        {
            pBar.Value = 0;
            window1.ShowDialog();
            Bworker.DoWork += worker_DoWork;
            Bworker.RunWorkerAsync(1000);
            dispatcher.IsEnabled = false;
            worker.IsEnabled = false;
            double step = Math.Round(N / 100);
            start = Convert.ToInt32(window1.Start.Text);
            stop = Convert.ToInt32(window1.Stop.Text);
            N = Convert.ToInt32(window1.N.Text);
            Calc(start, stop, N);
            res.Foreground = Brushes.LightGray;
        }

        private double Calc( double start, double stop, double N)
        {
            {
                var step = Math.Round(N / 100);
                h = (stop - start) / N;
                double sum = 0;

                for (var i = 1; i <= N; i++)
                {
                    x = start + i * h;
                    sum += Math.Pow(x, 9);

                    if (i % step == 0)
                    {
                        if (Bworker != null && Bworker.WorkerReportsProgress)
                        {
                            Bworker.ReportProgress((int)(i / step));
                        }
                    }

                }
                result = h * sum;
               
            }
            return result;
        }


        public void  worker_DoWork(object sender, DoWorkEventArgs e)
        {
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

            Calc(start, stop, N);
            e.Result = result;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Задача выполнена \n Результат:\t " + e.Result.ToString());
            res.Foreground = Brushes.Green;
            res.Text = e.Result.ToString();

        }

        public Task CalculateAsynk()
        {
            window1.ShowDialog();
            start = Convert.ToInt32(window1.Start.Text);
            stop = Convert.ToInt32(window1.Stop.Text);
            N = Convert.ToInt32(window1.N.Text);

            double h;
            double a = start;
            double b = stop;
            double x = 0;
            
            h = (b - a) / N;
            double sum = 0;
            int max = 100;
            res.Foreground = Brushes.LightGray;
            return Task.Run(() =>
            {
                 for (var i = 1; i <= N; i++)
                 {

                     x = a + i * h;
                     sum += Math.Pow(x, 9); ;

                    if (i % 42 == 0)
                    {
                         int progressPercentage = Convert.ToInt32(((double)i / max) * 1100);
                         Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => pBar.Value += progressPercentage));
                     }
                 }
                 var result = (h * sum);
                 MessageBox.Show("Задача выполнена \n Результат:\t" + result);          
             });
           
        }
    }
}
 

