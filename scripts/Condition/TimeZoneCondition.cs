using Codice.Client.BaseCommands;
using System;
using UnityEditor;
using UnityEngine;

///<summary>
///�w�肵�����ԑт̊ԍ�Ƃ�������ƒB��
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Condition/TimeZone Condition"), Icon("Assets/Editor/scripts/Condition/Icons/TimeZoneCondition.png")]
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
            DateTime startDate = startTime.CreateDate();
            DateTime afterTime = startDate.AddSeconds(progressTime.ToSeconds());

            TimeSpan afterDifference = afterTime - DateTime.Now;
            //�������Ăق����ł��˂͂�
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
