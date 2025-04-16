using UnityEngine;
using static Kazuro.Editor.Achievement.AchievementCondition;

///<summary>
///ビルドした回数が指定数超えると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/Build Condition"), Icon("Assets/Editor/scripts/Condition/Icons/BuildCondition.png")]
    public class BuildCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private uint targetBuildCount;
        [SerializeField] private bool isProgressCountAtOnce;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint currentCount = 0;

            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    currentCount = data.CurrentBuildCount;
                    break;

                case DayCategoryType.Daily:
                    currentCount = data.TodayBuildCount;
                    break;

                case DayCategoryType.Weekly:
                    currentCount = data.WeekBuildCount;
                    break;

                case DayCategoryType.Total:
                    currentCount = data.TotalBuildCount;
                    break;

                default:
                    return 0;
            }

            if (isProgressCountAtOnce)
            {
                return (uint)(currentCount < targetBuildCount ? 0 : 1);
            }
            return (uint)Mathf.Clamp(currentCount, 0, GetMaxConditionCount(data));
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return isProgressCountAtOnce ? 1 : targetBuildCount;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    return data.CurrentBuildCount >= targetBuildCount;

                case DayCategoryType.Daily:
                    return data.TodayBuildCount >= targetBuildCount;

                case DayCategoryType.Weekly:
                    return data.WeekBuildCount >= targetBuildCount;

                case DayCategoryType.Total:
                    return data.TotalBuildCount >= targetBuildCount;

                default:
                    Debug.LogWarning($"Invalid DayCategory: {dayCategory}");
                    return false;
            }
        }
    }
}
