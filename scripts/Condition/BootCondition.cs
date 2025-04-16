using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Boot Condition")]
    public class BootCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private uint targetBootCount;
        [SerializeField] private bool isProgressCountAtOnce;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            if (isProgressCountAtOnce)
            {
                return 1;
            }
            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    return 1;

                case DayCategoryType.Daily:
                    return data.TodayBootCount;

                case DayCategoryType.Weekly:
                    return data.WeekBootDays;

                case DayCategoryType.Total:
                    return data.TotalBootCount;

                default:
                    return 0;
            }
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return targetBootCount;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    return 1 >= targetBootCount;

                case DayCategoryType.Daily:
                    return data.TodayBootCount >= targetBootCount;

                case DayCategoryType.Weekly:
                    return data.WeekBootDays >= targetBootCount;

                case DayCategoryType.Total:
                    return data.TotalBootCount >= targetBootCount;

                default:
                    Debug.LogWarning($"Invalid DayCategory: {dayCategory}");
                    return false;
            }
        }
    }
}
