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
        [SerializeField] private DayCategory dayCategory;
        [SerializeField] private int playCount;

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                case DayCategory.Daily:
                    return data.PlayCount >= playCount;

                case DayCategory.Weekly:
                    return data.WeekPlayCount >= playCount;

                case DayCategory.Total:
                    return data.TotalPlayCount >= playCount;

                default:
                    Debug.LogWarning($"Invalid DayCategory: {dayCategory}");
                    return false;
            }
        }
    }
}
