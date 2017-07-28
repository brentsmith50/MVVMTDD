using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FriendStorage.UITests.ViewModels
{
    public class FriendEditViewModelTests
    {
        #region Fields
        private const int friendId = 5;
        private Mock<IFriendDataProvider> dataProviderMock;
        private FriendEditViewModel viewModel;
        private Mock<FriendSaveEvent> friendSavedEventMock;
        private Mock<IEventAggregator> eventAggregatorMock;
        #endregion


        #region Constructor
        public FriendEditViewModelTests()
        {
            this.friendSavedEventMock = new Mock<FriendSaveEvent>();
            this.eventAggregatorMock = new Mock<IEventAggregator>();

            this.eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSaveEvent>()).Returns(this.friendSavedEventMock.Object);

            this.dataProviderMock = new Mock<IFriendDataProvider>();

            this.dataProviderMock.Setup(dp => dp.GetFriendById(friendId))
                                 .Returns(new Friend { Id = friendId, FirstName = "Thomas" });

            this.viewModel = new FriendEditViewModel(this.dataProviderMock.Object, this.eventAggregatorMock.Object);
        }
        #endregion

        #region Test Methods
        [Fact]
        public void ShouldLoadFriend()
        {
            this.viewModel.Load(friendId);

            Assert.NotNull(this.viewModel.Friend);
            Assert.Equal(friendId, this.viewModel.Friend.Id);

            this.dataProviderMock.Verify(dp => dp.GetFriendById(friendId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForFriend()
        {
            var fired = this.viewModel.IsPropertyChangedFired(() => 
                this.viewModel.Load(friendId),
                nameof(viewModel.Friend));

            Assert.True(fired);
        }

        [Fact]
        public void ShouldDisableSaveCommandWhenFriendIsLoaded()
        {
            this.viewModel.Load(friendId);

            Assert.False(this.viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldEnableFriendWhenFriendIsChanged()
        {
            this.viewModel.Load(friendId);
            this.viewModel.Friend.FirstName = "Changed";

            Assert.True(this.viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableSaveCommandWithoutLoad()
        {
            Assert.False(this.viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandWhenFriendIsChanged()
        {
            // Failing
            this.viewModel.Load(friendId);
            var fired = false;
            this.viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            this.viewModel.Friend.FirstName = "Changed";

            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
        {
            var fired = false;
            this.viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            this.viewModel.Load(friendId);

            Assert.True(fired);
        }

        [Fact]
        public void ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted()
        {
            this.viewModel.Load(friendId);
            this.viewModel.Friend.FirstName = "Changed";
            this.viewModel.SaveCommand.Execute(null);

            this.dataProviderMock.Verify(dp => dp.SaveFriend(this.viewModel.Friend.Model), Times.Once);
        }

        [Fact]
        public void ShouldAcceptChangesWhenSaveCommandIsExecuted()
        {
            this.viewModel.Load(friendId);
            this.viewModel.Friend.FirstName = "Changed";
            this.viewModel.SaveCommand.Execute(null);

            Assert.False(this.viewModel.Friend.IsChanged);
        }
        
        [Fact]
        public void ShouldPublishFriendSavedEventWhenSaveCommandIsExecuted()
        {
            this.viewModel.Load(friendId);
            this.viewModel.Friend.FirstName = "Changed";
            this.viewModel.SaveCommand.Execute(null);

            this.friendSavedEventMock.Verify(e => e.Publish(this.viewModel.Friend.Model), Times.Once);
        }

        [Fact]
        public void ShouldCreateNewFriendWhenNullIsPassedToLoadMethod()
        {
            this.viewModel.Load(null);

            Assert.NotNull(this.viewModel.Friend);
            Assert.Equal(0, this.viewModel.Friend.Id);
            Assert.Null(this.viewModel.Friend.FirstName); 
            Assert.Null(this.viewModel.Friend.LastName);
            Assert.Null(this.viewModel.Friend.LastName);
            Assert.False(this.viewModel.Friend.IsDeveloper);

            this.dataProviderMock.Verify(vm => vm.GetFriendById(It.IsAny<int>()), Times.Never);
        }
        #endregion
    }
}
