using UnityEditor;
using UnityEngine;

///<summary>
///EditorでPlayModeにした回数が指定数を超えると達成
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Play Condition")]
    public class PlayCondition : AchievementCondition
    {
        [SerializeField] private int playCount;

        public override bool IsAchieved(AchievementDataManager data)
        {
            return data.PlayCount >= playCount;
        }
    }
}
