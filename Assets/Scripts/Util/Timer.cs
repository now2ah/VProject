using UnityEngine;
using System;

namespace VProject.Utils
{
    public class Timer : MonoBehaviour
    {
        private bool _isRunning;
        private float _endTime;
        private float _elapsedTime;
        private int _elapsedTick;
        private int _leftTimeTick;

        public float ElapsedTime => _elapsedTime;
        public int LeftTimeTick => _leftTimeTick;

        public event Action<int> OnTick;
        public event Action OnTimerEnded;

        private void Awake()
        {
            _isRunning = false;
            _endTime = 0;
            _elapsedTime = 0;
            _elapsedTick = 1;
            _leftTimeTick = 0;
        }

        public void StartTimer(float endTime)
        {
            _endTime = endTime;
            _leftTimeTick = (int)endTime - 1;
            _isRunning = true;
        }


        void Update()
        {
            if (_isRunning)
            {
                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= _elapsedTick)
                {
                    OnTick?.Invoke(_elapsedTick);
                    _elapsedTick++;
                    _leftTimeTick--;
                }

                if (_elapsedTime >= _endTime)
                {
                    _isRunning = false;
                    _elapsedTime = 0;
                    _elapsedTick = 1;
                    _leftTimeTick = 0;
                    OnTimerEnded?.Invoke();
                }
            }
        }
    }
}