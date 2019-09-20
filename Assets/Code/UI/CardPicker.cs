using UnityEngine;

namespace AceTheChase.UI
{
    /// <summary>
    /// A popover window that lets the user choose one card from a provided list.
    /// </summary>
    public class CardPicker : MonoBehaviour
    {
        /// <summary>
        /// The GridLayout UI object that will display the list of cards.
        /// </summary>
        public GameObject CardGrid;

        public UIManager UIManager;

        public void Clear()
        {
            foreach(Transform child in this.CardGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void Cancel()
        {
            this.UIManager.CancelCardPicker();
        }

        public void NoTarget()
        {
            this.UIManager.CardPickerNoTargetSelected();
        }
    }
}