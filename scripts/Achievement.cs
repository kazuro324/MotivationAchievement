using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public enum DayCategoryType
    {
        CurrentSession = 3,
        Daily = 0,
        Weekly = 1,
        Total = 2
    }

    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/AchievementFile")]
    public class Achievement : ScriptableObject
    {
        [SerializeField] private string achievementName;
        [SerializeField, Multiline(2)] private string achievementDescription;
        [SerializeField] private DayCategoryType dayCategory;
        [SerializeField] private Texture2D notAchievementIcon;
        [SerializeField] private Texture2D icon;
        [SerializeField] private bool isHide;
        [SerializeField] private AchievementCondition[] achievementCondition;

        public bool IsAllAchieved(AchievementDataManager data)
        {
            foreach (var condition in achievementCondition)
            {
                if (!condition.IsAchieved(data))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetConditionAchievedCount(AchievementDataManager data)
        {
            int count = 0;
            foreach (var condition in achievementCondition)
            {
                if (condition.IsAchieved(data))
                {
                    count += (int)condition.GetCurrentConditionCount(data);
                }
            }
            return count;
        }

        public int AllConditionCount(AchievementDataManager data)
        {
            int count = 0;
            foreach (var condition in achievementCondition)
            {
                count += (int)condition.GetMaxConditionCount(data);
            }
            return count;
        }

        public bool IsHide {  get { return isHide; } }

        public string AchievementName { get { return achievementName; } }

        public string AchievementDescription { get { return achievementDescription; } }

        public Texture2D Icon { get { return icon; } }

        public Texture2D NotAchievementIcon { get { return notAchievementIcon; } }

        public DayCategoryType DayCategory { get { return dayCategory; } }
    }
}
