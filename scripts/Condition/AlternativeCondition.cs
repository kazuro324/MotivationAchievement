using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public class AlternativeCondition : AchievementCondition
    {
        [SerializeField] private AchievementCondition[] alternativeList;

        public override uint GetCurrentConditionCount(AchievementDataManager data)
        {
            return IsAchieved(data) ? 1u : 0u;
        }

        public override uint GetMaxConditionCount(AchievementDataManager data)
        {
            return 1u;
        }

        public override bool IsAchieved(AchievementDataManager data)
        {
            foreach (var condition in alternativeList)
            {
                if (condition.IsAchieved(data))
                {
                    return true;
                }
            }
            return false;
        }
    }
}