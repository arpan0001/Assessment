using UnityEngine;

namespace InspectionSystem.Data
{
    [CreateAssetMenu(
        fileName = "InspectionObjectData",
        menuName = "Inspection System/Object Data")]
    public class InspectionObjectData : ScriptableObject
    {
        [Header("Unique Object ID")]
        [Tooltip("Used internally for objectives and save system.")]
        public string ObjectId;

        [Header("Display Name")]
        public string DisplayName;

        [Header("Description")]
        [TextArea(3, 5)]
        public string Description;
    }
}