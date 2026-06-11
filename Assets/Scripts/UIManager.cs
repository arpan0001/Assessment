using TMPro;
using UnityEngine;
using InspectionSystem.Core;
using InspectionSystem.Data;
using InspectionSystem.Objectives;
using InspectionSystem.Save;

namespace InspectionSystem.UI
{
  
    // Listens to game events and updates UI elements
    
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

        
        //Sets default text and hides completion panel.
        
        private void InitializeUI()
        {
            objectNameText.text = "Select Object";

            descriptionText.text = "Click an object to view information.";

            objectiveText.text = "Current Objective";

            progressText.text = "Progress: 0 / 0";

            completionPanel.SetActive(false);
        }

       
        // Updates object information when an object is selected
      
        private void DisplayObjectInfo(InspectionObjectData objectData)
        {
            selectedObject = objectData;

            objectNameText.text = objectData.DisplayName;

            descriptionText.text =objectData.Description;
        }

       
        private void UpdateProgressUI( int completedObjectives,int totalObjectives)
        {
            progressText.text = $"Progress: {completedObjectives} / {totalObjectives}";
        }

        
        
        // Sends inspection event for selected object
        
        public void OnInspectButtonPressed()
        {
            
            if (selectedObject == null)
            {
                Debug.LogWarning("No object selected.");

                return;
            }

            
            GameEvents.ObjectInspected?.Invoke(selectedObject.ObjectId);
        }

        
        private void ShowCompletionPanel()
        {
            completionPanel.SetActive(true);
        }

       
        public void RestartTraining()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        
        public void ResetTraining()
        {
           
            saveManager.ResetProgress();

            objectiveManager.ResetObjectives();

            completionPanel.SetActive(false);
        }
    }
}