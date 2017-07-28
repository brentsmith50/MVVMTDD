using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendStorage.Model;
using FriendStorage.DataAccess;

namespace FriendStorage.UI.DataProvider
{
    public class FriendDataProvider : IFriendDataProvider
    {
        private readonly Func<IDataService> dataServiceCreator;

        public FriendDataProvider(Func<IDataService> dataServiceCreator)
        {
            this.dataServiceCreator = dataServiceCreator;
        }

        public Friend GetFriendById(int friendId)
        {
            using (var dataService = this.dataServiceCreator())
            {
                return dataService.GetFriendById(friendId);
            }
        }

        public void SaveFriend(Friend friend)
        {
            using (var dataService = this.dataServiceCreator())
            {
                dataService.SaveFriend(friend);
            }
        }

        public void DeleteFriend(int friendId)
        {
            using (var dataService = this.dataServiceCreator())
            {
                dataService.DeleteFriend(friendId);
            }
        }
    }
}
