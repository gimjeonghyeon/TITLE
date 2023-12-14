using System;
using Firebase.Auth;

namespace Playground
{
    public class UserInfoManager
    {
        #region Public

        public string UserId { get; private set; }
        public string DisplayName { get; private set; }
        public Uri PhotoUrl { get; private set; }
        
        #endregion

        public void SetUserInfo(FirebaseUser user)
        {
            UserId = user.UserId;
            DisplayName = user.DisplayName;
            PhotoUrl = user.PhotoUrl;
        }
    }
}