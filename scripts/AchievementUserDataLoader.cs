using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Kazuro.Editor.Achievement
{
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

            //�ꎞ�ۑ��t�@�C��������ꍇ�ǂݍ���
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
            catch (Exception ex)
            {
                loadData = UserData.New;
                Debug.LogError($"UserData.json�̓ǂݍ��݂Ɏ��s���܂����B �G���[��:{ex.Message}");
            }
            return loadData;
        }
    }

}