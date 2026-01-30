using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace HangugoLearner.Managers
{
    [System.Serializable]
    public class PlayerProgress
    {
        public int coins = 100;
        public List<string> unlockedCourseIds = new List<string>();
        public List<string> unlockedLevelIds = new List<string>();
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int CurrentCoins => _progress.coins;
        
        private PlayerProgress _progress;
        private string _savePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _savePath = Path.Combine(Application.persistentDataPath, "player_progress.json");
                LoadData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddCoins(int amount)
        {
            _progress.coins += amount;
            SaveData();
        }

        public bool SpendCoins(int amount)
        {
            if (_progress.coins >= amount)
            {
                _progress.coins -= amount;
                SaveData();
                return true;
            }
            return false;
        }

        public bool IsCourseUnlocked(string courseId)
        {
            return _progress.unlockedCourseIds.Contains(courseId);
        }

        public void UnlockCourse(string courseId)
        {
            if (!_progress.unlockedCourseIds.Contains(courseId))
            {
                _progress.unlockedCourseIds.Add(courseId);
                SaveData();
            }
        }
        
        public bool IsLevelUnlocked(string levelId)
        {
            return _progress.unlockedLevelIds.Contains(levelId);
        }

        public void UnlockLevel(string levelId)
        {
            if (!_progress.unlockedLevelIds.Contains(levelId))
            {
                _progress.unlockedLevelIds.Add(levelId);
                SaveData();
            }
        }

        private void SaveData()
        {
            string json = JsonUtility.ToJson(_progress, true);
            File.WriteAllText(_savePath, json);
        }

        private void LoadData()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                _progress = JsonUtility.FromJson<PlayerProgress>(json);
            }
            else
            {
                _progress = new PlayerProgress();
                // Varsayılan olarak ilk kurs açık olabilir
                // _progress.unlockedCourseIds.Add("course_1");
            }
        }
    }
}
