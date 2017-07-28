using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorage.UITests.ViewModels
{
    public class NavigationItemViewModelTests
    {
        private const int friendId = 7;
        private Mock<IEventAggregator> eventAggregatorMock;
        private NavigationItemViewModel navigationItemViewModel;

        #region Constructor
        public NavigationItemViewModelTests()
        {
            this.eventAggregatorMock = new Mock<IEventAggregator>();
            this.navigationItemViewModel = new NavigationItemViewModel(friendId, "Thomas", this.eventAggregatorMock.Object);
        }
        #endregion

        #region Test Methods
        [Fact]
        public void ShouldPublishOpenFriendEditViewEvent()
        {
            var eventMock = new Mock<OpenFriendEditViewEvent>();
            this.eventAggregatorMock.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>()).Returns(eventMock.Object);

            this.navigationItemViewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(e => e.Publish(friendId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForDisplayMember()
        {
            var fired = this.navigationItemViewModel.IsPropertyChangedFired(() =>
            {
                this.navigationItemViewModel.DisplayMember = "Changed";
            }, nameof(this.navigationItemViewModel.DisplayMember));

            Assert.True(fired);
        }
        #endregion
    }
}
