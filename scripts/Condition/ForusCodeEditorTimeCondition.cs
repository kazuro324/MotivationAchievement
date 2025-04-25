using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(fileName = "ForusCodeEditorTimeConditionFile", menuName = "Kazuro/Editor/Achievement/Condition/ForusCodeEditorTimeCondition")]
    public class ForusCodeEditorTimeCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private TimeHolder targetTime;
        [SerializeField] private bool isProgressCountAtOnce;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            int intCurrentCount;
            if (isProgressCountAtOnce)
            {
                return (uint)(IsAchieved(data) ? 1 : 0);
            }
            switch (dayCategory) 
            {
                case DayCategoryType.Daily:
                    intCurrentCount = new TimeHolder(data.TodayFocusCodeEditorTime).ToMinutes();
                    break;

                case DayCategoryType.Weekly:
                    intCurrentCount = new TimeHolder(data.WeekFocusCodeEditorTime).ToMinutes();
                    break;

                case DayCategoryType.Total:
                    intCurrentCount = new TimeHolder(data.TotalFocusCodeEditorTime).ToMinutes();
                    break;

                default:
                    intCurrentCount = 0;
                    break;
            }

            return (uint)Mathf.Clamp(intCurrentCount, 0, GetMaxConditionCount(data));
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            var minute = (uint)targetTime.ToMinutes();
            if (minute <= 0)
            {
                minute = 1;
            }
            return isProgressCountAtOnce ? 1 : minute;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            var targetSecond = targetTime.ToSeconds();
            int currentSecond = 0;

            switch (dayCategory)
            {
                case DayCategoryType.Daily:
                    currentSecond = (int)data.TodayFocusCodeEditorTime;
                    break;

                case DayCategoryType.Weekly:
                    currentSecond = (int)data.WeekFocusCodeEditorTime;
                    break;

                case DayCategoryType.Total:
                    currentSecond = (int)data.TotalFocusCodeEditorTime;
                    break;

                default:
                    currentSecond = 0;
                    break;
            }

            return currentSecond >= targetSecond;
        }
    }
}