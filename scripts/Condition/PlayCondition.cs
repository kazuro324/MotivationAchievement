using UnityEditor;
using UnityEngine;

///<summary>
///EditorÇ≈PlayModeÇ…ÇµÇΩâÒêîÇ™éwíËêîÇí¥Ç¶ÇÈÇ∆íBê¨
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Play Condition"), Icon("Assets/Editor/scripts/Condition/Icons/PlayCondition.png")]
    public class PlayCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private uint playCount;
        [SerializeField] private bool isProgressCountAtOnce;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint currentCount = 0;

            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    currentCount = data.PlayCount;
                    break;

                case DayCategoryType.Daily:
                    currentCount = data.TodayPlayCount;
                    break;

                case DayCategoryType.Weekly:
                    currentCount = data.WeekPlayCount;
                    break;

                case DayCategoryType.Total:
                    currentCount = data.TotalPlayCount;
                    break;

                default:
                    currentCount = 0;
                    break;
            }

            if (isProgressCountAtOnce)
            {
                return (uint)(currentCount < playCount ? 0 : 1);
            }
            return currentCount;
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return isProgressCountAtOnce ? 1 : playCount;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategoryType.CurrentSession:
                    return data.PlayCount >= playCount;

                case DayCategoryType.Daily:
                    return data.TodayPlayCount >= playCount;

                case DayCategoryType.Weekly:
                    return data.WeekPlayCount >= playCount;

                case DayCategoryType.Total:
                    return data.TotalPlayCount >= playCount;

                default:
                    Debug.LogWarning($"Invalid DayCategory: {dayCategory}");
                    return false;
            }
        }
    }
}
