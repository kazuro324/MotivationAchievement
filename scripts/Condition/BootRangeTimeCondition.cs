using System;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    /// <summary>
    /// w’èŠÔ‘Ñ‚É‹N“®‚µ‚Ä‚¢‚é‚Æ’B¬
    /// </summary>
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/BootRangeTime Condition"), Icon("Assets/Editor/scripts/Condition/Icons/BootRangeTimeCondition.png")]
    public class BootRangeTimeCondition : AchievementCondition
    {
        [SerializeField] private TimeHolder startRangeTime;
        [SerializeField] private TimeHolder betweenRangeTime;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            return IsAchieved(data) ? 1u : 0u;
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return 1u;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            if (data.TodayBootCount < 1)
            {
                return false;
            }
            
            DateTime startDate = startRangeTime.CreateDate();
            DateTime endDate = betweenRangeTime.CreateDate();

            if (DateTime.Now <= startDate)
            {
                return false;
            }
            if (DateTime.Now > endDate)
            {
                return false;
            }
            return true;
        }
    }
}