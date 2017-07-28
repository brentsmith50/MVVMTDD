using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using FriendStorage.Model;
using System;
using System.Linq;

namespace FriendStorage.UI.ViewModel
{
    public interface INavigationViewModel
    {
        void Load();
    }

    public class NavigationViewModel : BindableBase, INavigationViewModel
    {
        #region Fields
        private ObservableCollection<NavigationItemViewModel> friends;
        private INavigationDataProvider dataProvider;
        private IEventAggregator eventAggregator;
        #endregion

        #region Constructors
        public NavigationViewModel(INavigationDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<FriendSaveEvent>().Subscribe(OnFriendSaved);
        }
        #endregion

        #region Properties
        public ObservableCollection<NavigationItemViewModel> Friends
        {
            get { return this.friends ?? (this.friends = new ObservableCollection<NavigationItemViewModel>()); }
            set { this.SetProperty(ref this.friends, value); }
        }
        #endregion

        #region Methods
        public void Load()
        {
            this.Friends.Clear();
            foreach (var friend in dataProvider.GetAllFriends())
            {
                this.Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, this.eventAggregator));
            }
        }

        private void OnFriendSaved(Friend friendToSave)
        {
            var displayMember = $"{friendToSave.FirstName} {friendToSave.LastName}";
            var navigationItem = this.Friends.Single(f => f.Id == friendToSave.Id);

            if (navigationItem != null)
            {
                navigationItem.DisplayMember = displayMember;
            }
            else
            {
                navigationItem = new NavigationItemViewModel(friendToSave.Id, displayMember, this.eventAggregator);
                this.Friends.Add(navigationItem);
            }
        }
        #endregion

    }   
}
