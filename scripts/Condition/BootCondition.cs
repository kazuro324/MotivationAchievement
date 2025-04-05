using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Boot Condition")]
    public class BootCondition : AchievementCondition
    {
        [SerializeField] private bool isTotal;
        [SerializeField] private byte targetBootCount;
        public override bool IsAchieved(AchievementDataManager data)
        {
            if (isTotal)
            {
                return data.TotalBootCount > targetBootCount;
            }
            return data.TodayBootCount > targetBootCount;
        }
    }
}
