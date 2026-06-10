using System;
using InspectionSystem.Data;

namespace InspectionSystem.Core
{
    public static class GameEvents
    {
        /// <summary>
        /// Fired when player selects an object.
        /// </summary>
        public static Action<InspectionObjectData> ObjectSelected;

        /// <summary>
        /// Fired when inspection is completed.
        /// </summary>
        public static Action<string> ObjectInspected;

        /// <summary>
        /// Fired when all objectives are finished.
        /// </summary>
        public static Action TrainingCompleted;

        public static Action<int, int> ObjectiveProgressUpdated;
    }
}