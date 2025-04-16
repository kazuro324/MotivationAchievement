using UnityEngine;

///<summary>
///˜A‘±“ú”ì‹Æ‚µ‚Ä“ú”‚ªw’è”‚ğ’´‚¦‚é‚Æ’B¬
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/DateContinue Condition"), Icon("Assets/Editor/scripts/Condition/Icons/DateContinueCondition.png")]
    public class DateContinueCondition : AchievementCondition
    {
        private enum ContinueDateType
        {
            Current,
            Week,
            Highest
        }

        [SerializeField] private ContinueDateType continueDateType;
        [SerializeField] uint targetContinueDay;

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (continueDateType)
            {
                default:
                case ContinueDateType.Current:
                    return data.CurrentContinueDays > targetContinueDay;
                
                case ContinueDateType.Week:
                    return data.WeekContinueDays > targetContinueDay;

                case ContinueDateType.Highest:
                    return data.HighestContinueDays > targetContinueDay;
            }
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return targetContinueDay;
        }

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            switch (continueDateType)
            {
                default:
                case ContinueDateType.Current:
                    return data.CurrentContinueDays;

                case ContinueDateType.Week:
                    return data.WeekContinueDays;

                case ContinueDateType.Highest:
                    return data.HighestContinueDays;
            }
        }
    }
}
