using UnityEngine;

///<summary>
///˜A‘±“ú”ì‹Æ‚µ‚Ä“ú”‚ªw’è”‚ğ’´‚¦‚é‚Æ’B¬
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/DateContinue Condition"), Icon("Assets/Editor/scripts/Condition/Icons/DateContinueCondition.png")]
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
        [SerializeField] private bool isProgressCountAtOnce;

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

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint currentCount = 0;

            switch (continueDateType)
            {
                default:
                case ContinueDateType.Current:
                    currentCount = data.CurrentContinueDays;
                    break;

                case ContinueDateType.Week:
                    currentCount = data.WeekContinueDays;
                    break;

                case ContinueDateType.Highest:
                    currentCount = data.HighestContinueDays;
                    break;
            }

            if (isProgressCountAtOnce)
            {
                return (uint)(currentCount < targetContinueDay ? 0 : 1);
            }
            return (uint)Mathf.Clamp(currentCount, 0, GetMaxConditionCount(data));
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return isProgressCountAtOnce ? 1 : targetContinueDay;
        }
    }
}
