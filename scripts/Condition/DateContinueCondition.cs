using UnityEngine;

///<summary>
///˜A‘±“ú”ì‹Æ‚µ‚Ä“ú”‚ªw’è”‚ğ’´‚¦‚é‚Æ’B¬
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/DateContinue Condition")]
    public class DateContinueCondition : AchievementCondition
    {
        [SerializeField] int continueStart;

        public override bool IsAchieved(AchievementDataManager data)
        {
            return data.CurrentContinueDays > continueStart;
        }
    }
}
