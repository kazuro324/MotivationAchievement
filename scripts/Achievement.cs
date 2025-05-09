﻿using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public enum CategoryType
    {
        Daily,
        Weekly,
        Total
    }

    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/AchievementFile")]
    public class Achievement : ScriptableObject
    {
        [SerializeField] private string achievementName;
        [SerializeField, Multiline(2)] private string achievementDescription;
        [SerializeField] private CategoryType category;
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
                    count++;
                }
            }
            return count;
        }

        public bool IsHide {  get { return isHide; } }

        public string AchievementName { get { return achievementName; } }

        public string AchievementDescription { get { return achievementDescription; } }

        public int AllConditionCount {  get { return achievementCondition.Length; } }

        public Texture2D Icon { get { return icon; } }

        public Texture2D NotAchievementIcon { get { return notAchievementIcon; } }

        public CategoryType Category { get { return category; } }
    }
}
