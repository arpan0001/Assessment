using System.Collections.Generic;
using UnityEngine;

using InspectionSystem.Core;

namespace InspectionSystem.Objectives
{
    /// <summary>
    /// Tracks inspection progress.
    /// Objects may be inspected in any order.
    /// </summary>
    public class ObjectiveManager : MonoBehaviour
    {
        [Header("Objective Data")]
        [SerializeField]
        private ObjectiveData objectiveData;

        // Stores completed inspections
        private readonly HashSet<string>
            completedObjects =
                new HashSet<string>();

        public int CompletedCount =>
            completedObjects.Count;

        public int TotalCount =>
            objectiveData.RequiredObjectIds.Count;

        private void OnEnable()
        {
            GameEvents.ObjectInspected +=
                OnObjectInspected;
        }

        private void OnDisable()
        {
            GameEvents.ObjectInspected -=
                OnObjectInspected;
        }

        private void Start()
        {
            UpdateProgress();
        }

        private void OnObjectInspected(
            string objectId)
        {
            // Ignore invalid IDs
            if (!objectiveData.RequiredObjectIds
                .Contains(objectId))
            {
                return;
            }

            // Ignore already completed objects
            if (completedObjects.Contains(
                objectId))
            {
                Debug.Log(
                    $"{objectId} already inspected.");

                return;
            }

            completedObjects.Add(
                objectId);

            Debug.Log(
                $"{objectId} inspection completed.");

            UpdateProgress();

            CheckTrainingCompletion();
        }

        private void UpdateProgress()
        {
            int completed =
                completedObjects.Count;

            int total =
                objectiveData.RequiredObjectIds.Count;

            GameEvents.ObjectiveProgressUpdated
                ?.Invoke(
                    completed,
                    total);
        }

        private void CheckTrainingCompletion()
        {
            if (completedObjects.Count !=
                objectiveData.RequiredObjectIds.Count)
            {
                return;
            }

            Debug.Log(
                "Training Completed");

            GameEvents.TrainingCompleted
                ?.Invoke();
        }

        public bool IsObjectCompleted(
            string objectId)
        {
            return completedObjects.Contains(
                objectId);
        }

        public void ResetObjectives()
        {
            completedObjects.Clear();

            UpdateProgress();
        }
    }
}