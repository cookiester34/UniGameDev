using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Research {
    [Serializable]
    [CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Research")]
    public class ResearchObject : ScriptableObject, IUiClickableHover {
        public delegate void ResearchedDelegate();
        public event ResearchedDelegate OnResearchFinished;
        public event ResearchedDelegate OnResearchStarted;
        /// <summary>
        /// Name of the research
        /// </summary>
        [SerializeField] private string researchName;
        
        /// <summary>
        /// A description to explain what the research does
        /// </summary>
        [TextArea] [SerializeField] private string description;

        [SerializeField] private List<ResourcePurchase> resources;

        /// <summary>
        /// Other prerequisite researches that must first be unlocked
        /// </summary>
        [SerializeField] private List<ResearchObject> Prerequisites;

        /// <summary>
        /// Whether this research topic has been researched
        /// </summary>
        [SerializeField] private bool researched;

        [SerializeField] private float researchProgress;
        [SerializeField] private Timer timer;

        /// <summary>
        /// An image for the research to show in the Ui
        /// </summary>
        [SerializeField] private Sprite uiSprite;

        public string ResearchName => researchName;
        public List<ResourcePurchase> Resources => resources;
        public string Description => description;

        public bool Researched => researched;

        public float ResearchProgress => researchProgress;
        public Timer Timer => timer;

        private void OnEnable() {
            researched = false;
            timer.OnTimerFinish += FinishResearch;
            timer.Reset();
        }

        public void CopySavedResearch(SavedResearch savedResearch) {
            researched = savedResearch.researched;
            researchProgress = savedResearch.progress;
            timer.LoadTimer(savedResearch.timer);
            if (researched) {
                OnResearchFinished?.Invoke();
            }
        }

        public void BeginResearch() {
            timer.Start();
            OnResearchStarted?.Invoke();
        }

        public void TickTimer(float timePassed) {
            timer.Tick(timePassed);
            researchProgress = timer.ProgressPercent;
        }

        public void FinishResearch() {
            researched = true;
            OnResearchFinished?.Invoke();
			Debug.Log("The research: " + researchName + " has been completed");
			UIEventAnnounceManager.Instance.AnnounceEvent("The research: " + researchName + " has been completed");
        }

        /// <summary>
        /// Determines if the previous research has been unlocked such that this one can
        /// </summary>
        /// <returns>True if previous required researches have been unlocked, else false</returns>
        public bool PrerequisitesMet() {
            bool met = true;

            if (Prerequisites != null) {
                foreach (ResearchObject prerequisite in Prerequisites) {
                    if (!prerequisite.researched) {
                        met = false;
                        break;
                    }
                }
            }

            return met;
        }


        public Sprite GetSprite() {
            return uiSprite;
        }

        public string GetHoverText() {
            string hoverText = researchName + "\n";
            foreach (ResourcePurchase purchase in resources) {
                hoverText = hoverText + purchase.resourceType + ": " + purchase.cost + " ";
            }
            if (hoverText != "\n") {
                return hoverText;
            }
            return "\nNo cost";
        }

        public void OnClick() {
            ResearchManager.Instance.ResearchTopic(this);
        }
    }
}
