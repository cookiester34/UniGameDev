using UnityEngine;

public class SceneTypeEnable : MonoBehaviour {
    [SerializeField] private bool allowInGameLevel;
    [SerializeField] private bool allowInLevelEditor;

    /// <summary>
    /// Disables the object if it is not allowed according to settings given
    /// </summary>
    public void Awake() {
        switch (CurrentSceneType.SceneType) {
            case SceneType.GameLevel:
                if (!allowInGameLevel) {
                    gameObject.SetActive(false);
                }
                break;
            
            case SceneType.LevelEditor:
                if (!allowInLevelEditor) {
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
