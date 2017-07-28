using FriendStorage.Model;
using FriendStorage.UI.ViewModel;
using System;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.WrapperDTO
{
    public class FriendWrapper : ViewModelBase
    {
        #region Fields
        // BAD... this has a reference to an entity
        private Friend friend;
        private bool isChanged;
        #endregion

        #region Constructor
        public FriendWrapper(Friend friend)
        {
            this.friend = friend;
        }
        #endregion

        #region Properties
        public Friend Model
        {
            get { return this.friend; }
        }

        public bool IsChanged
        {
            get { return this.isChanged; }
            set
            {
                this.isChanged = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get { return this.friend.Id; }
        }

        public string FirstName
        {
            get { return this.friend.FirstName; }
            set
            {
                this.friend.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return this.friend.LastName; }
            set
            {
                this.friend.LastName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Birthday
        {
            get { return this.friend.Birthday; }
            set
            {
                this.friend.Birthday = value;
                OnPropertyChanged();
            }
        }

        public bool IsDeveloper
        {
            get { return this.friend.IsDeveloper; }
            set
            {
                this.friend.IsDeveloper = value;
                OnPropertyChanged();
            }
        }


        
        #endregion

        #region Methods
        public void AcceptChanges()
        {
            IsChanged = false;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != nameof(IsChanged))
            {
                IsChanged = true;
            }
        }
        #endregion
    }
}
