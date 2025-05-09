using UnityEditor;
using UnityEngine;

///<summary>
///指定された時間作業をすると達成する
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Time Condition")]
    public class TimeCondition : AchievementCondition
    {

        [SerializeField] private double hour;
        public override bool IsAchieved(AchievementDataManager data)
        {
            if (EditorApplication.timeSinceStartup >= HourToSeconds())
            {
                return true;
            }
            return false;
        }

        private double HourToSeconds()
        {
            return (hour * 60d) * 60d;
        }
    }
}