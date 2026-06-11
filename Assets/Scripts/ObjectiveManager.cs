using System.Collections.Generic;
using UnityEngine;

using InspectionSystem.Core;

namespace InspectionSystem.Objectives
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField]
        private ObjectiveData objectiveData;

        private readonly HashSet<string>
            completedObjects =
                new HashSet<string>();

        private void OnEnable()
        {
            GameEvents.ObjectInspected +=
                OnObjectInspected;

            GameEvents.ProgressLoaded +=
                RestoreProgress;
        }

        private void OnDisable()
        {
            GameEvents.ObjectInspected -=
                OnObjectInspected;

            GameEvents.ProgressLoaded -=
                RestoreProgress;
        }

        private void Start()
        {
            UpdateProgress();
        }

        private void OnObjectInspected(
            string objectId)
        {
            if (!objectiveData.RequiredObjectIds
                .Contains(objectId))
            {
                return;
            }

            if (completedObjects.Contains(
                objectId))
            {
                return;
            }

            completedObjects.Add(
                objectId);

            Debug.Log(
                $"Inspected: {objectId}");

            // Notify SaveManager AFTER update
            GameEvents.ProgressChanged?.Invoke(
                GetCompletedObjects());

            UpdateProgress();

            CheckCompletion();
        }

        private void RestoreProgress(
            List<string> loadedObjects)
        {
            completedObjects.Clear();

            foreach (string id
                     in loadedObjects)
            {
                completedObjects.Add(id);
            }

            Debug.Log(
                $"Restored Count = {completedObjects.Count}");

            UpdateProgress();

            CheckCompletion();
        }

        private void UpdateProgress()
        {
            int completed =
                completedObjects.Count;

            int total =
                objectiveData.RequiredObjectIds.Count;

            Debug.Log(
                $"Progress Updated: {completed}/{total}");

            GameEvents.ObjectiveProgressUpdated
                ?.Invoke(
                    completed,
                    total);
        }

        private void CheckCompletion()
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

        public List<string>
            GetCompletedObjects()
        {
            return new List<string>(
                completedObjects);
        }

        public void ResetObjectives()
        {
            completedObjects.Clear();

            UpdateProgress();
        }
    }
}