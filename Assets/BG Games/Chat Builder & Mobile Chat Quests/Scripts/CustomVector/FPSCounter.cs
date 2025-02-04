using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.CustomVector
{
    public class FPSCounter : MonoBehaviour
    {
        // write method that counts frames per second in last 3 seconds and 
        // displays on the screen three values: average, min and max fps 
        // (use GUI.Label to display text)
        private float _fps;
        private float _minFps = float.MaxValue;
        private float _maxFps = float.MinValue;
        private float _sumFps;
        
        private float _time;
        private int _frames;
        
        private void Update()
        {
            _time += Time.deltaTime;
            _frames++;
            if (_time >= 3)
            {
                _fps = _frames / _time;
                _sumFps += _fps;
                _minFps = Mathf.Min(_minFps, _fps);
                _maxFps = Mathf.Max(_maxFps, _fps);
                _time = 0;
                _frames = 0;
            }
        }
        
        private void OnGUI()
        {
            if (_fps > 30)
            {
                GUI.contentColor = Color.green;
            }
            else
            {
                GUI.contentColor = Color.red;
            }
            
            GUI.Label(new Rect(10, 10, 200, 20), $"Average FPS: {_sumFps / 3}");
            GUI.Label(new Rect(10, 30, 200, 20), $"Min FPS: {_minFps}");
            GUI.Label(new Rect(10, 50, 200, 20), $"Max FPS: {_maxFps}");
        }
    }
}