using UnityEngine;

///<summary>
///ÀÑ‚Ì’B¬ğŒ‚ÌŠî’êƒNƒ‰ƒX
///</summary>
namespace Kazuro.Editor.Achievement
{
    public abstract class AchievementCondition : ScriptableObject
    {
        public enum DayCategory
        {
            Daily,
            Weekly,
            Total
        }

        public abstract bool IsAchieved(AchievementDataManager data);
    }
}
