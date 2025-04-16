using System;
using UnityEditor;
using UnityEngine;

///<summary>
///�w�肵�����ԑт̊ԍ�Ƃ�������ƒB��
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/TimeZone Condition")]
    public class TimeZoneCondition : AchievementCondition
    {
        [Header("�J�n����")]
        [SerializeField] private TimeHolder startTime;

        [Header("�I������")]
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
