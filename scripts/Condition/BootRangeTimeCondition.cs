using System;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    /// <summary>
    /// w’èŠÔ‘Ñ‚É‹N“®‚µ‚Ä‚¢‚é‚Æ’B¬
    /// </summary>
    public class BootRangeTimeCondition : AchievementCondition
    {
        [SerializeField] private TimeHolder startRangeTime;
        [SerializeField] private TimeHolder endRangeTime;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            return (uint)(IsAchieved(data) ? 1 : 0);
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return 1;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            if (data.TodayBootCount < 1)
            {
                return false;
            }
            
            DateTime startDate = startRangeTime.CreateDate();
            DateTime endDate = endRangeTime.CreateDate();

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