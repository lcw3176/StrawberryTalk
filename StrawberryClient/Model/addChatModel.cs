using StrawberryClient.Model.ObservableCollection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StrawberryClient.Model
{
    class AddChatModel
    {
        private ObservableCollection<Friends> addChatList = new ObservableCollection<Friends>();
        private List<string> checkList = new List<string>();
        private List<string> userList = new List<string>();

        public List<string> UserLIst
        {
            get { return userList; }
            set { userList = value; }
        }

        public ObservableCollection<Friends> AddChatList
        {
            get{ return addChatList; }
            set { addChatList = value; }
        }

        public List<string> CheckList
        {
            get { return checkList; }
            set { checkList = value; }
        }
        
    }
}
