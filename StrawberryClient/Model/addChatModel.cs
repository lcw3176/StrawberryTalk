using StrawberryClient.Model.ObservableCollection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StrawberryClient.Model
{
    class AddChatModel
    {
        private ObservableCollection<Friends> addChatList = new ObservableCollection<Friends>();
        private List<string> checkList = new List<string>();

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
