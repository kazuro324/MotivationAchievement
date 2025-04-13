using System;
using UnityEditor;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public class AchievementDataManager
    {
        private UserData loadedData;

        private uint weekWorkTime = 0;
        private uint totalWorkTime = 0;

        private int playCount = 0;
        private int weekPlayCount = 0;
        private int totalPlayCount = 0;

        private DateTime lastOpenDay;
        private DateTime firstStartDate;
        private uint weekContinueFirstDays = 0;
        private uint currentContinueDays = 0;
        private uint highestContinueDays = 0;

        private int currentBuildCount = 0;
        private int weekBuildCount = 0;
        private int totalBuildCount = 0;

        private int todayBootCount = 0;
        private int weekBootCount = 0;
        private int totalBootCount = 0;

        private int weekBootDays = 0;

        const int ContinueDay = 1;

        public int PlayCount { get { return playCount; } }

        public int CurrentBuildCount { get { return currentBuildCount; } }

        public DateTime FirstStartDate { get { return firstStartDate; } }

        public int TotalBuildCount { get { return totalBuildCount; } }

        public int TotalBootCount { get { return totalBootCount; } }
        public int TodayBootCount { get { return todayBootCount; } }

        public int TotalPlayCount { get { return totalPlayCount; } }

        public uint TotalWorkTime { get { return totalWorkTime; } }

        public uint CurrentContinueDays { get { return currentContinueDays; } }

        public uint WeekWorkTime { get { return weekWorkTime; } }
        public int WeekPlayCount { get { return weekPlayCount; } }
        public int WeekBuildCount { get { return weekBuildCount; } }
        public int WeekBootCount { get { return weekBootCount; } }

        public int WeekBootDays { get { return weekBootDays; } }

        public uint WeekContinueDays { get { return currentContinueDays - weekContinueFirstDays; } }

        public uint HighestContinueDays { get { return highestContinueDays; } }

        private bool isBuilding = false;

        public AchievementDataManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            TemporaryLoadData();
            LoadData();

            //連日起動の確認
            if (lastOpenDay.AddDays(ContinueDay) == DateTime.Today)
            {
                currentContinueDays++;
                if (currentContinueDays > highestContinueDays)
                {
                    highestContinueDays = currentContinueDays;
                }
            }
            else
            {
                if (lastOpenDay != DateTime.Today)
                {
                    currentContinueDays = 0;
                }
            }

            if (lastOpenDay != DateTime.Today)
            {
                weekBootDays++;
            }

            //週間の管理
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday && lastOpenDay != DateTime.Today)
            {
                weekWorkTime = 0;
                weekPlayCount = 0;
                weekBuildCount = 0;
                weekBootCount = 0;
                weekBootDays = 0;
                weekContinueFirstDays = CurrentContinueDays;
            }

            EditorApplication.playModeStateChanged += CountPlayMode;
            EditorApplication.quitting += SaveData;
        }

        public void TemporarySaveData()
        {
            TempData tempData = new TempData(CurrentBuildCount, PlayCount, TodayBootCount);

            AchievementUserDataLoader.TemporarySaveData(tempData);
        }

        private void TemporaryLoadData()
        {
            TempData tempData = AchievementUserDataLoader.TemporaryLoadData();
            todayBootCount = tempData.bootCount;
            currentBuildCount = tempData.buildCount;
            playCount = tempData.playCount;
        }

        private void LoadData()
        {
            UserData loadData = AchievementUserDataLoader.LoadData();

            todayBootCount += loadData.todayBootCount;
            weekBootCount = loadData.weekBootCount;
            weekBuildCount = loadData.weekBuildCount;
            weekPlayCount = loadData.weekPlayModeCount;
            weekWorkTime = loadData.weekWorkTime;

            totalBootCount = loadData.totalBootCount;
            totalBuildCount = loadData.totalBuildCount;
            totalPlayCount = loadData.totalPlayModeCount;
            totalWorkTime = loadData.totalWorkTime;

            currentContinueDays = loadData.currentContinueDays;
            highestContinueDays = loadData.highestContinueDays;

            firstStartDate = UserData.ArrayToDate(loadData.firstOpenDateArray);
            lastOpenDay = UserData.ArrayToDate(loadData.lastOpenDate);
            loadedData = loadData;
        }

        public void SaveData()
        {
            loadedData.weekBuildCount = WeekBuildCount + CurrentBuildCount;
            loadedData.weekPlayModeCount = WeekPlayCount + PlayCount;
            loadedData.weekBootCount = WeekBootCount + TodayBootCount;
            loadedData.weekWorkTime += (uint)EditorApplication.timeSinceStartup;
            loadedData.weekBootDays = WeekBootDays;
            loadedData.weekContinueFirstDays = weekContinueFirstDays;
            loadedData.highestContinueDays = HighestContinueDays;

            loadedData.totalBuildCount = TotalBuildCount + CurrentBuildCount;
            loadedData.totalPlayModeCount = TotalPlayCount + PlayCount;
            loadedData.todayBootCount = TodayBootCount;
            loadedData.totalBootCount = TotalBootCount;
            loadedData.totalWorkTime += (uint)EditorApplication.timeSinceStartup;
            loadedData.currentContinueDays = CurrentContinueDays;
            UserData.DateToArray(ref loadedData.lastOpenDate, DateTime.Today);

            AchievementUserDataLoader.SaveData(loadedData);
        }

        private void CountPlayMode(PlayModeStateChange stateChange)
        {
            if (stateChange != PlayModeStateChange.EnteredPlayMode)
            {
                return;
            }
            playCount++;
        }

        private void CountBuild()
        {
            if (BuildPipeline.isBuildingPlayer)
            {
                if (!isBuilding)
                {
                    currentBuildCount++;
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
            UnityEngine.Debug.Log($"Work Time: {EditorApplication.timeSinceStartup} TotalWorkTime: {totalWorkTime}");
            UnityEngine.Debug.Log($"Play Count: {PlayCount} Total: {TotalPlayCount}");
            UnityEngine.Debug.Log($"Boot Today: {TodayBootCount} Boot Total: {TotalBootCount}");
            UnityEngine.Debug.Log($"LastOpenDay: {lastOpenDay} CurrentContinueDays: {CurrentContinueDays}");
            UnityEngine.Debug.Log($"Boot Total: {TotalBootCount}");
            UnityEngine.Debug.Log($"lastOpenDay: {lastOpenDay}");
        }
    }
}