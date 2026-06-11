using System;
using System.Collections.Generic;
using InspectionSystem.Data;

namespace InspectionSystem.Core
{
    public static class GameEvents
    {
        // Object selection
        public static Action<InspectionObjectData> ObjectSelected;

        // User pressed inspect
        public static Action<string> ObjectInspected;

        // Progress UI update
        public static Action<int, int> ObjectiveProgressUpdated;

        // Training completed
        public static Action TrainingCompleted;

        // Save data loaded
        public static Action<List<string>> ProgressLoaded;

        // Progress changed after inspection
        public static Action<List<string>> ProgressChanged;
    }
}