using Microsoft.Win32;
using StrawberryClient.Command;
using StrawberryClient.Model;
using StrawberryClient.View;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.ViewModel
{
    class SetProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int maxProfileSize = 1024 * 1000;
        SetProfileModel setProfileModel;
        public ICommand resetPictureCommand { get; set; }
        public ICommand findPictureCommand { get; set; }
        public ICommand completeCommand { get; set; }


        public string path
        {
            get { return setProfileModel.Path; }
            set { setProfileModel.Path = value; }
        }

        public string userId
        {
            get { return setProfileModel.UserId; }
            set { setProfileModel.UserId = value; }
        }

        public ImageSource profileImage
        {
            get { return setProfileModel.ProfileImage; }
            set
            {
                setProfileModel.ProfileImage = value;
                OnPropertyUpdate("profileImage");
            }
        }

        public SetProfileViewModel()
        {
            setProfileModel = new SetProfileModel();
            findPictureCommand = new RelayCommand(findPictureExecuteMethod);
            completeCommand = new RelayCommand(completeExecuteMethod);
            resetPictureCommand = new RelayCommand(resetPictureExecuteMethod);
        }

        // 기본 프로필 사진으로 변경
        private void resetPictureExecuteMethod(object obj)
        {
            setProfileModel.setDefault();
            (obj as Window).Close();
        }

        // 설정할 프로필 사진 검색
        private void findPictureExecuteMethod(object obj)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Images Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png";

            if (open.ShowDialog() == true)
            {
                path = open.FileName;
                FileInfo info = new FileInfo(path);

                if (info.Length > maxProfileSize)
                {
                    MessageBox.Show("용랑이 큽니다. 줄여오세요.");
                    return;
                }

                ImageSourceConverter c = new ImageSourceConverter();
                profileImage = (ImageSource)c.ConvertFromString(path);
            }
        }

        // 설정 완료
        private void completeExecuteMethod(object obj)
        {
            setProfileModel.complete();
            (obj as Window).Close();
        }

        public void Init(string userId, ImageSource userImage)
        {
            this.userId = userId;
            this.profileImage = userImage;

            SetProfileView setProfile = new SetProfileView()
            {
                DataContext = this,
            };
            setProfile.Show();
        }

        private void OnPropertyUpdate(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
