using System;
using UnityEngine;

///<summary>
///指定した時間帯の間作業し続けると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    public class TimeZoneCondition : AchievementCondition
    {
        [Header("開始時間")]
        [SerializeField] private byte zoneFirstHour;
        [SerializeField] private byte zoneFirstMinute;
        [SerializeField] private byte zoneFirstSecond;

        [Header("終了時間")]
        [SerializeField] private byte zoneEndHour;
        [SerializeField] private byte zoneEndMinute;
        [SerializeField] private byte zoneEndSecond;

        const double Minutes60 = 60f;

        public override bool IsAchieved(AchievementDataManager data)
        {
            var startDate = DateToHourDouble(data.FirstStartDate);
            var startZoneDate = DateToHourDouble(zoneFirstSecond, zoneFirstMinute, zoneFirstSecond);
            var currentDate = DateToHourDouble(DateTime.Now);
            var endZoneDate = DateToHourDouble(zoneEndHour, zoneEndMinute, zoneEndSecond);
            if (startDate <= startZoneDate && currentDate >= endZoneDate)
            {
                return true;
            }
            return false;
        }

        private double DateToHourDouble(DateTime date)
        {
            var hour = date.Hour;
            var minutes = date.Minute / Minutes60;
            var seconds = (date.Second / Minutes60) / Minutes60;
            return hour * minutes + seconds;
        }

        private double DateToHourDouble(byte dHour, byte dMinute, byte dSecond)
        {
            var hour = dHour;
            var minutes = dMinute / Minutes60;
            var seconds = (dSecond / Minutes60) / Minutes60;
            return hour * minutes + seconds;
        }
    }

}
