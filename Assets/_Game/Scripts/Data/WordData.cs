using UnityEngine;

namespace HangugoLearner.Data
{
    [CreateAssetMenu(fileName = "NewWord", menuName = "Hangugo/Word")]
    public class WordData : ScriptableObject
    {
        [Header("Kelime Bilgisi")]
        [Tooltip("Hangul (Korece yazılışı)")]
        public string koreanWord;
        
        [Tooltip("Okunuşu (Latin alfabesiyle)")]
        public string pronunciation;
        
        [Tooltip("Türkçe karşılığı")]
        public string turkishMeaning;
        
        [Tooltip("Opsiyonel ses dosyası")]
        public AudioClip audioClip;
    }
}
