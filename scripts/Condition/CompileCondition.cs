using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public class CompileCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private uint targetCount;


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