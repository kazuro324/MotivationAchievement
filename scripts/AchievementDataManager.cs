using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public struct TempData
    {
        public uint buildCount;
        public uint playCount;
        public uint currentWorkTime;
        public uint currentCompileCount;

        public TempData(uint buildCount, uint playCount, uint currentWorkTime, uint currentCompileCount)
        {
            this.buildCount = buildCount;
            this.playCount = playCount;
            this.currentWorkTime = currentWorkTime;
            this.currentCompileCount = currentCompileCount;
        }

        public static TempData New
        {
            get
            {
                return new TempData(0, 0, 0, 0);
            }
        }
    }

    public struct UserData
    {
        public int[] firstOpenDateArray;
        public int[] lastOpenDate;

        public uint currentContinueDays;

        public uint todayCompileCount;
        public uint currentWorkTime;
        public uint todayPlayCount;
        public uint todayWorkTime;
        public uint todayBootCount;
        public uint todayBuildCount;
        public uint todayFocusUnityEditorTime;
        public uint todayFocusCodeEditorTime;

        public uint weekCompileCount;
        public uint weekWorkTime;
        public uint weekBuildCount;
        public uint weekPlayModeCount;
        public uint weekBootCount;
        public uint weekBootDays;
        public uint weekContinueFirstDays;
        public uint weekFocusUnityEditorTime;
        public uint weekFocusCodeEditorTime;

        public uint highestContinueDays;
        public uint totalCompileCount;
        public uint totalWorkTime;
        public uint totalBuildCount;
        public uint totalPlayModeCount;
        public uint totalBootCount;
        public uint totalBootDays;
        public uint totalFocusUnityEditorTime;
        public uint totalFocusCodeEditorTime;

        public DateTime FirstOpenDate { get { return ArrayToDate(firstOpenDateArray); } }
        public DateTime LastOpenDate { get { return ArrayToDate(lastOpenDate); } }

        private enum Date
        {
            Year,
            Month,
            Day,
            Hour,
            Minute,
            Second
        }

        public static void DateToArray(ref int[] toArray, DateTime fromDate)
        {
            toArray = new int[6];
            toArray[(int)Date.Year] = fromDate.Year;
            toArray[(int)Date.Month] = fromDate.Month;
            toArray[(int)Date.Day] = fromDate.Day;
            toArray[(int)Date.Hour] = fromDate.Hour;
            toArray[(int)Date.Minute] = fromDate.Minute;
            toArray[(int)Date.Second] = fromDate.Second;
        }

        public static DateTime ArrayToDate(int[] toArray)
        {
            DateTime date = new DateTime(toArray[(int)Date.Year], toArray[(int)Date.Month], toArray[(int)Date.Day],
                toArray[(int)Date.Hour], toArray[(int)Date.Minute], toArray[(int)Date.Second]);
            return date;
        }

        public static UserData New
        {
            get
            {
                var newData = new UserData();
                DateToArray(ref newData.firstOpenDateArray, DateTime.Now);
                DateToArray(ref newData.lastOpenDate, DateTime.Today);
                newData.todayWorkTime = 0;
                newData.todayPlayCount = 0;
                newData.todayBuildCount = 0;
                newData.todayBootCount = 0;
                newData.todayCompileCount = 0;
                newData.todayFocusUnityEditorTime = 0;
                newData.todayFocusCodeEditorTime = 0;
                newData.weekBootCount = 0;
                newData.weekBuildCount = 0;
                newData.weekPlayModeCount = 0;
                newData.weekWorkTime = 0;
                newData.weekBootDays = 0;
                newData.weekCompileCount = 0;
                newData.weekFocusUnityEditorTime = 0;
                newData.weekFocusCodeEditorTime = 0;
                newData.weekContinueFirstDays = 0;
                newData.currentWorkTime = 0;
                newData.currentContinueDays = 0;
                newData.highestContinueDays = 0;
                newData.totalWorkTime = 0;
                newData.totalBuildCount = 0;
                newData.totalPlayModeCount = 0;
                newData.totalBootCount = 0;
                newData.totalCompileCount = 0;
                newData.totalFocusUnityEditorTime = 0;
                newData.totalFocusCodeEditorTime = 0;
                return newData;
            }
        }
    }

    public class AchievementDataManager : IDisposable
    {
        private UserData loadedData;
        private TempData tempData;

        const int ContinueDay = 1;

        public uint TodayWorkTime { get { return loadedData.todayWorkTime; } }

        public uint TodayPlayCount { get { return loadedData.todayPlayCount; } }

        public uint TodayBuildCount { get { return loadedData.todayBuildCount; } }

        public uint TodayCompileCount { get { return loadedData.totalCompileCount; } }

        public uint TodayFocusUnityEditorTime { get { return loadedData.todayFocusUnityEditorTime; } }

        public uint TodayFocusCodeEditorTime { get { return loadedData.todayFocusCodeEditorTime;  } }

        public uint PlayCount { get { return tempData.playCount; } }

        public uint CurrentCompileCount { get { return tempData.currentCompileCount; } }

        public uint CurrentBuildCount { get { return tempData.buildCount; } }

        public DateTime FirstStartDate { get { return loadedData.FirstOpenDate; } }
        public DateTime LastOpenDate { get { return loadedData.LastOpenDate; } }

        public uint CurrentContinueDays { get { return loadedData.currentContinueDays; } }

        public uint WeekCompileCount { get { return loadedData.weekCompileCount; } }

        public uint WeekWorkTime { get { return loadedData.weekWorkTime; } }
        public uint WeekPlayCount { get { return loadedData.weekPlayModeCount; } }
        public uint WeekBuildCount { get { return loadedData.weekBuildCount; } }
        public uint WeekBootCount { get { return loadedData.weekBootCount; } }

        public uint WeekFocusUnityEditorTime { get { return loadedData.weekFocusUnityEditorTime; } }

        public uint WeekFocusCodeEditorTime { get { return loadedData.weekFocusCodeEditorTime; } }


        public uint WeekBootDays { get { return loadedData.weekBootDays; } }

        public uint WeekContinueDays { get { return loadedData.currentContinueDays - loadedData.weekContinueFirstDays; } }

        public uint TotalCompileCount { get { return loadedData.totalCompileCount; } }

        public uint TotalBuildCount { get { return loadedData.totalBuildCount; } }

        public uint TotalBootCount { get { return loadedData.totalBootCount; } }
        public uint TodayBootCount { get { return loadedData.todayBootCount; } }

        public uint TotalPlayCount { get { return loadedData.totalPlayModeCount; } }

        public uint TotalWorkTime { get { return loadedData.totalWorkTime; } }

        public uint TotalFocusUnityEditorTime { get { return loadedData.totalFocusUnityEditorTime; } }

        public uint TotalFocusCodeEditorTime { get { return loadedData.totalFocusCodeEditorTime; } }


        public uint HighestContinueDays { get { return loadedData.highestContinueDays; } }

        private bool isBuilding = false;

        private bool isStartUp = true;

        private double previousUnityEditorUpdateTime;
        private double previousCodeEditorUpdateTime;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


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

            //別の日
            if (timeDifference.Days != 0)
            {
                //週間の管理
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                {
                    ResetWeeklyStats();
                }

                loadedData.weekBootDays++;
                loadedData.totalBootDays++;
                loadedData.todayBootCount = 0;
                loadedData.todayWorkTime = 0;
                loadedData.todayPlayCount = 0;
                loadedData.todayBuildCount = 0;
                loadedData.todayCompileCount = 0;
                loadedData.weekFocusUnityEditorTime = 0;
                loadedData.todayFocusCodeEditorTime = 0;

                
            }

            AchievementManager.Instance.OnUpdate += UpdateFocusEditorCount;
            AchievementManager.Instance.OnUpdate += CountBuild;
            EditorApplication.playModeStateChanged += CountPlayMode;
            CompilationPipeline.compilationStarted += CountCompile;
            EditorApplication.quitting += SaveData;
        }

        private void ResetWeeklyStats()
        {
            loadedData.weekWorkTime = 0;
            loadedData.weekPlayModeCount = 0;
            loadedData.weekBuildCount = 0;
            loadedData.weekBootCount = 0;
            loadedData.weekBootDays = 0;
            loadedData.weekCompileCount = 0;
            loadedData.weekFocusUnityEditorTime = 0;
            loadedData.weekFocusCodeEditorTime = 0;
            loadedData.weekContinueFirstDays = CurrentContinueDays;
        }

        public void TemporarySaveData()
        {
            tempData.currentWorkTime = (uint)EditorApplication.timeSinceStartup;

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
            loadedData.todayPlayCount++;
            loadedData.weekPlayModeCount++;
            loadedData.totalPlayModeCount++;
        }

        public void CountCompile(object obj)
        {
            tempData.currentCompileCount++;
            loadedData.weekCompileCount++;
            loadedData.totalCompileCount++;
        }

        private void CountBuild()
        {
            if (BuildPipeline.isBuildingPlayer)
            {
                if (!isBuilding)
                {
                    tempData.buildCount++;
                    loadedData.todayBuildCount++;
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

        private void UpdateFocusEditorCount()
        {
            double focusCodeEditorTime = 0d;
            double focusUnityEditorTime = 0d;

            if (CheckFocusUnityEditor())
            {
                focusUnityEditorTime = EditorApplication.timeSinceStartup;
                focusUnityEditorTime -= previousUnityEditorUpdateTime;
                loadedData.todayFocusUnityEditorTime += (uint)focusUnityEditorTime;
                loadedData.weekFocusUnityEditorTime += (uint)focusUnityEditorTime;
                loadedData.totalFocusUnityEditorTime += (uint)focusUnityEditorTime;
                previousUnityEditorUpdateTime = EditorApplication.timeSinceStartup;
            }

            if (CheckFocusCodeEditor())
            {
                focusCodeEditorTime = EditorApplication.timeSinceStartup;
                focusCodeEditorTime -= previousCodeEditorUpdateTime;
                loadedData.todayFocusCodeEditorTime += (uint)focusCodeEditorTime;
                loadedData.weekFocusCodeEditorTime += (uint)focusCodeEditorTime;
                loadedData.totalFocusCodeEditorTime += (uint)focusCodeEditorTime;
                previousCodeEditorUpdateTime = EditorApplication.timeSinceStartup;
            }
        }

        private bool CheckFocusUnityEditor()
        {
            return EditorApplication.isFocused;
        }

        private bool CheckFocusCodeEditor()
        {
            string editorPath = EditorPrefs.GetString("kScriptsDefaultApp");
            

            if (string.IsNullOrEmpty(editorPath) || !File.Exists(editorPath))
            {
                return false;
            }

            string editorExeName = Path.GetFileNameWithoutExtension(editorPath);

            IntPtr foregroundWindow = GetForegroundWindow();
            if (foregroundWindow == IntPtr.Zero)
            {
                return false;
            }

            GetWindowThreadProcessId(foregroundWindow, out uint processId);

            try
            {
                Process foregroundProcess = Process.GetProcessById((int)processId);
                var editorName = foregroundProcess.ProcessName;
                return string.Equals(editorName, editorExeName);
            }
            catch
            {
                return false;
            }
        }

        public void PrintInformation()
        {
            UnityEngine.Debug.Log($"Work Time: {EditorApplication.timeSinceStartup} TotalWorkTime: {loadedData.totalWorkTime}");
            UnityEngine.Debug.Log($"Play Count: {PlayCount} Total: {TotalPlayCount}");
            UnityEngine.Debug.Log($"Boot Today: {TodayBootCount} Boot Total: {TotalBootCount}");
            UnityEngine.Debug.Log($"LastOpenDay: {LastOpenDate} CurrentContinueDays: {CurrentContinueDays}");
            UnityEngine.Debug.Log($"Build: {CurrentBuildCount}");
            UnityEngine.Debug.Log($"UnityEditor: {TodayFocusUnityEditorTime}");
            UnityEngine.Debug.Log($"CodeEditor: {TodayFocusCodeEditorTime}");
        }

        public void Dispose()
        {
            AchievementManager.Instance.OnUpdate -= UpdateFocusEditorCount;
            AchievementManager.Instance.OnUpdate -= CountBuild;
            EditorApplication.playModeStateChanged -= CountPlayMode;
            CompilationPipeline.compilationStarted -= CountCompile;
        }
    }
}