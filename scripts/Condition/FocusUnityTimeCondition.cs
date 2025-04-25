using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(fileName = "FocusUnityTimeConditionFile", menuName = "Kazuro/Editor/Achievement/Condition/FocusUnityTimeCondition")]
    public class FocusUnityTimeCondition : AchievementCondition
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
                    intCurrentCount = new TimeHolder(data.TodayFocusUnityEditorTime).ToMinutes();
                    break;

                case DayCategoryType.Weekly:
                    intCurrentCount = new TimeHolder(data.WeekFocusUnityEditorTime).ToMinutes();
                    break;

                case DayCategoryType.Total:
                    intCurrentCount = new TimeHolder(data.TotalFocusUnityEditorTime).ToMinutes();
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
                    currentSecond = (int)data.TodayFocusUnityEditorTime;
                    break;

                case DayCategoryType.Weekly:
                    currentSecond = (int)data.WeekFocusUnityEditorTime;
                    break;

                case DayCategoryType.Total:
                    currentSecond = (int)data.TotalFocusUnityEditorTime;
                    break;

                default:
                    currentSecond = 0;
                    break;
            }

            return currentSecond >= targetSecond;
        }
    }
}