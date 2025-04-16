using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/Boot Condition"), Icon("Assets/Editor/scripts/Condition/Icons/BootCondition.png")]
    public class BootCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private uint targetBootCount;
        [SerializeField] private bool isProgressCountAtOnce;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint currentCount = 0;

            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    currentCount = 1;
                    break;

                case DayCategoryType.Daily:
                    currentCount = data.TodayBootCount;
                    break;

                case DayCategoryType.Weekly:
                    currentCount = data.WeekBootDays;
                    break;

                case DayCategoryType.Total:
                    currentCount = data.TotalBootCount;
                    break;

                default:
                    currentCount = 0;
                    break;
            }

            if (isProgressCountAtOnce)
            {
                return (uint)(currentCount < targetBootCount ? 0 : 1);
            }
            return (uint)Mathf.Clamp(currentCount, 0, GetMaxConditionCount(data));
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return isProgressCountAtOnce ? 1 : targetBootCount;
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
