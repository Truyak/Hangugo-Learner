using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HangugoLearner.Data;
using HangugoLearner.Managers;
using TMPro;

namespace HangugoLearner.UI
{
    public class CourseSelectionUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private GameObject _courseButtonPrefab;
        [SerializeField] private List<CourseData> _allCourses; // Editörden atanacak
        
        [Header("Tutorial UI")]
        [SerializeField] private GameObject _tutorialPanel;
        [SerializeField] private TextMeshProUGUI _tutorialTitle;
        [SerializeField] private TextMeshProUGUI _tutorialContent;
        [SerializeField] private Button _closeTutorialButton;

        private void Start()
        {
            _tutorialPanel.SetActive(false);
            _closeTutorialButton.onClick.AddListener(() => _tutorialPanel.SetActive(false));
            
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            // Temizle
            foreach (Transform child in _gridContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var course in _allCourses)
            {
                GameObject btnObj = Instantiate(_courseButtonPrefab, _gridContainer);
                Button btn = btnObj.GetComponent<Button>();
                TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                Image img = btnObj.GetComponentInChildren<Image>(); // Ikon varsa

                txt.text = course.courseName;

                bool isUnlocked = GameManager.Instance.IsCourseUnlocked(course.id);
                
                if (isUnlocked)
                {
                    btn.interactable = true;
                    btn.onClick.AddListener(() => OpenUnlockingCourse(course));
                }
                else
                {
                    // Kilitli durumu
                    txt.text += $"\n(Kilitli: {course.unlockCost} Coin)";
                    btn.onClick.AddListener(() => TryUnlockCourse(course));
                }
            }
        }

        private void TryUnlockCourse(CourseData course)
        {
            if (GameManager.Instance.CurrentCoins >= course.unlockCost)
            {
                bool success = GameManager.Instance.SpendCoins(course.unlockCost);
                if (success)
                {
                    GameManager.Instance.UnlockCourse(course.id);
                    PopulateGrid(); // UI yenile
                    ShowTutorial(course);
                }
            }
            else
            {
                Debug.Log("Yetersiz bakiye!");
            }
        }

        private void OpenUnlockingCourse(CourseData course)
        {
            // Alt seviye seçim ekranına veya doğrudan quize yönlendir
            Debug.Log($"{course.courseName} seçildi.");
            // Burada LevelSelectionUI açılabilir. 
            // Demo için doğrudan ilk level'ı başlatalım mı? Yoksa alt menü mü yapalım?
            // "Alt menü" mantığı daha uygun.
            // Şimdilik Tutorial gösterelim yine de hatırlatma olsun.
            ShowTutorial(course);
        }

        private void ShowTutorial(CourseData course)
        {
            if (!string.IsNullOrEmpty(course.tutorialTitle))
            {
                _tutorialPanel.SetActive(true);
                _tutorialTitle.text = course.tutorialTitle;
                _tutorialContent.text = course.tutorialContent;
            }
        }
    }
}
