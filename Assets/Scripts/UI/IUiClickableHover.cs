using UnityEngine;

namespace UI {
    /// <summary>
    /// Interface for UI that can be clicked on and displays as a button with a tooltip
    /// </summary>
    public interface IUiClickableHover {
        /// <summary>
        /// The sprite to display in the UI
        /// </summary>
        /// <returns>The sprite to display in the UI</returns>
        Sprite GetSprite();
    
        /// <summary>
        /// The text to display when the image is hovered over
        /// </summary>
        /// <returns>The text to display when the image is hovered over</returns>
        string GetHoverText();
    
        /// <summary>
        /// Some action to perform when the button is clicked
        /// </summary>
        void OnClick();
    }
}
