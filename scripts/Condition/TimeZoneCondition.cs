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
