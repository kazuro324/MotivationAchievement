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
        [SerializeField] private double second;
        [SerializeField] private double minute;
        [SerializeField] private double hour;

        const double ONEHOUR = 3600d;
        const double ONEMINUTE = 60d;

        public override bool IsAchieved(AchievementDataManager data)
        {
            if (EditorApplication.timeSinceStartup >= ToSeconds())
            {
                return true;
            }
            return false;
        }

        private double ToSeconds()
        {
            return (hour * ONEHOUR) + (minute * ONEMINUTE) + second;
        }
    }
}