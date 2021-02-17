using System;
using TMPro;
using UnityEngine;

namespace Client.UI
{
    public class UITimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelTimer;
        
        public Action TimeIsUp { get; set; }

        private bool _timerIsRunning;
        private float _timeRemaining;
        
        private void UpdateTimer(int currentTime)
        {
            _levelTimer.SetText(currentTime.ToString());
        }
        
        public void StartTimer(int timeRemaining)
        {
            _timeRemaining = timeRemaining;
            _timerIsRunning = true;
        }
        
        private void Update()
        {
            if (!_timerIsRunning) return;
            
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0;
                _timerIsRunning = false;
                
                TimeIsUp?.Invoke();
            }
                
            UpdateTimer((int) _timeRemaining);
        }

        private void OnDestroy()
        {
            TimeIsUp = null;
        }
    }
}