using System.Collections.Generic;
using UnityEngine;
using InspectionSystem.Core;

namespace InspectionSystem.Objectives
{
    
    // Manages inspection objectives and tracks progress.
    
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField]
        private ObjectiveData objectiveData;

        // Stores inspected object IDs (no duplicates allowed)
        private readonly HashSet<string> completedObjects = new();

        // Stores all required object IDs
        private HashSet<string> requiredObjects;

        // Prevents completion event from firing multiple times
        private bool isCompleted;

        private void Awake()
        {
            if (objectiveData == null)
            {
                Debug.LogError("ObjectiveData is not assigned.", this);
                enabled = false;
                return;
            }

            requiredObjects = new HashSet<string>(objectiveData.RequiredObjectIds);
        }

        // Subscribe to game events
        private void OnEnable()
        {
            GameEvents.ObjectInspected += OnObjectInspected;
            GameEvents.ProgressLoaded += RestoreProgress;
        }

        // Unsubscribe from game events
        private void OnDisable()
        {
            GameEvents.ObjectInspected -= OnObjectInspected;
            GameEvents.ProgressLoaded -= RestoreProgress;
        }

        private void Start()
        {
            UpdateProgress();
        }

        // Called when an object is inspected
        private void OnObjectInspected(string objectId)
        {
            if (!requiredObjects.Contains(objectId))
            {
                return;
            }

            if (!completedObjects.Add(objectId))
            {
                return;
            }

            Debug.Log($"Inspected: {objectId}");

            // Save updated progress
            GameEvents.ProgressChanged?.Invoke(GetCompletedObjects());

            UpdateProgress();
            CheckCompletion();
        }

        private void RestoreProgress(List<string> loadedObjects)
        {
            completedObjects.Clear();

            foreach (string id in loadedObjects)
            {
                completedObjects.Add(id);
            }

            Debug.Log($"Restored Count = {completedObjects.Count}");

            UpdateProgress();
            CheckCompletion();
        }

        // Update progress count and notify UI
        private void UpdateProgress()
        {
            int completed = completedObjects.Count;
            int total = requiredObjects.Count;

            Debug.Log($"Progress Updated: {completed}/{total}");

            GameEvents.ObjectiveProgressUpdated?.Invoke(completed, total);
        }

        // Check if all required objects are inspected
        private void CheckCompletion()
        {
            if (isCompleted)
            {
                return;
            }

            if (completedObjects.Count != requiredObjects.Count)
            {
                return;
            }

            isCompleted = true;

            Debug.Log("Training Completed");

            // Notify that training is finished
            GameEvents.TrainingCompleted?.Invoke();
        }

        // Returns a copy of completed object IDs
        public List<string> GetCompletedObjects()
        {
            return new List<string>(completedObjects);
        }

        public void ResetObjectives()
        {
            completedObjects.Clear();
            isCompleted = false;
            UpdateProgress();
        }
    }
}