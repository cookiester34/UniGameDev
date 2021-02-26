using System;
using UnityEngine;

namespace Util {
    /// <summary>
    /// Simple timer util, use by creating and starting, then call finished to see if the timer has finished
    /// </summary>
    [Serializable]
    public class Timer {
        public delegate void TimerFinsihed();
        public event TimerFinsihed OnTimerFinish;

        /// <summary>
        /// How long the timer should run for
        /// </summary>
        [SerializeField] private float runTime;
        
        /// <summary>
        /// How long the timer has been running for
        /// </summary>
        [SerializeField] private float timeRunning;
        
        /// <summary>
        /// Percentage through the timer
        /// </summary>
        [SerializeField] private float progressPercent;
        
        /// <summary>
        ///  Whether the timer is currently active
        /// </summary>
        [SerializeField] private bool active;

        public bool Active => active;
        public float ProgressPercent => progressPercent;

        public Timer(float runTime = 1) {
            this.runTime = runTime;
        }

        /// <summary>
        /// Loads another timer into this one maintaining the listeners on this timer
        /// </summary>
        /// <param name="otherTimer">THe other timer to load its progress etc from, does not load its listeners</param>
        public void LoadTimer(Timer otherTimer) {
            runTime = otherTimer.runTime;
            progressPercent = otherTimer.progressPercent;
            active = otherTimer.active;
            timeRunning = otherTimer.timeRunning;
        }

        public void Tick(float timeTick) {
            if (active) {
                timeRunning += timeTick;
                progressPercent = (timeRunning / runTime) * 100;
                if (progressPercent > 99f) {
                    active = false;
                    OnTimerFinish?.Invoke();
                }
            }
        }

        public void Reset(bool setactive = false) {
            timeRunning = 0;
            progressPercent = 0;
            if (setactive) {
                active = true;
            }
        }

        public void Start() {
            active = true;
        }
    }
}
