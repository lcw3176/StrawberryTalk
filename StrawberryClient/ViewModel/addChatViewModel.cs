using StrawberryClient.Command;
using StrawberryClient.Model;
using StrawberryClient.Model.ObservableCollection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StrawberryClient.ViewModel
{
    class AddChatViewModel : INotifyPropertyChanged
    {
        AddChatModel addChatModel;
        public ICommand checkCommand { get; set; }
        public ICommand completeCommand { get; set; }
        List<string> userList = new List<string>();
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void Close(string userName);
        public event Close onClose;


        public ObservableCollection<Friends> addChatList
        {
            get { return addChatModel.AddChatList; }
            set
            {
                addChatModel.AddChatList = value;

                for(int i = 0;i < addChatModel.AddChatList.Count; i++)
                {
                    addChatModel.AddChatList[i].checkCommand = this.checkCommand;
                }

                OnPropertyUpdate("addChatList");
            }
        }

        public AddChatViewModel()
        {
            addChatModel = new AddChatModel();
            checkCommand = new RelayCommand(checkExecuteMethod);
            completeCommand = new RelayCommand(completeExecuteMethod);

        }


        private void checkExecuteMethod(object obj)
        {
            var id = userList.Find(e => e == (obj as TextBlock).Text);

            if (string.IsNullOrEmpty(id))
            { 
                userList.Add((obj as TextBlock).Text);
            }

            else
            {
                userList.Remove((obj as TextBlock).Text);
            }
            
        }

        private void completeExecuteMethod(object obj)
        {
            onClose(string.Join("@", userList));
            (obj as Window).Close();
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
