using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public class CompileCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private uint targetCount;
        [SerializeField] private bool isProgressCountAtOnce;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint currentCount = 0;

            switch (dayCategory)
            {
                default:
                case DayCategoryType.CurrentSession:
                    currentCount = data.CurrentBuildCount;
                    break;

                case DayCategoryType.Daily:
                    currentCount = data.TodayCompileCount;
                    break;

                case DayCategoryType.Weekly:
                    currentCount = data.WeekCompileCount;
                    break;

                case DayCategoryType.Total:
                    currentCount = data.TotalCompileCount;
                    break;
            }

            if (isProgressCountAtOnce)
            {
                return (uint)(currentCount < targetCount ? 0 : 1);
            }
            return currentCount;
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return isProgressCountAtOnce ? 1 : targetCount;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                default:
                case DayCategoryType.CurrentSession:
                    return data.CurrentBuildCount >= targetCount;

                case DayCategoryType.Daily:
                    return data.TodayCompileCount >= targetCount;

                case DayCategoryType.Weekly:
                    return data.WeekCompileCount >= targetCount;

                case DayCategoryType.Total:
                    return data.TotalCompileCount >= targetCount;
            }
        }
    }
}