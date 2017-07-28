using FriendStorage.UI.ViewModel;
using Xunit;
using Moq;
using System;
using Prism.Events;
using FriendStorage.UI.Events;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.WrapperDTO;

namespace FriendStorage.UITests.ViewModels
{
    public class MainViewModelTests
    {
        #region Fields
        private MainViewModel mainViewModel;
        private Mock<INavigationViewModel> navViewModelMock;
        private Mock<IEventAggregator> eventAggregatorMock;
        private OpenFriendEditViewEvent openFriendEditViewEvent;
        private List<Mock<IFriendEditViewModel>> friendEditViewModelMocks;
        #endregion

        #region Constructors
        public MainViewModelTests()
        {
            this.friendEditViewModelMocks = new List<Mock<IFriendEditViewModel>>();
            openFriendEditViewEvent = new OpenFriendEditViewEvent();
            navViewModelMock = new Mock<INavigationViewModel>();
            eventAggregatorMock = new Mock<IEventAggregator>();

            eventAggregatorMock.Setup(e => e.GetEvent<OpenFriendEditViewEvent>())
                .Returns(openFriendEditViewEvent);

            mainViewModel = new MainViewModel(navViewModelMock.Object, CreateFriendEditViewModel,
                                                eventAggregatorMock.Object);
        }
        #endregion

        #region Standard Methods
        private IFriendEditViewModel CreateFriendEditViewModel()
        {
            var friendEditViewModelMock = new Mock<IFriendEditViewModel>();

            friendEditViewModelMock.Setup(vm => vm.Load(It.IsAny<int>())).Callback<int?>(friendId =>
            {
                friendEditViewModelMock.Setup(vm => vm.Friend)
                                       .Returns( new FriendWrapper(new Friend { Id = friendId.Value }));
            });


            this.friendEditViewModelMocks.Add(friendEditViewModelMock);
            return friendEditViewModelMock.Object;
        }
        #endregion

        #region Test Methods
        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            mainViewModel.Load();

            navViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelAndLoadAndSelectIt()
        {
            const int friendId = 7;
            this.openFriendEditViewEvent.Publish(friendId);

            Assert.Equal(1, this.mainViewModel.FriendEditViewModels.Count);

            var friendEditVM = this.mainViewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVM, this.mainViewModel.SelectedFriendEditViewModel);

            this.friendEditViewModelMocks.First().Verify(vm => vm.Load(friendId), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelWithNullLoadAndSelectIt()
        {
            this.mainViewModel.AddFriendCommand.Execute(null);

            Assert.Equal(1, this.mainViewModel.FriendEditViewModels.Count);

            var friendEditVM = this.mainViewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVM, this.mainViewModel.SelectedFriendEditViewModel);

            this.friendEditViewModelMocks.First().Verify(vm => vm.Load(null), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelOnlyOnce()
        {
            this.openFriendEditViewEvent.Publish(5);
            this.openFriendEditViewEvent.Publish(5);
            this.openFriendEditViewEvent.Publish(6);
            this.openFriendEditViewEvent.Publish(7);
            this.openFriendEditViewEvent.Publish(7);
            Assert.Equal(3, this.mainViewModel.FriendEditViewModels.Count);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedFriendEditViewModel()
        {
            var friendEditVMMock = new Mock<IFriendEditViewModel>();
            var fired = this.mainViewModel.IsPropertyChangedFired(() =>
            {
                this.mainViewModel.SelectedFriendEditViewModel = friendEditVMMock.Object;
            }, nameof(this.mainViewModel.SelectedFriendEditViewModel));

            Assert.True(fired);
        }

        [Fact]
        public void ShouldRemoveFriendEditViewModelOnCloseFriendEditTabCommand()
        {
            openFriendEditViewEvent.Publish(7);

            var friendEditVM = this.mainViewModel.SelectedFriendEditViewModel;
            this.mainViewModel.CloseFriendTabCommand.Execute(friendEditVM);

            Assert.Equal(0, this.mainViewModel.FriendEditViewModels.Count);
        }
        #endregion
    }
}
