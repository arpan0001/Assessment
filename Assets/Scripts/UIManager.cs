using TMPro;
using UnityEngine;

using InspectionSystem.Core;
using InspectionSystem.Data;

namespace InspectionSystem.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text objectNameText;

        [SerializeField]
        private TMP_Text descriptionText;

        [SerializeField]
        private GameObject completionPanel;

        private InspectionObjectData selectedObject;

        private void OnEnable()
        {
            GameEvents.ObjectSelected +=
                DisplayObject;

            GameEvents.TrainingCompleted +=
                ShowCompletionPanel;
        }

        private void OnDisable()
        {
            GameEvents.ObjectSelected -=
                DisplayObject;

            GameEvents.TrainingCompleted -=
                ShowCompletionPanel;
        }

        private void DisplayObject(
            InspectionObjectData objectData)
        {
            selectedObject = objectData;

            objectNameText.text =
                objectData.DisplayName;

            descriptionText.text =
                objectData.Description;
        }

        public void OnInspectButtonPressed()
        {
            if (selectedObject == null)
                return;

            GameEvents.ObjectInspected?.Invoke(
                selectedObject.ObjectId);
        }

        private void ShowCompletionPanel()
        {
            completionPanel.SetActive(true);
        }
    }
}