using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kazuro.Editor.Achievement
{
    public class AchievementNotifyPopUp : EditorWindow
    {
        [SerializeField] private VisualTreeAsset notificationBoxTree;

        private Achievement holdAchievement;

        const float visibleTime = 5f;
        const int MicroSecond = 1000;

        public static float PopUpWidth { get { return 300f; } }

        public static float PopUpHeight { get { return 80f; } }

        public void SetUp(Achievement achievement, float height)
        {
            holdAchievement = achievement;
            position = new Rect(Screen.currentResolution.width - PopUpWidth, height, PopUpWidth, PopUpHeight);
        }
    
        public void ShowWindow()
        {
            ShowPopup();
            var task = CloseWindow();
        }

        private async Task CloseWindow()
        {
            await Task.Run(() => Thread.Sleep((int)visibleTime * MicroSecond));

            AchievementManager.Instance.RemoveVisibleNotifyPopUp();
            Close();
        }

        private void CreateGUI()
        {
            notificationBoxTree.CloneTree(this.rootVisualElement);
            var box = rootVisualElement.Q<GroupBox>("AchievementNotifyPopUpBox");
            box.Q<Image>("AchievementIcon").image = holdAchievement.Icon;

            var informationContainer = box.Q<VisualElement>("InformationContainer");
            var title = informationContainer.Q<Label>("AchievementTitle");
            var description = informationContainer.Q<Label>("AchievementDescription");

            title.text = holdAchievement.AchievementName;
            description.text = holdAchievement.AchievementDescription;
        }
    }
}
