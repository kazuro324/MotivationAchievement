using UnityEditor;
using UnityEngine;

///<summary>
///Editor‚ÅPlayMode‚É‚µ‚½‰ñ”‚ªw’è”‚ğ’´‚¦‚é‚Æ’B¬
///</summary>
namespace Kazuro.Editor.Achievement
{
    [CreateAssetMenu(menuName = "Kazuro/Editor/Achievement/Play Condition")]
    public class PlayCondition : AchievementCondition
    {
        [SerializeField] private bool isTotal;
        [SerializeField] private int playCount;

        public override bool IsAchieved(AchievementDataManager data)
        {
            if (isTotal)
            {
                return data.TotalPlayCount >= playCount;
            }
            return data.PlayCount >= playCount;
        }
    }
}
