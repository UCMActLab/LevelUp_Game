using System;
using System.Collections;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class ChatScrollAnimation : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private float _animationTime;
        [SerializeField] private float _threshold;
        [Space]
        [SerializeField] private ChatMessageSpawner _spawner;

        private Coroutine _animationCoroutine;

        private void Awake()
        {
            _spawner.SpawnedMessage += PlayAnimation;
        }

        private void OnDestroy()
        {
            _spawner.SpawnedMessage -= PlayAnimation;
        }

        [ContextMenu("Play Animation")]
        private void PlayAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            
            _animationCoroutine = StartCoroutine(AnimateScroll());
        }

        private IEnumerator AnimateScroll()
        {
            yield return null;
            
            float elapsedTime = 0;
            float start = _scroll.verticalNormalizedPosition;

            while (elapsedTime < _animationTime)
            {
                float t = EaseOutExpo(elapsedTime / _animationTime);
                
                float current = Mathf.Lerp(start, 0, t);
                _scroll.verticalNormalizedPosition = current;

                elapsedTime += Time.deltaTime;

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    yield break;
                }

                yield return null;
            }

            _scroll.verticalNormalizedPosition = 0;
        }
        
        private float EaseOutExpo(float x)
        {
            return x == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * x);
        }
        
        private float EaseOutCirc(float x) 
        {
            return (float)Math.Sqrt(1 - Math.Pow(x - 1, 2));
        }
    }
}