using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StrawberryClient.View
{
    /// <summary>
    /// ChatRoomView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatRoomView : Window
    {

        public delegate void scrollEnd(ScrollViewer scrollViewer);
        public event scrollEnd endOfScroll;
        private int height = 1100;

        public ChatRoomView()
        {
            InitializeComponent();
            scrollView.ScrollToEnd();
            scrollView.ScrollChanged += ScrollView_ScrollChanged;
            this.Closed += ChatRoomView_Closed;
        }

        private void ChatRoomView_Closed(object sender, System.EventArgs e)
        {
            scrollView.ScrollChanged -= ScrollView_ScrollChanged;
        }

        // 스크롤 끝 감지
        // 메세지 추가 요청
        private void ScrollView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = (ScrollViewer)sender;

            if (scroll.VerticalOffset == 0 && scroll.ScrollableHeight - scroll.VerticalOffset >= height)
            {
                endOfScroll(scroll);
            }
        }

        private void chatWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
