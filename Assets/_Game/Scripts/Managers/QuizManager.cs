using System.Collections.Generic;
using UnityEngine;
using HangugoLearner.Data;
using System.Linq;

namespace HangugoLearner.Managers
{
    public class QuizManager : MonoBehaviour
    {
        public static QuizManager Instance { get; private set; }
        
        [Header("Ayarlar")]
        [SerializeField] private int _coinsPerCorrectAnswer = 5;
        
        // Quiz State
        private List<WordData> _currentQuestionList;
        private int _currentIndex;
        private int _score;
        public bool IsTurkishToKorean { get; private set; }

        // Events for UI
        public System.Action<WordData, List<string>> OnQuestionReady; // Soru Kelimesi, Şıklar
        public System.Action<bool> OnAnswerResult; // Doğru/Yanlış
        public System.Action<int> OnQuizFinished; // Skor

        private void Awake()
        {
            Instance = this;
        }

        public void StartQuiz(LevelData levelData, bool mexicanStandoffMode = false) // StandName placeholder
        {
            if (levelData.words == null || levelData.words.Count == 0)
            {
                Debug.LogError("Level has no words!");
                return;
            }

            _currentQuestionList = new List<WordData>(levelData.words);
            ShuffleList(_currentQuestionList);
            
            _currentIndex = 0;
            _score = 0;
            
            // Rastgele yön belirle
            IsTurkishToKorean = Random.value > 0.5f;

            GenerateNextQuestion();
        }

        private void GenerateNextQuestion()
        {
            if (_currentIndex >= _currentQuestionList.Count)
            {
                OnQuizFinished?.Invoke(_score);
                GameManager.Instance.AddCoins(_score);
                return;
            }

            WordData currentWord = _currentQuestionList[_currentIndex];
            
            // Generate multiple choice options
            List<string> options = new List<string>();
            string correctAnswer = IsTurkishToKorean ? currentWord.koreanWord : currentWord.turkishMeaning;
            options.Add(correctAnswer);

            // Add distractors
            List<WordData> distractors = new List<WordData>(_currentQuestionList);
            distractors.Remove(currentWord);
            ShuffleList(distractors);

            foreach (var distractor in distractors)
            {
                if (options.Count >= 4) break;
                string wrongOption = IsTurkishToKorean ? distractor.koreanWord : distractor.turkishMeaning;
                options.Add(wrongOption);
            }

            // Shuffle options
            ShuffleList(options);

            OnQuestionReady?.Invoke(currentWord, options);
        }

        public void SubmitAnswer(string selectedAnswer)
        {
            WordData currentWord = _currentQuestionList[_currentIndex];
            string correctAnswer = IsTurkishToKorean ? currentWord.koreanWord : currentWord.turkishMeaning;

            bool isCorrect = (selectedAnswer == correctAnswer);
            
            if (isCorrect)
            {
                _score += _coinsPerCorrectAnswer;
            }

            OnAnswerResult?.Invoke(isCorrect);
            
            _currentIndex++;
            // Kısa bir gecikme ile sonraki soruya geçilebilir, UI Manager bunu tetikleyebilir
            // Şimdilik doğrudan çağırmıyoruz, UI animasyonu bitince NextQuestion çağırır.
        }

        public void NextQuestion()
        {
            GenerateNextQuestion();
        }
        
        private void ShuffleList<T>(List<T> list)
        {
             int n = list.Count;
             while (n > 1) {
                 n--;
                 int k = Random.Range(0, n + 1);
                 T value = list[k];
                 list[k] = list[n];
                 list[n] = value;
             }
        }
    }
}
