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

        public override bool IsAchieved(AchievementDataManager data)
        {
            if (progressTime.CreateDate() > DateTime.Now)
            {
                return false;
            }
            
            if (startTime.CreateDate() > DateTime.Now.AddSeconds(EditorApplication.timeSinceStartup))
            {
                return false;
            }
            return true;
        }
    }
}
