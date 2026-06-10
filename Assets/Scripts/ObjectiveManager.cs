using UnityEngine;
using InspectionSystem.Core;

namespace InspectionSystem.Objectives
{
    public class ObjectiveManager : MonoBehaviour
    {
        [Header("Objective Data")]
        [SerializeField]
        private ObjectiveData objectiveData;

        private int currentObjectiveIndex;

        public int CurrentObjectiveIndex =>
            currentObjectiveIndex;

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
            DisplayCurrentObjective();
        }

        private void OnObjectInspected(
            string objectId)
        {
            // Safety Check
            if (currentObjectiveIndex >=
                objectiveData.ObjectiveIds.Count)
                return;

            string requiredObjectId =
                objectiveData.ObjectiveIds[
                    currentObjectiveIndex];

            // Wrong object selected
            if (objectId != requiredObjectId)
            {
                Debug.Log(
                    $"Wrong Object! Expected: {requiredObjectId}");

                return;
            }

            Debug.Log(
                $"Correct Object: {objectId}");

            currentObjectiveIndex++;

            GameEvents.ObjectiveProgressUpdated?.Invoke(
            currentObjectiveIndex,
             objectiveData.ObjectiveIds.Count);

            // All objectives completed
            if (currentObjectiveIndex >=
                objectiveData.ObjectiveIds.Count)
            {
                Debug.Log(
                    "Training Completed!");

                GameEvents.TrainingCompleted?.Invoke();

                return;
            }

            DisplayCurrentObjective();
        }

        private void DisplayCurrentObjective()
        {
            string currentObjective =
                objectiveData.ObjectiveIds[
                    currentObjectiveIndex];

            Debug.Log(
                $"Current Objective: {currentObjective}");
        }
    }
}