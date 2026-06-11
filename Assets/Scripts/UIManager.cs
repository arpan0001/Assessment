using TMPro;
using UnityEngine;

using InspectionSystem.Core;
using InspectionSystem.Data;
using InspectionSystem.Objectives;
using InspectionSystem.Save;

namespace InspectionSystem.UI
{
    /// <summary>
    /// Handles all UI updates.
    /// Only listens to events and updates UI.
    /// Contains no gameplay logic.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Object Information")]
        [SerializeField]
        private TMP_Text objectNameText;

        [SerializeField]
        private TMP_Text descriptionText;

        [Header("Objective UI")]
        [SerializeField]
        private TMP_Text objectiveText;

        [SerializeField]
        private TMP_Text progressText;

        [Header("Completion UI")]
        [SerializeField]
        private GameObject completionPanel;

        [SerializeField]
        private SaveManager saveManager;

        [SerializeField]
        private ObjectiveManager objectiveManager;



        // Currently selected object
        private InspectionObjectData selectedObject;

        private void OnEnable()
        {
            GameEvents.ObjectSelected += DisplayObjectInfo;

            GameEvents.ObjectiveProgressUpdated +=
                UpdateProgressUI;

            GameEvents.TrainingCompleted +=
                ShowCompletionPanel;
        }

        private void OnDisable()
        {
            GameEvents.ObjectSelected -= DisplayObjectInfo;

            GameEvents.ObjectiveProgressUpdated -=
                UpdateProgressUI;

            GameEvents.TrainingCompleted -=
                ShowCompletionPanel;
        }

        private void Start()
        {
            InitializeUI();
        }

        /// <summary>
        /// Sets default UI values.
        /// </summary>
        private void InitializeUI()
        {
            objectNameText.text = "Select Object";

            descriptionText.text =
                "Click an object to view information.";

            objectiveText.text =
                "Current Objective";

            progressText.text =
                "Progress: 0 / 0";

            completionPanel.SetActive(false);
        }

        /// <summary>
        /// Called when an object is selected.
        /// </summary>
        private void DisplayObjectInfo(
            InspectionObjectData objectData)
        {
            selectedObject = objectData;

            objectNameText.text =
                objectData.DisplayName;

            descriptionText.text =
                objectData.Description;
        }

        /// <summary>
        /// Called when progress changes.
        /// </summary>
        private void UpdateProgressUI(
            int completedObjectives,
            int totalObjectives)
        {
            progressText.text =
                $"Progress: {completedObjectives} / {totalObjectives}";
        }

        /// <summary>
        /// Called by the Inspect Button.
        /// </summary>
        public void OnInspectButtonPressed()
        {
            if (selectedObject == null)
            {
                Debug.LogWarning(
                    "No object selected.");

                return;
            }

            GameEvents.ObjectInspected?.Invoke(
                selectedObject.ObjectId);
        }

        /// <summary>
        /// Shows completion screen.
        /// </summary>
        private void ShowCompletionPanel()
        {
            completionPanel.SetActive(true);

        }

        /// <summary>
        /// Optional restart button.
        /// </summary>
        public void RestartTraining()
        {
            UnityEngine.SceneManagement.SceneManager
                .LoadScene(
                    UnityEngine.SceneManagement
                    .SceneManager
                    .GetActiveScene()
                    .buildIndex);
        }

        public void ResetTraining()
        {
            saveManager.ResetProgress();

            objectiveManager.ResetObjectives();

            completionPanel.SetActive(false);
        }
    
    }
}