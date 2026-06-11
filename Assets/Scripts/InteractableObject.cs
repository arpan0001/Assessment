using UnityEngine;
using InspectionSystem.Data;
using InspectionSystem.Core;

namespace InspectionSystem.Interaction
{

    // Represents an object that can be selected and inspected by the player
    

    public class InteractableObject : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField]
        private InspectionObjectData objectData;

        
        public InspectionObjectData Data => objectData;

        
        // Called when object is selected
       
        public void Select()
        {
            if (objectData == null)
            {
                Debug.LogWarning( $"{name} has no data assigned.");

                return;
            }

            GameEvents.ObjectSelected?.Invoke( objectData);
        }
    }
}