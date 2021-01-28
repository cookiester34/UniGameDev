using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Research {
    [Serializable]
    [CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Research")]
    public class ResearchObject : ScriptableObject {
        /// <summary>
        /// Name of the research
        /// </summary>
        [SerializeField] private string name;
        
        /// <summary>
        /// A description to explain what the research does
        /// </summary>
        [SerializeField] private string description;
        
        /// <summary>
        /// A cost for the research
        /// </summary>
        [SerializeField] private int cost;
        
        /// <summary>
        /// How long it takes for the research to be completed
        /// </summary>
        [SerializeField] int timeToResearch;

        /// <summary>
        /// Other prerequisite researches that must first be unlocked
        /// </summary>
        [SerializeField] private List<ResearchObject> Prerequisites;

        /// <summary>
        /// Whether this research topic has been researched
        /// </summary>
        [SerializeField] private bool researched;
        
        /// <summary>
        /// An image for the research to show in the Ui
        /// </summary>
        [SerializeField] Image UIImage;

        public string Name => name;
        public int Cost => cost;
        public int TimeToResearch => timeToResearch;
        public string Description => description;

        public bool Researched {
            get => researched;
            set => researched = value;
        }

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
    }
}
