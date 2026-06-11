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
    /// Listens to game events and updates UI elements.
    /// Does not contain gameplay logic.
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

        // Currently selected inspection object
        private InspectionObjectData selectedObject;

        // Subscribe to UI-related events
        private void OnEnable()
        {
            GameEvents.ObjectSelected += DisplayObjectInfo;

            GameEvents.ObjectiveProgressUpdated += UpdateProgressUI;

            GameEvents.TrainingCompleted += ShowCompletionPanel;
        }

        // Unsubscribe from events
        private void OnDisable()
        {
            GameEvents.ObjectSelected -= DisplayObjectInfo;

            GameEvents.ObjectiveProgressUpdated -= UpdateProgressUI;

            GameEvents.TrainingCompleted -= ShowCompletionPanel;
        }

        // Set default UI values
        private void Start()
        {
            InitializeUI();
        }

        /// <summary>
        /// Sets default text and hides completion panel.
        /// </summary>
        private void InitializeUI()
        {
            objectNameText.text = "Select Object";

            descriptionText.text = "Click an object to view information.";

            objectiveText.text = "Current Objective";

            progressText.text = "Progress: 0 / 0";

            completionPanel.SetActive(false);
        }

        /// <summary>
        /// Updates object information when an object is selected.
        /// </summary>
        private void DisplayObjectInfo(InspectionObjectData objectData)
        {
            selectedObject = objectData;

            objectNameText.text = objectData.DisplayName;

            descriptionText.text =objectData.Description;
        }

        /// <summary>
        /// Updates progress text on the UI.
        /// </summary>
        private void UpdateProgressUI( int completedObjectives,int totalObjectives)
        {
            progressText.text = $"Progress: {completedObjectives} / {totalObjectives}";
        }

        /// <summary>
        /// Called when the Inspect button is pressed.
        /// Sends inspection event for selected object.
        /// </summary>
        public void OnInspectButtonPressed()
        {
            // Prevent inspection if nothing is selected
            if (selectedObject == null)
            {
                Debug.LogWarning("No object selected.");

                return;
            }

            // Notify system that object was inspected
            GameEvents.ObjectInspected?.Invoke(selectedObject.ObjectId);
        }

        /// <summary>
        /// Shows completion panel when training is finished.
        /// </summary>
        private void ShowCompletionPanel()
        {
            completionPanel.SetActive(true);
        }

        /// <summary>
        /// Reloads the current scene.
        /// </summary>
        public void RestartTraining()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Clears saved progress and resets objectives.
        /// </summary>
        public void ResetTraining()
        {
            // Delete saved progress
            saveManager.ResetProgress();

            // Reset runtime objective progress
            objectiveManager.ResetObjectives();

            // Hide completion screen
            completionPanel.SetActive(false);
        }
    }
}