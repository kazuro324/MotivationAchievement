using Codice.Client.BaseCommands;
using System;
using UnityEditor;
using UnityEngine;

///<summary>
///指定した時間帯の間作業し続けると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/TimeZone Condition"), Icon("Assets/Editor/scripts/Condition/Icons/TimeZoneCondition.png")]
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
            DateTime startDate = startTime.CreateDate();
            DateTime afterTime = startDate.AddSeconds(progressTime.ToSeconds());

            TimeSpan afterDifference = afterTime - DateTime.Now;
            //差を見てほしいですねはい
            if (afterDifference.TotalSeconds < 0)
            {
                return false;
            }

            DateTime beforeDate = DateTime.Now.AddSeconds(EditorApplication.timeSinceStartup);

            TimeSpan beforeDifference = startDate - beforeDate;
            if (beforeDifference.TotalSeconds > 0)
            {
                return false;
            }
            return true;
        }
    }
}
