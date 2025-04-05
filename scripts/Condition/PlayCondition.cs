using UnityEditor;
using UnityEngine;

///<summary>
///Editor‚ÅPlayMode‚É‚µ‚½‰ñ”‚ªw’è”‚ğ’´‚¦‚é‚Æ’B¬
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Play Condition")]
    public class PlayCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private int playCount;

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategoryType.Daily:
                    return data.PlayCount >= playCount;

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
