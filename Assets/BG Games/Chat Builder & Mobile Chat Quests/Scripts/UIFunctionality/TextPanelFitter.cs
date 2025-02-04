using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality
{
    public class TextPanelFitter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private ContentSizeFitter _contentSizeFitter;

        private bool _started;

        private void Start()
        {
            _inputField.onValueChanged.AddListener(StartUpdateDelay);
        }

        private void StartUpdateDelay(string arg0)
        {
            StartCoroutine(ChangeLayout());
        }

        private IEnumerator ChangeLayout()
        {
            if (_started)
                yield break;
        
            _started = true;
            _contentSizeFitter.enabled=false;
        
            yield return null;
            _started = false;
            _contentSizeFitter.enabled=true;
        }
    }
}