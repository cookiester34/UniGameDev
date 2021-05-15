using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class UiHoverable : MonoBehaviour {
        /// <summary>
        /// Image component
        /// </summary>
        private Image _image;

        /// <summary>
        /// Button component
        /// </summary>
        protected Button _button;

        [SerializeField] private TooltipEnabler _tooltipEnabler;

        [Tooltip("The scriptable object must implement IUiClickableHover, used to call its methods")]
        [SerializeField] protected ScriptableObject _scriptableObject;
		public bool isLevelEditorOnly;
        private IUiClickableHover _clickableHover;
		private BuildingData _buildingData;

        protected virtual void Awake() {
            Setup();
            Debug.Log(CurrentSceneType.SceneType);
            if (isLevelEditorOnly && CurrentSceneType.SceneType != SceneType.LevelEditor) {
				gameObject.SetActive(false);
			}
        }

        private void OnValidate() {
            Setup();
        }

        /// <summary>
        /// Sets up required components, additionally reads data from the research object to set up image, text etc.
        /// </summary>
        private void Setup() {
            if (_image == null) {
                _image = GetComponent<Image>();
            }

            if (_button == null) {
                _button = GetComponent<Button>();
            }

            if (_tooltipEnabler == null) {
                _tooltipEnabler = GetComponent<TooltipEnabler>();
                if (_tooltipEnabler == null) {
                    Debug.LogError(
                        "Something has gone wrong in setting up the button components, ensure there is a child with text component");
                }
            }

            if (_scriptableObject == null) {
                Debug.LogWarning(
                    "The scriptable object is null, nothing to use to setup the ui from: " + gameObject.name);
                return;
            }

            if (!(_scriptableObject is IUiClickableHover)) {
                Debug.LogError(
                    "An incompatible scriptable is being used, the scriptable must implement IUiClickableHover "
                    + gameObject.name);
            } else {
                _clickableHover = (IUiClickableHover) _scriptableObject;
                _image.sprite = _clickableHover.GetSprite();
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(_clickableHover.OnClick);
				if (_scriptableObject is BuildingData) {
					_buildingData = (BuildingData) _scriptableObject;
                    _tooltipEnabler.TooltipText = _clickableHover.GetHoverText() + CalculateCostString();
                }
				else {
                    _tooltipEnabler.TooltipText = _clickableHover.GetHoverText();
				}
                
            }
        }

        void Update()
        {
            if (_buildingData)
            {
                _button.interactable = GetIsInBuildingLimit() && ResourceManagement.Instance.CanUseResources(_buildingData.Tier1Cost);
            }
        }
		
		private string CalculateCostString() {
			string builder = "\n";
			foreach (ResourcePurchase cost in _buildingData.Tier1Cost) {
				builder = builder + cost.resourceType.ToString() + ": " + cost.cost.ToString() + " ";
			}
			if (builder != "\n") {
				return builder;
			}
			return "\nNo cost";
		}

        private bool GetIsInBuildingLimit()
        {
            int buildingTypeIndex = (int)_buildingData.BuildingType;
            return _buildingData.MaxInstances > BuildingManager.Instance.numBuildingTypes[buildingTypeIndex];
        }
    }
}
