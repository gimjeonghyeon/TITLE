using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Playground.Title
{
    public class LoginView : MonoBehaviour
    {
        #region Private

        [SerializeField] private Button _googleLoginButton;
        [SerializeField] private TextMeshProUGUI _googleLoginButtonText;
        
        #endregion

        // TODO: 추후 제거 필요
        #region Test 
        
        [SerializeField] private TextMeshProUGUI _userDisplayName;
        [SerializeField] private TextMeshProUGUI _userEmail;
        [SerializeField] private Image _userPhoto;

        #endregion

        private void Awake() => SetButtonActive(false);

        public void SetButtonActive(bool active) => _googleLoginButton.gameObject.SetActive(active);

        public IObservable<Unit> OnGoogleLoginButtonClick() => _googleLoginButton.onClick.AsObservable();
        
        public void SetGoogleLoginButtonText(string text) => _googleLoginButtonText.text = text;

        public void SetUserInfo(string userDisplayName, string userEmail)
        {
            _userDisplayName.text = userDisplayName;
            _userEmail.text = userEmail;
        }

        public void SetUserPhoto(Sprite sprite) => _userPhoto.sprite = sprite;
    }

}
