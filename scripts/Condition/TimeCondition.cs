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
        public int year;
        public byte month;
        public byte day;
        public byte hour;
        public byte minute;
        public byte second;

        const int ONEHOUR = 3600;
        const int ONEMINUTE = 60;

        public TimeHolder(int year,byte month,byte day,byte hour, byte minute, byte second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
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
            return new DateTime(
                this.year,
                this.month,
                this.day,
                this.hour,
                this.minute,
                this.second
                );
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