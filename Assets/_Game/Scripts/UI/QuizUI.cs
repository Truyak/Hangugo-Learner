using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using HangugoLearner.Managers;
using HangugoLearner.Data;
using System.Collections;

namespace HangugoLearner.UI
{
    public class QuizUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _questionText;
        [SerializeField] private List<Button> _answerButtons;
        [SerializeField] private TextMeshProUGUI _feedbackText;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _nextButton; // Sonraki soruya geçmesi için opsiyonel

        private void Start()
        {
            QuizManager.Instance.OnQuestionReady += UpdateQuestionUI;
            QuizManager.Instance.OnAnswerResult += ShowFeedback;
            QuizManager.Instance.OnQuizFinished += ShowResults;
            
            _resultPanel.SetActive(false);
            _feedbackText.text = "";
        }

        private void OnDestroy()
        {
            if (QuizManager.Instance != null)
            {
                QuizManager.Instance.OnQuestionReady -= UpdateQuestionUI;
                QuizManager.Instance.OnAnswerResult -= ShowFeedback;
                QuizManager.Instance.OnQuizFinished -= ShowResults;
            }
        }

        private void UpdateQuestionUI(WordData word, List<string> options)
        {
            // Soru metnini ayarla
            if (QuizManager.Instance.IsTurkishToKorean)
            {
                _questionText.text = $"'{word.turkishMeaning}' kelimesinin Korecesi nedir?";
            }
            else
            {
                _questionText.text = $"'{word.koreanWord}' kelimesinin Türkçesi nedir?";
            }

            _feedbackText.text = "";

            // Butonları ayarla
            for (int i = 0; i < _answerButtons.Count; i++)
            {
                if (i < options.Count)
                {
                    _answerButtons[i].gameObject.SetActive(true);
                    _answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
                    
                    string answer = options[i];
                    _answerButtons[i].onClick.RemoveAllListeners();
                    _answerButtons[i].onClick.AddListener(() => SubmitAnswer(answer));
                    _answerButtons[i].interactable = true;
                }
                else
                {
                    _answerButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void SubmitAnswer(string answer)
        {
            // Cevap verildikten sonra butonları kilitle
            foreach (var btn in _answerButtons)
            {
                btn.interactable = false;
            }
            QuizManager.Instance.SubmitAnswer(answer);
        }

        private void ShowFeedback(bool isCorrect)
        {
            _feedbackText.text = isCorrect ? "DOĞRU!" : "YANLIŞ!";
            _feedbackText.color = isCorrect ? Color.green : Color.red;
            
            // Otomatik geçiş için coroutine başlat
            StartCoroutine(NextQuestionRoutine());
        }

        private IEnumerator NextQuestionRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            QuizManager.Instance.NextQuestion();
        }

        private void ShowResults(int score)
        {
            _resultPanel.SetActive(true);
            _scoreText.text = $"Toplam Puan: {score}";
            _questionText.text = "";
            _feedbackText.text = "";
            foreach(var btn in _answerButtons) btn.gameObject.SetActive(false);
        }

        public void ReturnToMenu()
        {
            // Menu sahnesine dönüş veya panel kapatma
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene"); // Sahne adını sonra netleştiririz
        }
    }
}
