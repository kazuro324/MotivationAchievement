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
        public int bootCount;

        public TempData(int buildCount, int playCount, int bootCount)
        {
            this.buildCount = buildCount;
            this.playCount = playCount;
            this.bootCount = bootCount;
        }

        public static TempData New
        {
            get
            {
                var newData = new TempData();
                newData.buildCount = 0;
                newData.playCount = 0;
                newData.bootCount = 0;
                return newData;
            }
        }
    }

    public struct UserData
    {
        public int[] firstOpenDateArray;
        public int[] lastOpenDate;

        public uint weekWorkTime;
        public int weekBuildCount;
        public int weekPlayModeCount;
        public int weekBootCount;
        public int weekBootDays;
        public uint weekContinueFirstDays;

        public uint currentContinueDays;
        public uint highestContinueDays; 
        public uint totalWorkTime;
        public int totalBuildCount;
        public int totalPlayModeCount;
        public int totalBootCount;
        public int todayBootCount;

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
                newData.weekBootCount = 0;
                newData.weekBuildCount = 0;
                newData.weekPlayModeCount = 0;
                newData.weekWorkTime = 0;
                newData.weekBootDays = 0;
                newData.weekContinueFirstDays = 0;
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
        const string saveFilePath = "Assets/Editor/Save/UserData.json";
        const string alternativeSaveFilePath = "Assets/Editor/Save/AlternativeUserData.json";
        const string alternativeSaveMetaPath = "Assets/Editor/Save/AlternativeUserData.json.meta";
        const string temporarySaveFilePath = "Assets/Editor/Save/TempData.json";
        const string temporarySaveMetaPath = "Assets/Editor/Save/TempData.json.meta";
        const int EmptySearchLength = 6;

        public static void TemporarySaveData(TempData data)
        {
            var jsonData = JsonUtility.ToJson(data);
            var writer = new StreamWriter(temporarySaveFilePath, false);
            writer.Write(jsonData);
            writer.Close();
            AssetDatabase.Refresh();
        }

        public static void DeleteTemporaryData()
        {
            if (File.Exists(temporarySaveFilePath))
            {
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


        public static TempData TemporaryLoadData()
        {
            AssetDatabase.Refresh();

            //�ꎞ�ۑ��t�@�C��������ꍇ�ǂݍ���
            try
            {
                var tmpAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(temporarySaveFilePath);
                var tmpData = (TempData)JsonUtility.FromJson(tmpAsset.text, typeof(TempData));

                DeleteTemporaryData();
                return tmpData;
            }
            catch
            {
                return TempData.New;
            }
        }

        public static void SaveData(UserData data)
        {
            var jsonData = JsonUtility.ToJson(data);
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

            AssetDatabase.Refresh();
        }

        public static UserData LoadData()
        {
            UserData loadData = UserData.New;

            try
            {
                //�O��̏I�����Ƀf�[�^������ɕۑ��ł��Ȃ������ꍇ
                var alternativeJsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(alternativeSaveFilePath);
                if (alternativeJsonAsset != null)
                {
                    loadData = (UserData)JsonUtility.FromJson(alternativeJsonAsset.text, typeof(UserData));
                    DeleteAlternativeData();
                    Debug.LogError("�O��̏I�����Ƀf�[�^������ɕۑ��ł��܂���ł����B" +
                        "UserData.json���폜���邩���̃A�v���P�[�V�����Ŏg�p����Ă��邩�m�F���Ă��������B");
                    return loadData;
                }

                //�O��̏I�����Ƀf�[�^������ɕۑ��ł����ꍇ
                var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(saveFilePath);
                if (jsonAsset == null)
                {
                    //�t�@�C�������ꍇ����
                    SaveData(UserData.New);
                    jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(saveFilePath);
                }

                loadData = (UserData)JsonUtility.FromJson(jsonAsset.text, typeof(UserData));
                if (jsonAsset.text.Length <= EmptySearchLength)
                {
                    loadData = UserData.New;
                }
            }
            catch
            {
                loadData = UserData.New;
                Debug.LogError("UserData.json�̓ǂݍ��݂Ɏ��s���܂����B");
            }
            return loadData;
        }
    }

}