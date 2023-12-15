using System;
using Firebase.Auth;
using UnityEngine;

namespace Playground
{
    public class UserInfoManager
    {
        #region Public

        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public Uri PhotoUrl { get; private set; }
        
        #endregion

        public void SetUserInfo(FirebaseUser user)
        {
            DisplayName = user.DisplayName;
            Email = user.Email;
            PhotoUrl = user.PhotoUrl;
            
            Debug.Log($"display name: {DisplayName}");
            Debug.Log($"email : {Email}");
            Debug.Log($"photo url: {PhotoUrl}");
        }
    }
}