using UnityEditor;
using UnityEngine;

///<summary>
///w’è‚³‚ê‚½ŠÔì‹Æ‚ğ‚·‚é‚Æ’B¬
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Time Condition"), Icon("Assets/Editor/scripts/Condition/Icons/TimeCondition.png")]
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