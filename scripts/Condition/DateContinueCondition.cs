using UnityEngine;

///<summary>
///連続日数作業して日数が指定数を超えると達成
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
