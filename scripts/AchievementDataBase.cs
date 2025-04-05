using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/DataBase")]
    public class AchievementDataBase : ScriptableObject
    {
        [SerializeField] private Achievement[] achievements;

        public Achievement[] Achievements { get { return achievements; } }
    }
}
