using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.Threading.Tasks;
using System.Threading;

namespace Kazuro.Editor.Achievement
{
    public class AchievementManager : EditorWindow, IDisposable
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private static AchievementManager instance;
        [SerializeField] private AchievementDataBase dataBase;
        private AchievementDataManager dataManager;
        private Dictionary<UnityEngine.UIElements.GroupBox, Achievement> achieveReferenceList = new Dictionary<UnityEngine.UIElements.GroupBox, Achievement>();
        private HashSet<Achievement> noAchievements = new HashSet<Achievement>();
        private Queue<Achievement> removeAchievementQueue = new Queue<Achievement>();
        private static Queue<AchievementNotifyPopUp> visibleNotifyPopUpQueue = new Queue<AchievementNotifyPopUp>();


        public event Action OnUpdate;
        public int AchievementCount { get { return dataBase.Achievements.Length; } }
        public Achievement[] GetAchievementDataBase { get { return dataBase.Achievements; } }
        public static AchievementManager Instance 
        { 
            get 
            {
                if (instance == null)
                {
                    instance = CreateInstance<AchievementManager>();
                }
                return instance;
            }
        }
        public AchievementDataManager DataManager
        {
            get
            {
                if (dataManager == null)
                {
                    dataManager = new AchievementDataManager();
                }
                return dataManager;
            }
        }


        //Editor起動時に読み込み
        [InitializeOnLoadMethod]
        private static void InitializeInstance()
        {
            if (Instance.dataBase == null)
            {
                Debug.LogWarning("AchievementDataBaseが設定されていません。");
                return;
            }
            var token = Instance.cancellationTokenSource.Token;

            Instance.dataManager = new AchievementDataManager();

            // 未達成の実績をリストアップ&終わるまで待機
            Instance.CheckAchieved();

            CompilationPipeline.compilationStarted += Instance.StartCompile;
            Instance.OnUpdate += Instance.CheckClaimAchieved;

            Instance.UpdateAsync(token);
        }

        private void CheckAchieved()
        {
            int count = 0;
            foreach (var achievement in Instance.dataBase.Achievements)
            {
                if (achievement == null)
                {
                    Debug.LogWarning($"{achievement.name}は正常に読み込めませんでした。");
                    continue;
                }

                //IsAllAchieved内での例外を捕捉
                try
                {
                    if (!achievement.IsAllAchieved(Instance.DataManager))
                    {
                        Instance.noAchievements.Add(achievement);
                        count++;
                    }
                }
                catch (NullReferenceException nre)
                {
                    Debug.LogError($"Achievement '{achievement.name}' でNullReferenceExceptionが発生しました: {nre.Message}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Achievement '{achievement.name}' で例外が発生しました: {ex.Message}");
                }
            }

            Debug.Log($"実績を読み込みました。登録実績数: {count}中 {Instance.AchievementCount}");

        }

        private async void UpdateAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                OnUpdate?.Invoke();
                await Task.Delay(1000, token);
            }
        }

        private static void EndClass()
        {
            Instance.OnUpdate -= Instance.CheckClaimAchieved;
            CompilationPipeline.compilationStarted -= Instance.StartCompile;
        }

        private void StartCompile(object obj)
        {
            EndClass();
            DataManager.TemporarySaveData();
            DataManager.SaveData();
        }

        public bool IsAchieved(Achievement achievement)
        {
            return !noAchievements.Contains(achievement);
        }

        public void CheckClaimAchieved()
        {
            foreach (var achievement in noAchievements)
            {
                if (achievement.IsAllAchieved(Instance.DataManager))
                {
                    removeAchievementQueue.Enqueue(achievement);
                    GeneratePopUp(achievement);
                }
            }
            DeleteRemoveQueue();
        }

        public static void GeneratePopUp(Achievement achievement)
        {
            var notifyPopUp = CreateInstance<AchievementNotifyPopUp>();
            notifyPopUp.SetUp(achievement,
                Screen.currentResolution.height - ((visibleNotifyPopUpQueue.Count) * AchievementNotifyPopUp.PopUpHeight));
            notifyPopUp.ShowWindow();
            visibleNotifyPopUpQueue.Enqueue(notifyPopUp);
        }

        public void AddAchieveReferenceList(UnityEngine.UIElements.GroupBox box, Achievement achievement)
        {
            achieveReferenceList.Add(box, achievement);
        }

        public Achievement GetAchieveReferenceList(UnityEngine.UIElements.GroupBox box)
        {
            return achieveReferenceList[box];
        }

        public void RemoveVisibleNotifyPopUp()
        {
            if (visibleNotifyPopUpQueue.Count <= 0)
            {
                return;
            }
            visibleNotifyPopUpQueue.Dequeue();

            foreach (var box in visibleNotifyPopUpQueue)
            {
                box.position = new Rect(box.position.x, box.position.y + AchievementNotifyPopUp.PopUpHeight,
                    AchievementNotifyPopUp.PopUpWidth, AchievementNotifyPopUp.PopUpHeight);
            }
        }

        private void DeleteRemoveQueue()
        {
            while(removeAchievementQueue.Count > 0)
            {
                var queue = removeAchievementQueue.Dequeue();
                noAchievements.Remove(queue);
            }
        }

        public int GetSatisfiedCount()
        {
            int count = 0;
            foreach (var achievement in dataBase.Achievements)
            {
                if (IsAchieved(achievement))
                {
                    count++;
                }
            }
            return count;
        }

        public void Dispose()
        {
            EndClass();

            Instance.noAchievements.Clear();
            Instance.removeAchievementQueue.Clear();
            Instance.achieveReferenceList.Clear();

            cancellationTokenSource.Cancel();

            cancellationTokenSource.Dispose();
        }

        [MenuItem("Achievement/View")]
        public static void View()
        {
            Instance.DataManager.PrintInformation();
        }

        [MenuItem("Achievement/Reload")]
        public static void Reload()
        {
            Instance.noAchievements.Clear();
            Instance.removeAchievementQueue.Clear();
            Instance.achieveReferenceList.Clear();
            Instance.CheckAchieved();
        }
    }
}
