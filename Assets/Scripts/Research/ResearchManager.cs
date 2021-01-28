using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Research {
    public class ResearchManager : MonoBehaviour {
        /// <summary>
        /// Delegate for the event to fire when research is completed
        /// </summary>
        public delegate void OnResearchComplete();

        /// <summary>
        /// Event to use when if something needs to update when a research is completed, hook into as such
        /// ResearchManager.Instance.researchCompleted += MyFunc; 
        /// </summary>
        public event OnResearchComplete researchCompleted;
        
        /// <summary>
        /// Private singleton access
        /// </summary>
        private ResearchManager _instance;
        
        /// <summary>
        /// Access to the singleton
        /// </summary>
        public ResearchManager Instance => _instance;
        
        /// <summary>
        /// List of all completed researches, can be queried against to see what options should be available
        /// </summary>
        List<ResearchObject> completedResearch = new List<ResearchObject>();
        
        /// <summary>
        /// Dictionary of ongoing research timers used to track when the research is complete
        /// </summary>
        Dictionary<ResearchObject, Timer> ongoingResearch = new Dictionary<ResearchObject, Timer>();

        public List<ResearchObject> CompletedResearch => completedResearch;
        public Dictionary<ResearchObject, Timer> OngoingResearch => ongoingResearch;

        #region DebugMessages
        private void SecondInstance() {
            Debug.LogWarning("Attempted to create 2nd instance of Research Manager");
        }
        #endregion

        /// <summary>
        /// Handles singleton access
        /// </summary>
        private void Awake() {
            if (_instance != null) {
                SecondInstance();
                Destroy(this);
                return;
            }

            _instance = this;
        }

        private void Update() {
            RunTimers();
        }

        /// <summary>
        /// Checks all currently running timers to see if they are complete, when a timer finishes the researchCompleted
        /// event will be fired
        /// </summary>
        private void RunTimers() {
            ResearchObject finishedResearch = null;
            foreach (KeyValuePair<ResearchObject,Timer> research in ongoingResearch) {
                Timer timer = research.Value;
                if (timer.Finished()) {
                    finishedResearch = research.Key;
                    completedResearch.Add(research.Key);
                    researchCompleted?.Invoke();
                    Debug.Log("Research completed!");
                    break;
                }
            }

            if (finishedResearch != null) {
                ongoingResearch.Remove(finishedResearch);
            }
        }

        /// <summary>
        /// Begins research on a topic
        /// </summary>
        /// <param name="researchObject">The research object to begin researching</param>
        public void ResearchTopic(ResearchObject researchObject) {
            ResourceManagement.Instance.UpdateResourceCurrentAmount("pollen", -researchObject.Cost);
            Timer timer = new Timer(researchObject.TimeToResearch);
            ongoingResearch.Add(researchObject, timer);
            timer.Start();
        }
    }
}
