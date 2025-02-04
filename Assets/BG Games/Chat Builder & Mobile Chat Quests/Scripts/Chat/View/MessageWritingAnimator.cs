using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class MessageWritingAnimator : MonoBehaviour
    {
        [SerializeField] private Transform[] _dots;

        [Header("Animation parameters")] [SerializeField]
        private float _delayBetweenDots = 0.2f;

        [SerializeField] private float _duration = 1.8f;
        [SerializeField] private float _amplitude = 20;
        [SerializeField] private AnimationCurve _animationCurve;
        private float _currentProgress;
        private float _startY;

        private void Awake()
        {
            _startY = _dots[0].localPosition.y;
        }


        public void Enable()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            StartAnimation();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _currentProgress += Time.deltaTime;
            for (int i = 0; i < _dots.Length; i++)
            {
                Vector3 vector3 = _dots[i].localPosition;
                vector3.y = _startY + _animationCurve.Evaluate(_currentProgress - _delayBetweenDots * i) * _amplitude;
                _dots[i].localPosition = vector3;
            }

            if (_currentProgress >= _duration)
            {
                StartAnimation();
            }
        }

        private void StartAnimation()
        {
            _currentProgress = 0;
        }
    }
}