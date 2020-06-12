using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StrawberryClient.View
{
    /// <summary>
    /// AlarmView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmView : Window
    {
        int count = 0;

        public AlarmView(string userName, string content)
        {
            InitializeComponent();
            this.userName.Text = userName;
            this.content.Text = content;
            this.Loaded += AlarmView_Loaded;

            DispatcherTimer timer = new DispatcherTimer();


            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Wait);
            timer.Start();

        }

        private void AlarmView_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = workingArea.Right - this.Width;
            this.Top = workingArea.Bottom - this.Height;
        }

        private void timer_Wait(object sender, EventArgs e)
        {
            count++;

            if (count >= 2)
            {
                DispatcherTimer timer = new DispatcherTimer();

                timer.Interval = TimeSpan.FromSeconds(0.01);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
        }



        private void timer_Tick(object seder, EventArgs e)
        {
            this.Opacity -= 0.01;

            if (this.Opacity <= 0)
            {
                this.Close();
            }
        }


        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
