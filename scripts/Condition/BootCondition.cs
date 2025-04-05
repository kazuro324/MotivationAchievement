using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Boot Condition")]
    public class BootCondition : AchievementCondition
    {
        [SerializeField] private DayCategory dayCategory;
        [SerializeField] private byte targetBootCount;
        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategory.Daily:
                    return data.TodayBootCount >= targetBootCount;

                case DayCategory.Weekly:
                    return data.WeekBootDays >= targetBootCount;

                case DayCategory.Total:
                    return data.TotalBootCount >= targetBootCount;

                default:
                    Debug.LogWarning($"Invalid DayCategory: {dayCategory}");
                    return false;
            }
        }
    }
}
