using System;
using UnityEngine;

namespace Util {
    /// <summary>
    /// Simple timer util, use by creating and starting, then call finished to see if the timer has finished
    /// </summary>
    [Serializable]
    public class Timer {
        /// <summary>
        /// Time that the timer started running
        /// </summary>
        [SerializeField] private float _startTime = 0f;
        
        /// <summary>
        /// How long the timer should run for
        /// </summary>
        [SerializeField] private float _runningTime = 1f;
        
        public float RunningTime {
            set => _runningTime = value;
        }

        public Timer(float runningTime = 1) {
            _runningTime = runningTime;
        }

        public void Start() {
            _startTime = Time.time;
        }

        public bool Finished() {
            return Time.time >= (_runningTime + _startTime);
        }

        /// <summary>
        /// Gets the progress through the timer as a value between 0 and 100
        /// </summary>
        /// <returns>The progress through the timer between 0 and 100</returns>
        public float Progress() {
            float currentRunTime = Time.time - (_startTime);
            float endTime = _startTime + _runningTime;
            return (currentRunTime / endTime) * 100;
        }
    }
}
