using UnityEngine;
using static Kazuro.Editor.Achievement.AchievementCondition;

///<summary>
///�r���h�����񐔂��w�萔������ƒB��
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Build Condition"), Icon("Assets/Editor/scripts/Condition/Icons/BuildCondition.png")]
    public class BuildCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private byte targetBuildCount;
        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategoryType.Daily:
                    return data.CurrentBuildCount >= targetBuildCount;

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
