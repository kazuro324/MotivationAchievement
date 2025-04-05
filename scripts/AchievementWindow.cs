using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kazuro.Editor.Achievement
{
    public class AchievementWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset achievementWindowTree;
        [SerializeField] private VisualTreeAsset achievementBoxTree;

        private static AchievementWindow currentOpenWindow;

        private List<Tab> categoryTabs;
        private ScrollView[] achievementContainers;

        readonly Color invisibleProgressColor = new Color(0.1882353f, 0.1882353f, 0.1882353f, 1f);
        readonly Color achievedProgressColor = new Color(0.172549f, 0.3647059f, 0.5294118f, 1f);

        [MenuItem("Achievement/Open Window")]
        public static void OpenWindow()
        {
            //表示中ならフォーカス
            if (currentOpenWindow != null)
            {
                currentOpenWindow.Focus();
                return;
            }

            //ウィンドウ生成
            currentOpenWindow = CreateInstance<AchievementWindow>();
            currentOpenWindow.titleContent = new GUIContent("Achievement");

            currentOpenWindow.Show();
        }

        private void CreateGUI()
        {
            achievementWindowTree.CloneTree(this.rootVisualElement);
            UpdateProgressBar();
            var tabView = rootVisualElement.Q<TabView>("CategoryTabBar");
            categoryTabs = tabView.Query<Tab>().ToList();
            achievementContainers = new ScrollView[categoryTabs.Count];
            for (int i = 0; i < categoryTabs.Count; i++)
            {
                achievementContainers[i] = categoryTabs[i].Q<ScrollView>("AchievementContainer");
            }
            
            //個々の実績を表示
            foreach (var achievement in AchievementManager.Instance.GetAchievementDataBase)
            {
                GenerateAchievementElement(achievement);
            }
        }
        private void OnGUI()
        {
            UpdateProgressBar();
            var tabView = rootVisualElement.Q<TabView>("CategoryTabBar");
            var tabIndex = categoryTabs.IndexOf(tabView.activeTab);

            if (achievementContainers != null)
            {
                //個々の実績表示を更新
                for (int i = 0; i < achievementContainers[tabIndex].childCount; i++)
                {
                    var achievement = AchievementManager.Instance.GetAchieveReferenceList(achievementContainers[tabIndex][i] as GroupBox);

                    UpdateBoxVisual(achievementContainers[tabIndex][i] as GroupBox, achievement);
                }
            }
        }

        private void UpdateProgressBar()
        {
            var allSatisfiedProgressBar = rootVisualElement.Q<ProgressBar>("AllSatisfiedProgressBar");

            allSatisfiedProgressBar.value = AchievementManager.Instance.GetSatisfiedCount();
            allSatisfiedProgressBar.highValue = AchievementManager.Instance.AchievementCount;
            allSatisfiedProgressBar.lowValue = 0;
            allSatisfiedProgressBar.title = allSatisfiedProgressBar.value + " / " + AchievementManager.Instance.AchievementCount;
        }

        private void GenerateAchievementElement(Achievement achievement)
        {
            //非表示で未取得であれば非表示
            if (achievement.IsHide && !AchievementManager.Instance.IsAchieved(achievement))
            {
                return;
            }

            achievementBoxTree.CloneTree(this.rootVisualElement);
            var box = rootVisualElement.Query<GroupBox>("AchievementBox").NotVisible().First();
            achievementContainers[(int)achievement.Category].Add(box);
            AchievementManager.Instance.AddAchieveReferenceList(box, achievement);
            UpdateBoxVisual(box, achievement);
        }

        private void UpdateBoxVisual(GroupBox box, Achievement achievement)
        {
            box.visible = true;

            var informationContainer = box.Q<VisualElement>("InformationContainer");
            var title = informationContainer.Q<Label>("AchievementTitle");
            var description = informationContainer.Q<Label>("AchievementDescription");
            var boxImage = box.Q<Image>("AchievementIcon");

            title.text = achievement.AchievementName;
            description.text = achievement.AchievementDescription;

            //進捗表示
            int currentAchieved = achievement.GetConditionAchievedCount(AchievementManager.Instance.DataManager);
            int maxAchieved = achievement.AllConditionCount;
            var achieveProgressBar = informationContainer.Q<ProgressBar>("SatisfiedProgressBar");
            achieveProgressBar.lowValue = 0;
            achieveProgressBar.highValue = maxAchieved;
            achieveProgressBar.value = currentAchieved;
            achieveProgressBar.title = currentAchieved + " / " + maxAchieved;

            var progressFill = achieveProgressBar.Q(className: "unity-progress-bar__progress");

            //全ての進捗を達成すると色変化
            if (AchievementManager.Instance.IsAchieved(achievement))
            {
                boxImage.image = achievement.Icon;
                boxImage.tintColor = Color.white;
                progressFill.style.backgroundColor = achievedProgressColor;
            }
            else
            {
                boxImage.image = achievement.NotAchievementIcon;
                boxImage.tintColor = Color.white;
                if (achievement.GetConditionAchievedCount(AchievementManager.Instance.DataManager) <= 0)
                {
                    progressFill.style.backgroundColor = invisibleProgressColor;
                    return;
                }
                progressFill.style.backgroundColor = Color.red;
            }
        }
    }
}
