using UnityEngine;
using InspectionSystem.Data;

namespace InspectionSystem.Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField]
        private InspectionObjectData objectData;

        public InspectionObjectData Data => objectData;

        /// <summary>
        /// Called when player selects this object.
        /// </summary>
        public void Select()
        {
            Debug.Log($"Selected: {objectData.DisplayName}");
        }
    }
}