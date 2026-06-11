using System.Collections;
using System.IO;
using UnityEngine;

using InspectionSystem.Core;

namespace InspectionSystem.Save
{
    public class SaveManager : MonoBehaviour
    {
        private SaveData saveData;

        private string SavePath =>
            Path.Combine(
                Application.persistentDataPath,
                "InspectionSave.json");

        private void OnEnable()
        {
            GameEvents.ProgressChanged +=
                SaveProgress;
        }

        private void OnDisable()
        {
            GameEvents.ProgressChanged -=
                SaveProgress;
        }

        private IEnumerator Start()
        {
            // Wait one frame so all systems subscribe
            yield return null;

            LoadProgress();
        }

        private void SaveProgress(
            System.Collections.Generic.List<string>
            completedObjects)
        {
            saveData = new SaveData();

            saveData.CompletedObjects =
                completedObjects;

            string json =
                JsonUtility.ToJson(
                    saveData,
                    true);

            File.WriteAllText(
                SavePath,
                json);

            Debug.Log(
                $"Saved Count = {completedObjects.Count}");
        }

        private void LoadProgress()
        {
            if (!File.Exists(
                SavePath))
            {
                saveData =
                    new SaveData();

                Debug.Log(
                    "No Save Found");

                return;
            }

            string json =
                File.ReadAllText(
                    SavePath);

            saveData =
                JsonUtility.FromJson<SaveData>(
                    json);

            Debug.Log(
                $"Loaded Count = {saveData.CompletedObjects.Count}");

            GameEvents.ProgressLoaded?.Invoke(
                saveData.CompletedObjects);

            Debug.Log(
                "Progress Restored");
        }

        public void DeleteSave()
        {
            if (File.Exists(
                SavePath))
            {
                File.Delete(
                    SavePath);
            }

            saveData =
                new SaveData();

            Debug.Log(
                "Save Deleted");
        }

        public void ResetProgress()
        {
            DeleteSave();
        }
    }
}