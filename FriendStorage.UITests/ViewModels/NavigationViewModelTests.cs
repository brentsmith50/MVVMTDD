using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FriendStorage.Model;
using Moq;
using Prism.Events;
using FriendStorage.UI.Events;

namespace FriendStorage.UITests.ViewModels
{
    public class NavigationViewModelTests
    {
        private NavigationViewModel navigationViewModel;
        private FriendSaveEvent friendSaveEvent;

        #region Constructors
        public NavigationViewModelTests()
        {
            this.friendSaveEvent = new FriendSaveEvent();

            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(e => e.GetEvent<FriendSaveEvent>()).Returns(this.friendSaveEvent);

            var navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(dp => dp.GetAllFriends())
                .Returns(new List<LookupItem>
                {
                    new LookupItem { Id = 1, DisplayMember = "Micky" },
                    new LookupItem { Id = 2, DisplayMember = "Mouse" }
                });

            navigationViewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object);
        }
        #endregion

        [Fact]
        public void ShouldLoadFriends()
        {
            navigationViewModel.Load();
            Assert.Equal(2, navigationViewModel.Friends.Count);

            var friend = navigationViewModel.Friends.SingleOrDefault(f => f.DisplayMember == "Mouse");
            Assert.NotNull(friend);
            Assert.Equal("Mouse", friend.DisplayMember);
        }

        [Fact]
        public void ShouldLoadFriendsOnce()
        {
            navigationViewModel.Load();
            navigationViewModel.Load();
            Assert.Equal(2, navigationViewModel.Friends.Count);
        }

        [Fact]
        public void ShouldUpdateNavigationItemWhenFriendIsSaved()
        {
            this.navigationViewModel.Load();
            var navigationItem = this.navigationViewModel.Friends.First();

            var friendId = navigationItem.Id;
            this.friendSaveEvent.Publish(
                new Friend
                {
                    Id = friendId,
                    FirstName = "Julia",
                    LastName = "Huber"  
                });

            Assert.Equal("Julia Huber", navigationItem.DisplayMember);
        }

        [Fact]
        public void ShouldAddNavigationItemWhenAddedFriendSaved()
        {
            this.navigationViewModel.Load();

            const int newFriendId = 97;

            this.friendSaveEvent.Publish(new Friend
            {
                Id = newFriendId,
                FirstName = "Anna",
                LastName = "Huber"
            });

            Assert.Equal(3, this.navigationViewModel.Friends.Count);

            //var addedItem = this.navigationViewModel.Friends.SingleOrDefault(n => n.Id == newFriendId);
            //Assert.NotNull(addedItem);
        }
    }
}
