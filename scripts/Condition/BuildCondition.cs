using UnityEngine;
using static Kazuro.Editor.Achievement.AchievementCondition;

///<summary>
///�r���h�����񐔂��w�萔������ƒB��
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Build Condition")]
    public class BuildCondition : AchievementCondition
    {
        [SerializeField] private DayCategory dayCategory;
        [SerializeField] private byte targetBuildCount;
        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategory.Daily:
                    return data.CurrentBuildCount >= targetBuildCount;

                case DayCategory.Weekly:
                    return data.WeekBuildCount >= targetBuildCount;

                case DayCategory.Total:
                    return data.TotalBuildCount >= targetBuildCount;

                default:
                    Debug.LogWarning($"Invalid DayCategory: {dayCategory}");
                    return false;
            }
        }
    }
}
