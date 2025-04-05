using UnityEngine;

///<summary>
///���т̒B�������̊��N���X
///</summary>
namespace Kazuro.Editor.Achievement
{
    public abstract class AchievementCondition : ScriptableObject
    {
        public enum DayCategory
        {
            Daily,
            Weekly,
            Total
        }

        public abstract bool IsAchieved(AchievementDataManager data);
    }
}
