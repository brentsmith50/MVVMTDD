using FriendStorage.UI.Command;
using FriendStorage.UI.Events;
using Prism.Events;
using Prism.Mvvm;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationItemViewModel : BindableBase
    {
        #region Fields
        private IEventAggregator eventAggregator;
        private DelegateCommand openFriendEditViewCommand;
        private string displayMember;
        #endregion

        #region Constructors
        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            this.eventAggregator = eventAggregator;
        }
        #endregion

        #region Properties
        public string DisplayMember
        {
            get { return this.displayMember; }
            set
            {
                this.displayMember = value;
                this.OnPropertyChanged();
            }
        }

        public int Id { get; private set; }

        public DelegateCommand OpenFriendEditViewCommand
        {
            get { return this.openFriendEditViewCommand ?? (this.openFriendEditViewCommand = new DelegateCommand(OnFriendEditViewExecute)); }
        }
        #endregion

        #region Methods
        private void OnFriendEditViewExecute(object obj)
        {
            this.eventAggregator.GetEvent<OpenFriendEditViewEvent>().Publish(this.Id);
        }
        #endregion
    }
}
