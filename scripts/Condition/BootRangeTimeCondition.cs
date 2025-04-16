using System;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    /// <summary>
    /// �w�莞�ԑтɋN�����Ă���ƒB��
    /// </summary>
    public class BootRangeTimeCondition : AchievementCondition
    {
        [SerializeField] private TimeHolder startRangeTime;
        [SerializeField] private TimeHolder endRangeTime;

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