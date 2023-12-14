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

        private void Awake() => SetButtonActive(false);

        public void SetButtonActive(bool active) => _googleLoginButton.gameObject.SetActive(active);

        public IObservable<Unit> OnGoogleLoginButtonClick() => _googleLoginButton.onClick.AsObservable();
        
        public void SetGoogleLoginButtonText(string text) => _googleLoginButtonText.text = text;

    }

}
