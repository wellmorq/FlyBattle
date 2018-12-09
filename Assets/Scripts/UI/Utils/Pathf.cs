using System.IO;
using UnityEditor;
using UnityEngine;

namespace FlyBattle.Utils
{
    public class Pathf
    {
        #region DataPath & ProfilePath

        private static string _runtimeAssetsDataPath;
        private static string _profilesDataPath;

        /// <summary>
        /// Предоставляет относительный путь до папки сохранения ассетов, либо создает его
        /// </summary>
        public static string RuntimeAssetsDataPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_runtimeAssetsDataPath)) return _runtimeAssetsDataPath;
                _runtimeAssetsDataPath = GetNewPath(Consts.c_runtimeAssets_folderName, Consts.c_data_folderName);

                return _runtimeAssetsDataPath;
            }
        }

        /// <summary>
        /// Предоставляет относительный путь до папки сохранения профилей, либо создает его
        /// </summary>
        public static string ProfilesDataPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_profilesDataPath)) return _profilesDataPath;
                _profilesDataPath = GetNewPath(RuntimeAssetsDataPath, Consts.c_profiles_folderName);

                return _profilesDataPath;
            }
        }

        private static string GetNewPath(string parentPath, string createFolder)
        {
            var path = Path.Combine(Application.dataPath, parentPath, createFolder);
            if (Directory.Exists(path)) return path;

            var dir = Directory.CreateDirectory(path);
            dir.Create();

            return dir.FullName;
        }

        #endregion
    }
}