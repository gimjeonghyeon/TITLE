using TMPro;
using UnityEngine;

namespace Title
{
    public class TitleView : MonoBehaviour
    {
        #region Private
        [SerializeField] private TextMeshProUGUI _progressText;
        #endregion
        
        public void UpdateProgressText(string text) => _progressText.text = text;
    }
}
