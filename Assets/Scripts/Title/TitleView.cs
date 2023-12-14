using TMPro;
using UnityEngine;

namespace Playground.Title
{
    public class TitleView : MonoBehaviour
    {
        #region Private
        
        [SerializeField] private TextMeshProUGUI _progressText;
        
        #endregion
        
        public void UpdateProgressText(string text) => _progressText.text = text;
    }
}
