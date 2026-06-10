using UnityEngine;
using InspectionSystem.Data;
using InspectionSystem.Core;

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
            GameEvents.ObjectSelected?.Invoke(objectData);
        }
    }
}