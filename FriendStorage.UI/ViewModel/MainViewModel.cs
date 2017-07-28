using FriendStorage.DataAccess;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<IFriendEditViewModel> friendEditViewModels;
        private IFriendEditViewModel selectedFriendEditViewModel;
        private Func<IFriendEditViewModel> friendEditVmCreator;
        private DelegateCommand closeFriendTabCommand;
        private DelegateCommand addFriendCommand;
        #endregion

        #region Constructors
        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendEditViewModel> friendEditVmCreator, IEventAggregator eventAggregator)
        {
            this.NavigationViewModel = navigationViewModel;
            this.friendEditVmCreator = friendEditVmCreator;
            //this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
        }

        #endregion

        #region Properties
        public INavigationViewModel NavigationViewModel { get; private set; }

        public ObservableCollection<IFriendEditViewModel> FriendEditViewModels
        {
            get { return this.friendEditViewModels ?? (this.friendEditViewModels = new ObservableCollection<IFriendEditViewModel>()); }
        }

        public IFriendEditViewModel SelectedFriendEditViewModel
        {
            get { return this.selectedFriendEditViewModel; }
            set
            {
                this.selectedFriendEditViewModel = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand CloseFriendTabCommand
        {
            get { return this.closeFriendTabCommand ?? (this.closeFriendTabCommand = new DelegateCommand(OnFriendTabCloseExecute)); }
        }

        public DelegateCommand AddFriendCommand
        {
            get { return this.addFriendCommand ?? (this.addFriendCommand = new DelegateCommand(OnAddFreindExecute)); }
        }
        #endregion

        #region Methods

        public void Load()
        {
            this.NavigationViewModel.Load();
        }
        
        private void OnOpenFriendEditView(int friendId)
        {
            var selectedFriend = this.FriendEditViewModels.SingleOrDefault(v => v.Friend.Id == friendId);
            if (selectedFriend == null)
            {
                selectedFriend = this.CreateAndLoadFriendEditViewModel(friendId);
            }
            this.SelectedFriendEditViewModel = selectedFriend;
        }

        private void OnFriendTabCloseExecute(object selectedTabControl)
        {
            var friendEditViewModel = (IFriendEditViewModel)selectedFriendEditViewModel;
            FriendEditViewModels.Remove(friendEditViewModel);
        }


        private void OnAddFreindExecute(object obj)
        {
            this.SelectedFriendEditViewModel = this.CreateAndLoadFriendEditViewModel(null);
        }

        private IFriendEditViewModel CreateAndLoadFriendEditViewModel(int? friendId)
        {
            var friendEditVm = this.friendEditVmCreator();
            this.FriendEditViewModels.Add(friendEditVm);
            friendEditVm.Load(friendId);
            return friendEditVm;
        }
        #endregion
    }
}
