using UnityEngine;

namespace Util {
    /// <summary>
    /// Simple timer util, use by creating and starting, then call finished to see if the timer has finished
    /// </summary>
    public class Timer {
        /// <summary>
        /// Time that the timer started running
        /// </summary>
        private float _startTime = 0f;
        
        /// <summary>
        /// How long the timer should run for
        /// </summary>
        private float _runningTime = 1f;
        
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
    }
}
