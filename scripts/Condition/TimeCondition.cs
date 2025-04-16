using System;
using UnityEditor;
using UnityEngine;

///<summary>
///w’è‚³‚ê‚½ŠÔì‹Æ‚ğ‚·‚é‚Æ’B¬
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

        public static TimeHolder operator +(TimeHolder a, TimeHolder b)
        {
            a.hour += b.hour;
            a.minute += b.minute;
            a.second += b.second;
            return a;
        }
    }

    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Time Condition"), Icon("Assets/Editor/scripts/Condition/Icons/TimeCondition.png")]
    public class TimeCondition : AchievementCondition
    {
        [SerializeField] private DayCategoryType dayCategory;

        [SerializeField] private TimeHolder timeholder;

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
            return targetTime >= timeholder.ToSeconds();
        }

        
    }
}