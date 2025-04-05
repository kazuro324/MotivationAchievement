using UnityEngine;

///<summary>
///ビルドした回数が指定数超えると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Build Condition")]
    public class BuildCondition : AchievementCondition
    {
        [SerializeField] private bool isTotal;
        [SerializeField] private byte targetBuildCount;
        public override bool IsAchieved(AchievementDataManager data)
        {
            if (isTotal)
            {
                return data.TotalBuildCount >= targetBuildCount;
            }
            return data.CurrentBuildCount >= targetBuildCount;
        }
    }
}
