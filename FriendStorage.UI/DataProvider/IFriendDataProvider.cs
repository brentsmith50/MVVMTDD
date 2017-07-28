using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider
{
    public interface IFriendDataProvider 
    {
        Friend GetFriendById(int friendId);
        void SaveFriend(Friend friend);
        void DeleteFriend(int friendId);
    }
}
