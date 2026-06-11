using System;
using System.Collections.Generic;

namespace InspectionSystem.Save
{
    [Serializable]
    public class SaveData
    {
        public List<string> CompletedObjects =
            new List<string>();
    }
}