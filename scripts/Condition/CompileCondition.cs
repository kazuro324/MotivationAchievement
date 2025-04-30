using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/Compile Condition"), Icon("Assets/Editor/scripts/Condition/Icons/CompileCondition.png")]
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
                return currentCount < targetCount ? 0u : 1u;
            }
            return (uint)Mathf.Clamp(currentCount, 0, GetMaxConditionCount(data));
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