using System.Collections.Generic;
using UnityEngine;

namespace HangugoLearner.Data
{
    [CreateAssetMenu(fileName = "NewCourse", menuName = "Hangugo/Course")]
    public class CourseData : ScriptableObject
    {
        [Header("Kurs/Kategori Ayarları")]
        public string id;
        public string courseName;
        public Sprite icon;
        public int unlockCost = 50; // Varsayılan açma maliyeti
        
        [Header("Eğitici İçerik (Tutorial)")]
        public string tutorialTitle;
        [TextArea(5, 10)]
        public string tutorialContent;

        [Header("Seviyeler")]
        public List<LevelData> levels;
    }
}
