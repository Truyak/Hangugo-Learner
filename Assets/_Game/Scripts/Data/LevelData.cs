using System.Collections.Generic;
using UnityEngine;

namespace HangugoLearner.Data
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Hangugo/Level")]
    public class LevelData : ScriptableObject
    {
        [Header("Seviye/Alt Başlık Ayarları")]
        public string id;
        public string levelName;
        public int unlockCost = 2; // Varsayılan açma maliyeti
        
        [Header("İçerik")]
        public List<WordData> words;
    }
}
