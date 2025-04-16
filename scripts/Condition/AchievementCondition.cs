using UnityEngine;

///<summary>
///ÀÑ‚Ì’B¬ğŒ‚ÌŠî’êƒNƒ‰ƒX
///</summary>
namespace Kazuro.Editor.Achievement
{
    public abstract class AchievementCondition : ScriptableObject
    {
        public abstract bool IsAchieved(AchievementDataManager data);

        public abstract uint GetMaxConditionCount(AchievementDataManager data);

        public abstract uint GetCurrentConditionCount(AchievementDataManager data);
    }
}
