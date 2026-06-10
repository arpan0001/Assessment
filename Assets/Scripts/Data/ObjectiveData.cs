using System.Collections.Generic;
using UnityEngine;

namespace InspectionSystem.Objectives
{
    [CreateAssetMenu(
        fileName = "ObjectiveData",
        menuName = "Inspection System/Objective Data")]
    public class ObjectiveData : ScriptableObject
    {
        [Header("Objective Sequence")]
        public List<string> ObjectiveIds =
            new List<string>();
    }
}