using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
    public struct TempData
    {
        public int buildCount;
        public int playCount;
        public uint currentWorkTime;

        public TempData(int buildCount, int playCount, uint currentWorkTime)
        {
            this.buildCount = buildCount;
            this.playCount = playCount;
            this.currentWorkTime = currentWorkTime;
        }

        public static TempData New
        {
            get
            {
                var newData = new TempData();
                newData.buildCount = 0;
                newData.playCount = 0;
                newData.currentWorkTime = 0;
                return newData;
            }
        }
    }

    public struct UserData
    {
        public int[] firstOpenDateArray;
        public int[] lastOpenDate;

        public uint currentContinueDays;

        public uint currentWorkTime;
        public uint todayWorkTime;
        public int todayBootCount;

        public uint weekWorkTime;
        public int weekBuildCount;
        public int weekPlayModeCount;
        public int weekBootCount;
        public int weekBootDays;
        public uint weekContinueFirstDays;

        public uint highestContinueDays; 
        public uint totalWorkTime;
        public int totalBuildCount;
        public int totalPlayModeCount;
        public int totalBootCount;
        public int totalBootDays;

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
                newData.weekBootCount = 0;
                newData.weekBuildCount = 0;
                newData.weekPlayModeCount = 0;
                newData.weekWorkTime = 0;
                newData.weekBootDays = 0;
                newData.weekContinueFirstDays = 0;
                newData.currentWorkTime = 0;
                newData.currentContinueDays = 0;
                newData.highestContinueDays = 0;
                newData.totalWorkTime = 0;
                newData.totalBuildCount = 0;
                newData.totalPlayModeCount = 0;
                newData.totalBootCount = 0;
                newData.todayBootCount = 0;
                return newData;
            }
        }
    }

    public class AchievementUserDataLoader
    {
        const string dataFolder = "Assets/Editor/Save/";
        const string saveFilePath = dataFolder + "UserData.json";
        const string alternativeSaveFilePath = dataFolder + "AlternativeUserData.json";
        const string alternativeSaveMetaPath = dataFolder + "AlternativeUserData.json.meta";
        const string temporarySaveFilePath = dataFolder + "TempData.json";
        const string temporarySaveMetaPath = dataFolder + "TempData.json.meta";
        const int EmptySearchLength = 6;

        public static void TemporarySaveData(TempData data)
        {
            var jsonData = JsonUtility.ToJson(data);
            var writer = new StreamWriter(temporarySaveFilePath, false);
            writer.Write(jsonData);
            writer.Close();
            AssetDatabase.Refresh();
        }

        private static bool IsSaveDirectory() => File.Exists(dataFolder);

        public static void DeleteTemporaryData(out bool isStartUp)
        {
            isStartUp = true;
            if (File.Exists(temporarySaveFilePath))
            {
                isStartUp = false;
                File.Delete(temporarySaveFilePath);
                File.Delete(temporarySaveMetaPath);
                AssetDatabase.Refresh();
            }
        }

        public static void DeleteAlternativeData()
        {
            if (File.Exists(alternativeSaveFilePath))
            {
                File.Delete(alternativeSaveFilePath);
                File.Delete(alternativeSaveMetaPath);
                AssetDatabase.Refresh();
            }
        }


        public static TempData TemporaryLoadData(out bool isStartUp)
        {
            AssetDatabase.Refresh();

            //一時保存ファイルがある場合読み込み
            try
            {
                var tmpAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(temporarySaveFilePath);
                var tmpData = (TempData)JsonUtility.FromJson(tmpAsset.text, typeof(TempData));

                DeleteTemporaryData(out isStartUp);
                return tmpData;
            }
            catch
            {
                isStartUp = true;
                return TempData.New;
            }
        }

        public static void SaveData(UserData data)
        {
            var jsonData = JsonUtility.ToJson(data);

            if (!IsSaveDirectory())
            {
                Directory.CreateDirectory(dataFolder);
            }

            try
            {
                var writer = new StreamWriter(saveFilePath, false);
                writer.Write(jsonData);
                writer.Close();
            }
            catch
            {
                var writer = new StreamWriter(alternativeSaveFilePath, false);
                writer.Write(jsonData);
                writer.Close();
            }
            finally
            {
                AssetDatabase.Refresh();
            }
        }

        public static UserData LoadData()
        {
            UserData loadData = UserData.New;

            if (!IsSaveDirectory())
            {
                Directory.CreateDirectory(dataFolder);
            }

            try
            {
                //前回の終了時にデータが正常に保存できなかった場合
                var alternativeJsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(alternativeSaveFilePath);
                if (alternativeJsonAsset != null)
                {
                    loadData = (UserData)JsonUtility.FromJson(alternativeJsonAsset.text, typeof(UserData));
                    DeleteAlternativeData();
                    Debug.LogError("前回の終了時にデータが正常に保存できませんでした。" +
                        "UserData.jsonを削除するか他のアプリケーションで使用されているか確認してください。");
                    return loadData;
                }

                //前回の終了時にデータが正常に保存できた場合
                var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(saveFilePath);
                if (jsonAsset == null)
                {
                    //ファイル無い場合生成
                    SaveData(UserData.New);
                    jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(saveFilePath);
                }

                loadData = (UserData)JsonUtility.FromJson(jsonAsset.text, typeof(UserData));
                if (jsonAsset.text.Length <= EmptySearchLength)
                {
                    loadData = UserData.New;
                }
            }
            catch (Exception ex)
            {
                loadData = UserData.New;
                Debug.LogError($"UserData.jsonの読み込みに失敗しました。 エラー文:{ex.Message}");
            }
            return loadData;
        }
    }

}