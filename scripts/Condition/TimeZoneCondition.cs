using System;
using UnityEditor;
using UnityEngine;

///<summary>
///指定した時間帯の間作業し続けると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/TimeZone Condition")]
    public class TimeZoneCondition : AchievementCondition
    {
        [Header("開始時間")]
        [SerializeField] private TimeHolder startTime;

        [Header("終了時間")]
        [SerializeField] private TimeHolder progressTime;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            return (uint)(IsAchieved(data) ? 1 : 0);
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return 1;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            if (progressTime.Hour > DateTime.Now.Hour &&
                progressTime.Minute > DateTime.Now.Minute &&
                progressTime.Second > DateTime.Now.Second)
            {
                return false;
            }

            DateTime beforeTime = DateTime.Now.AddSeconds(EditorApplication.timeSinceStartup);

            if (startTime.Hour > beforeTime.Hour &&
                startTime.Minute > beforeTime.Minute &&
                startTime.Second > beforeTime.Second)
            {
                return false;
            }
            return true;
        }
    }
}
