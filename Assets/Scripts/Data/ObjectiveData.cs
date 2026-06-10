using System.Collections.Generic;
using UnityEngine;

namespace InspectionSystem.Objectives
{
    [CreateAssetMenu(
        fileName = "ObjectiveData",
        menuName = "Inspection System/Objective Data")]
    public class ObjectiveData : ScriptableObject
    {
        public List<string> RequiredObjectIds =
            new List<string>();
    }
}