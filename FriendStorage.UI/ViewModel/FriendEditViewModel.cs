using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.WrapperDTO;
using Prism.Events;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int? friendId);
        FriendWrapper Friend { get; }
    }

    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        #region Fields
        private IFriendDataProvider dataProvider;
        private FriendWrapper friend;
        private IEventAggregator eventAggregator;
        #endregion

        #region Constructor
        public FriendEditViewModel(IFriendDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            this.SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }
        #endregion

        #region Properties
        public FriendWrapper Friend
        {
            get { return this.friend; }
            set
            {
                this.friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; private set; }
        #endregion

        #region Methods

        public void Load(int? friendId)
        {
            var friend = friendId.HasValue
                ? this.dataProvider.GetFriendById(friendId.Value)
                : new Friend();

            this.Friend = new FriendWrapper(friend);

            ((DelegateCommand)this.SaveCommand).RaiseCanExecuteChanged();
            Friend.PropertyChanged += Friend_PropertyChanged;
        }

        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnSaveCanExecute(object arg)
        {
            return Friend != null && Friend.IsChanged;
        }

        private void OnSaveExecute(object obj)
        {
            this.dataProvider.SaveFriend(this.Friend.Model);
            Friend.AcceptChanges();
            this.eventAggregator.GetEvent<FriendSaveEvent>().Publish(this.Friend.Model);
        }
        #endregion
    }
}
