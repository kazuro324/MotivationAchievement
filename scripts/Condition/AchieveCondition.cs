using UnityEngine;

///<summary>
///他のコンディションを満たすと達成
////</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Achieve Condition")]
    public class AchieveCondition : AchievementCondition
    {
        [SerializeField] private AchievementCondition[] conditions;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            uint count = 0;
            foreach (var condition in conditions)
            {
                if (condition.IsAchieved(data))
                {
                    count++;
                }
            }
            return count;
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return (uint)conditions.Length;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsAchieved(data))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
