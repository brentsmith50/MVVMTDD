using System;
using System.ComponentModel;

namespace FriendStorage.UITests
{
    public static class NotifyPropertyChangedExtension
    {
        public static bool IsPropertyChangedFired(this INotifyPropertyChanged notiifyPropertyChanged, Action action, string propertyName)
        {
            var fired = false;

            notiifyPropertyChanged.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    fired = true;
                }
            };

            action();
            return fired;
        }
    }
}
