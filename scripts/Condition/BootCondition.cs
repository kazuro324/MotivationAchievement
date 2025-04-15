using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Boot Condition")]
    public class BootCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private byte targetBootCount;
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
