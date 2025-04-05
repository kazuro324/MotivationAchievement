using UnityEngine;

///<summary>
///ビルドした回数が指定数超えると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Build Condition")]
    public class BuildCondition : AchievementCondition
    {
        [SerializeField] private byte targetBuildCount;
        public override bool IsAchieved(AchievementDataManager data)
        {
            return data.CurrentBuildCount > targetBuildCount;
        }
    }
}
