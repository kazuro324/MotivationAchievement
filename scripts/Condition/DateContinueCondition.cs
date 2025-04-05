using UnityEngine;

///<summary>
///�A��������Ƃ��ē������w�萔�𒴂���ƒB��
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
