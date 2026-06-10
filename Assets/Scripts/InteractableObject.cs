using UnityEngine;

using InspectionSystem.Data;
using InspectionSystem.Core;

namespace InspectionSystem.Interaction
{
    /// <summary>
    /// Represents an object that can be selected
    /// and inspected by the player.
    /// </summary>
    public class InteractableObject : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField]
        private InspectionObjectData objectData;

        /// <summary>
        /// Public read-only access.
        /// </summary>
        public InspectionObjectData Data => objectData;

        /// <summary>
        /// Called when object is selected.
        /// </summary>
        public void Select()
        {
            if (objectData == null)
            {
                Debug.LogWarning(
                    $"{name} has no data assigned.");

                return;
            }

            GameEvents.ObjectSelected?.Invoke(
                objectData);
        }
    }
}