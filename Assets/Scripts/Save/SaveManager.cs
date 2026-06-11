using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using InspectionSystem.Core;

namespace InspectionSystem.Save
{
    /// <summary>
    /// Handles saving, loading and deleting inspection progress.
    /// Data is stored as a JSON file.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        // --- ADDED FOR SINGLETON PATTERN ---
        public static SaveManager Instance { get; private set; }

        // Stores save data in memory
        private SaveData saveData;

        // Path where save file is stored
        private string SavePath => Path.Combine(Application.persistentDataPath, "InspectionSave.json");

        #region Initialization

        // --- ADDED FOR SINGLETON ENFORCEMENT ---
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps save tracking alive across scene changes
        }

        #endregion

        #region Unity Lifecycle

        // Subscribe to save event
        private void OnEnable()
        {
            GameEvents.ProgressChanged += SaveProgress;
        }

        // Unsubscribe from save event
        private void OnDisable()
        {
            GameEvents.ProgressChanged -= SaveProgress;
        }

        // Load saved data when game starts
        private IEnumerator Start()
        {
            yield return null;
            LoadProgress();
        }

        #endregion

        #region Save / Load

        // Save progress to JSON file
        private void SaveProgress(List<string> completedObjects)
        {
            try
            {
                saveData = new SaveData
                {
                    // Store completed object IDs
                    CompletedObjects = new List<string>(completedObjects)
                };

                // Convert data to JSON
                string json = JsonUtility.ToJson(saveData, true);

                // Write JSON to file
                File.WriteAllText(SavePath, json);

                Debug.Log($"Saved Count = {completedObjects.Count}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to save progress: {exception.Message}");
            }
        }

        // Load progress from JSON file
        private void LoadProgress()
        {
            try
            {
                // Return if save file doesn't exist
                if (!File.Exists(SavePath))
                {
                    saveData = new SaveData();
                    Debug.Log("No Save Found");
                    return;
                }

                // Read save file
                string json = File.ReadAllText(SavePath);

                // Convert JSON to SaveData object
                saveData = JsonUtility.FromJson<SaveData>(json);

                // Safety checks
                if (saveData == null) saveData = new SaveData();

                saveData.CompletedObjects ??= new List<string>();

                Debug.Log($"Loaded Count = {saveData.CompletedObjects.Count}");

                // Notify systems that progress is loaded
                GameEvents.ProgressLoaded?.Invoke(saveData.CompletedObjects);

                Debug.Log("Progress Restored");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to load progress: {exception.Message}");

                saveData = new SaveData();
            }
        }

        #endregion

        #region Public Methods

        // Delete save file and clear data
        public void DeleteSave()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                }

                saveData = new SaveData();

                Debug.Log("Save Deleted");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to delete save: {exception.Message}");
            }
        }

        // Reset all progress
        public void ResetProgress()
        {
            DeleteSave();
        }

        #endregion
    }
}