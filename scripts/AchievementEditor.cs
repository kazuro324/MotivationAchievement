using UnityEngine;
using UnityEditor;

namespace Kazuro.Editor.Achievement
{
    [CustomEditor(typeof(Achievement))]
    public class AchievementEditor : UnityEditor.Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            var obj = target as Achievement;

            if (obj == null || obj.Icon == null)
            {
                return base.RenderStaticPreview(assetPath, subAssets, width, height);
            }

            var texture = new Texture2D(width, height);

            EditorUtility.CopySerialized(obj.Icon, texture);

            return texture;
        }
    }
}