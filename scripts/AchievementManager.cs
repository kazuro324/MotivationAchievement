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
        private List<Achievement> noAchievements = new List<Achievement>();
        private Queue<Achievement> removeAchievementQueue = new Queue<Achievement>();
        private static Queue<AchievementNotifyPopUp> visibleNotifyPopUpQueue = new Queue<AchievementNotifyPopUp>();

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
        public AchievementDataManager DataManager { get { return dataManager; } }


        //Editor�N�����ɓǂݍ���
        [InitializeOnLoadMethod]
        private static void InitializeInstance()
        {
            if (Instance.dataBase == null)
            {
                Debug.LogWarning("AchievementDataBase���ݒ肳��Ă��܂���B");
                return;
            }
            var token = Instance.cancellationTokenSource.Token;

            Instance.dataManager = new AchievementDataManager();

            // ���B���̎��т����X�g�A�b�v&�I���܂őҋ@
            Instance.CheckAchieved();

            //EditorApplication.update += Instance.CheckClaimAchieved;
            CompilationPipeline.compilationStarted += Instance.StartCompile;

            Debug.Log($"���т�ǂݍ��݂܂����B�o�^���ѐ�: {Instance.AchievementCount}");

            Instance.CheckCanAchievementGrant(token);
        }

        private void CheckAchieved()
        {
            foreach (var achievement in Instance.dataBase.Achievements)
            {
                if (achievement == null)
                {
                    Debug.LogWarning($"{achievement.name}�͐���ɓǂݍ��߂܂���ł����B");
                    continue;
                }

                //IsAllAchieved���ł̗�O��ߑ�
                try
                {
                    if (!achievement.IsAllAchieved(Instance.dataManager))
                    {
                        Instance.noAchievements.Add(achievement);
                    }
                }
                catch (NullReferenceException nre)
                {
                    Debug.LogError($"Achievement '{achievement.name}' ��NullReferenceException���������܂���: {nre.Message}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Achievement '{achievement.name}' �ŗ�O���������܂���: {ex.Message}");
                }
            }
        }

        private async void CheckCanAchievementGrant(CancellationToken token)
        {
            while (true)
            {
                Instance.CheckClaimAchieved();
                await Task.Delay(1000, token);
            }
        }

        private static void EndClass()
        {
            EditorApplication.update -= Instance.CheckClaimAchieved;
            CompilationPipeline.compilationStarted -= Instance.StartCompile;
        }

        private void StartCompile(object obj)
        {
            EndClass();
            dataManager.TemporarySaveData();
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

            Debug.Log("Death Manager");
        }

        [MenuItem("Achievement/View")]
        public static void View()
        {
            Instance.dataManager.PrintInformation();
        }

        [MenuItem("Achievement/Reload")]
        public static void Reload()
        {
            Instance.noAchievements.Clear();
            Instance.removeAchievementQueue.Clear();
            Instance.achieveReferenceList.Clear();
            Instance.CheckAchieved();
        }

        [MenuItem("Achievement/Restart")]
        public static void Restart()
        {
            instance = null;
            InitializeInstance();
        }
    }
}
