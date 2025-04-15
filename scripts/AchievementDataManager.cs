using System;
using UnityEditor;

namespace Kazuro.Editor.Achievement
{
    public class AchievementDataManager : IDisposable
    {
        private UserData loadedData;
        private TempData tempData;

        const int ContinueDay = 1;

        public int PlayCount { get { return tempData.playCount; } }

        public int CurrentBuildCount { get { return tempData.buildCount; } }

        public DateTime FirstStartDate { get { return loadedData.FirstOpenDate; } }
        public DateTime LastOpenDate { get { return loadedData.LastOpenDate; } }

        public int TotalBuildCount { get { return loadedData.totalBuildCount; } }

        public int TotalBootCount { get { return loadedData.totalBootCount; } }
        public int TodayBootCount { get { return loadedData.todayBootCount; } }

        public int TotalPlayCount { get { return loadedData.totalPlayModeCount; } }

        public uint TotalWorkTime { get { return loadedData.totalWorkTime; } }

        public uint CurrentContinueDays { get { return loadedData.currentContinueDays; } }

        public uint WeekWorkTime { get { return loadedData.weekWorkTime; } }
        public int WeekPlayCount { get { return loadedData.weekPlayModeCount; } }
        public int WeekBuildCount { get { return loadedData.weekBuildCount; } }
        public int WeekBootCount { get { return loadedData.weekBootCount; } }

        public int WeekBootDays { get { return loadedData.weekBootDays; } }

        public uint WeekContinueDays { get { return loadedData.currentContinueDays - loadedData.weekContinueFirstDays; } }

        public uint HighestContinueDays { get { return loadedData.highestContinueDays; } }

        private bool isBuilding = false;

        private bool isStartUp = true;

        public AchievementDataManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            TemporaryLoadData();
            LoadData();

            TimeSpan timeDifference = DateTime.Now - loadedData.LastOpenDate;

            if (isStartUp)
            {
                loadedData.todayBootCount++;
                loadedData.weekBootCount++;
                loadedData.totalBootCount++;
            }

            //連日起動の確認
            if (timeDifference.Days == ContinueDay)
            {
                loadedData.currentContinueDays++;
                loadedData.weekContinueFirstDays++;
                loadedData.highestContinueDays = Math.Max(CurrentContinueDays, HighestContinueDays);
            }
            else if (timeDifference.Days > ContinueDay)
            {
                loadedData.currentContinueDays = 0;
                loadedData.weekContinueFirstDays = 0;
            }

            if (timeDifference.Days != 0)
            {
                loadedData.weekBootDays++;
                loadedData.totalBootDays++;
                loadedData.todayBootCount = 0;
                loadedData.todayWorkTime = 0;

                //週間の管理
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                {
                    ResetWeeklyStats();
                }
            }

            AchievementManager.Instance.OnUpdate += CountBuild;
            EditorApplication.playModeStateChanged += CountPlayMode;
            EditorApplication.quitting += SaveData;
        }

        private void ResetWeeklyStats()
        {
            loadedData.weekWorkTime = 0;
            loadedData.weekPlayModeCount = 0;
            loadedData.weekBuildCount = 0;
            loadedData.weekBootCount = 0;
            loadedData.weekBootDays = 0;
            loadedData.weekContinueFirstDays = CurrentContinueDays;
        }

        public void TemporarySaveData()
        {
            TempData tempData = new TempData(CurrentBuildCount, PlayCount, (uint)EditorApplication.timeSinceStartup);

            AchievementUserDataLoader.TemporarySaveData(tempData);
        }

        private void TemporaryLoadData()
        {
            TempData tempData = AchievementUserDataLoader.TemporaryLoadData(out isStartUp);

            this.tempData = tempData;
        }

        private void LoadData()
        {
            UserData loadData = AchievementUserDataLoader.LoadData();

            loadedData = loadData;
        }

        public void SaveData()
        {
            loadedData.todayWorkTime += (uint)EditorApplication.timeSinceStartup - loadedData.currentWorkTime;
            loadedData.weekWorkTime += (uint)EditorApplication.timeSinceStartup - loadedData.currentWorkTime;
            loadedData.totalWorkTime += (uint)EditorApplication.timeSinceStartup - loadedData.currentWorkTime;
            loadedData.currentWorkTime = (uint)EditorApplication.timeSinceStartup;
            UserData.DateToArray(ref loadedData.lastOpenDate, DateTime.Today);

            AchievementUserDataLoader.SaveData(loadedData);
        }

        private void CountPlayMode(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.EnteredPlayMode)
            {
                return;
            }
            tempData.playCount++;
            loadedData.weekPlayModeCount++;
            loadedData.totalPlayModeCount++;
        }

        private void CountBuild()
        {
            if (BuildPipeline.isBuildingPlayer)
            {
                if (!isBuilding)
                {
                    tempData.buildCount++;
                    loadedData.weekBuildCount++;
                    loadedData.totalBuildCount++;
                    isBuilding = true;
                }
            }else
            {
                if (isBuilding)
                {
                    isBuilding = false;
                }
            }
        }

        public void PrintInformation()
        {
            UnityEngine.Debug.Log($"Work Time: {EditorApplication.timeSinceStartup} TotalWorkTime: {loadedData.totalWorkTime}");
            UnityEngine.Debug.Log($"Play Count: {PlayCount} Total: {TotalPlayCount}");
            UnityEngine.Debug.Log($"Boot Today: {TodayBootCount} Boot Total: {TotalBootCount}");
            UnityEngine.Debug.Log($"LastOpenDay: {LastOpenDate} CurrentContinueDays: {CurrentContinueDays}");
            UnityEngine.Debug.Log($"Build: {CurrentBuildCount}");
        }

        public void Dispose()
        {
            AchievementManager.Instance.OnUpdate -= CountBuild;
        }
    }
}