using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

///<summary>
///éwíËÇ≥ÇÍÇΩéûä‘çÏã∆ÇÇ∑ÇÈÇ∆íBê¨
///</summary>
namespace Kazuro.Editor.Achievement
{
    [System.Serializable]
    public struct TimeHolder
    {
        [SerializeField] private byte hour;
        [SerializeField] private byte minute;
        [SerializeField] private byte second;

        public byte Hour { get { return hour; } }

        public byte Minute { get { return minute; } }

        public byte Second { get { return second; } }

        const int ONEHOUR = 3600;
        const int ONEMINUTE = 60;

        public TimeHolder(byte hour, byte minute, byte second)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public int ToSeconds()
        {
            return (hour * ONEHOUR) + (minute * ONEMINUTE) + second;
        }

        public uint ToHours()
        {
            return (uint)(hour + (minute / ONEMINUTE) + (second / ONEHOUR));
        }

        public DateTime CreateDate()
        {
            DateTime tempTime = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day
                );

            tempTime.AddSeconds(ToSeconds());
            return tempTime;
        }
    }

    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Time Condition"), Icon("Assets/Editor/scripts/Condition/Icons/TimeCondition.png")]
    public class TimeCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;

        [SerializeField] private TimeHolder timeHolder;
        [SerializeField] private bool isProgressCountAtOnce;

        const int ONEHOUR = 3600;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint value = 0;

            switch (dayCategory)
            {
                default:
                case DayCategoryType.CurrentSession:
                    value = (uint)EditorApplication.timeSinceStartup;
                    break;

                case DayCategoryType.Daily:
                    value = (uint)data.TodayWorkTime;
                    break;

                case DayCategoryType.Weekly:
                    value = (uint)data.WeekWorkTime;
                    break;

                case DayCategoryType.Total:
                    value = (uint)data.TotalWorkTime;
                    break;
            }

            if (isProgressCountAtOnce)
            {
                return (uint)(value < timeHolder.ToHours() ? 0 : 1);
            }
            return value;
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return isProgressCountAtOnce ? 1 : timeHolder.ToHours();
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            switch (dayCategory)
            {
                default:
                case DayCategoryType.CurrentSession:
                    return CheckTime((uint)EditorApplication.timeSinceStartup);

                case DayCategoryType.Daily:
                    return CheckTime(data.TodayWorkTime);

                case DayCategoryType.Weekly:
                    return CheckTime(data.WeekWorkTime);

                case DayCategoryType.Total:
                    return CheckTime(data.TotalWorkTime);
            }
        }

        private bool CheckTime(uint targetTime)
        {
            return targetTime >= timeHolder.ToSeconds();
        }

        private uint SecondToHours(double second)
        {
            return (uint)(second / ONEHOUR);
        }
    }
}