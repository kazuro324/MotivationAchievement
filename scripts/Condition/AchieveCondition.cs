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
