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
        Dictionary<ResearchObject, Timer> ongoingResearch = new Dictionary<ResearchObject, Timer>();
        
        public List<ResearchObject> AllResearch => allResearch;
        public Dictionary<ResearchObject, Timer> OngoingResearch => ongoingResearch;

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
            ResearchObject finishedResearch = null;
            foreach (KeyValuePair<ResearchObject,Timer> research in ongoingResearch) {
                Timer timer = research.Value;
                if (timer.Finished()) {
                    finishedResearch = research.Key;
                    research.Key.Research();
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
            if (researchObject.Researched) {
                //Topic already researched
                return;
            }

            if (!researchObject.PrerequisitesMet()) {
                PrerequisitesNotResearched(researchObject.ResearchName);
                return;
            }

            if (ResourceManagement.Instance.UseResources(researchObject.Resources)) {
                DebugResearchStarted();
                Timer timer = new Timer(researchObject.TimeToResearch);
                ongoingResearch.Add(researchObject, timer);
                researchObject.BeginResearch();
                timer.Start();
            }
        }
    }
}
