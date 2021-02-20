using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Research {
    public class ResearchManager : MonoBehaviour {
        /// <summary>
        /// Private singleton access
        /// </summary>
        private static ResearchManager _instance = null;

        [SerializeField] private List<ResearchObject> allResearch = new List<ResearchObject>();
        
        /// <summary>
        /// Dictionary of ongoing research timers used to track when the research is complete
        /// </summary>
        List<ResearchObject> ongoingResearch = new List<ResearchObject>();
        
        /// <summary>
        /// A list to temporarily storing finished research in, used so we can remove an item from the ongoing without
        /// modifying its contents when it may be iterating over them
        /// </summary>
        List<ResearchObject> tempFinishedResearch = new List<ResearchObject>();
        
        public List<ResearchObject> AllResearch => allResearch;
        public List<ResearchObject> OngoingResearch => ongoingResearch;

        #region DebugMessages
        private void SecondInstance() {
            Debug.LogWarning("Attempted to create 2nd instance of Research Manager");
        }

        private void PrerequisitesNotResearched(string researchName) {
            Debug.LogWarning(
                "Attempting to research " + researchName + " when its prerequisites have not been met");
        }

        private void DebugResearchStarted() {
            Debug.Log("A research task has started");
        }
        #endregion

        public static ResearchManager Instance {
            get {
                if (_instance == null) {
                    GameObject go = Resources.Load<GameObject>(ResourceLoad.ResearchSingleton);
                    Instantiate(go);
                }

                return _instance;
            }
        }

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
            foreach (ResearchObject research in ongoingResearch) {
                research.TickTimer(Time.deltaTime);
            }

            foreach (ResearchObject researchObject in tempFinishedResearch) {
                ongoingResearch.Remove(researchObject);
            }
            tempFinishedResearch.Clear();
        }

        /// <summary>
        /// Begins research on a topic
        /// </summary>
        /// <param name="researchObject">The research object to begin researching</param>
        /// <param name="useResources">Whether the resources should be used to begin the research</param>
        public void ResearchTopic(ResearchObject researchObject, bool useResources = true) {
            if (researchObject.Researched) {
                //Topic already researched
                return;
            }

            if (!researchObject.PrerequisitesMet()) {
                PrerequisitesNotResearched(researchObject.ResearchName);
                return;
            }

            if (!useResources || ResourceManagement.Instance.UseResources(researchObject.Resources)) {
                DebugResearchStarted();
                ongoingResearch.Add(researchObject);
                researchObject.BeginResearch();
                researchObject.OnResearchFinished += () => tempFinishedResearch.Add(researchObject);
            }
        }
    }
}
