using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HangugoLearner.Data;
using HangugoLearner.Managers;
using TMPro;

namespace HangugoLearner.UI
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private GameObject _levelButtonPrefab;
        [SerializeField] private GameObject _panelObject; // Bu panelin kendisi (aç/kapa için)
        [SerializeField] private GameObject _quizPanelObject; // Quiz başladığında açılacak panel

        private CourseData _currentCourse;

        public void Open(CourseData course)
        {
            _currentCourse = course;
            _panelObject.SetActive(true);
            PopulateGrid();
        }
        
        public void Close()
        {
            _panelObject.SetActive(false);
        }

        private void PopulateGrid()
        {
            foreach (Transform child in _gridContainer) Destroy(child.gameObject);

            if (_currentCourse == null || _currentCourse.levels == null) return;

            foreach (var level in _currentCourse.levels)
            {
                GameObject btnObj = Instantiate(_levelButtonPrefab, _gridContainer);
                Button btn = btnObj.GetComponent<Button>();
                TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

                txt.text = level.levelName;

                bool isUnlocked = GameManager.Instance.IsLevelUnlocked(level.id);

                if (isUnlocked)
                {
                    btn.onClick.AddListener(() => StartQuiz(level));
                }
                else
                {
                     txt.text += $"\n({level.unlockCost} Coin)";
                     btn.onClick.AddListener(() => TryUnlockLevel(level));
                }
            }
        }

        private void TryUnlockLevel(LevelData level)
        {
             if (GameManager.Instance.SpendCoins(level.unlockCost))
            {
                GameManager.Instance.UnlockLevel(level.id);
                PopulateGrid();
            }
        }

        private void StartQuiz(LevelData level)
        {
            Close(); // Level seçim ekranını kapat
            _quizPanelObject.SetActive(true); // Quiz ekranını aç
            QuizManager.Instance.StartQuiz(level);
        }
    }
}
