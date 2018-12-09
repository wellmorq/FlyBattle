using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlyBattle.Utils;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using Common.Cryptography;

namespace FlyBattle.Controllers
{
    /// <summary>
    /// Предоставляет доступ к управлению коллекцией и текущим состоянием профилей
    /// </summary>
    [CreateAssetMenu(menuName = "MyAssets/ScriptableObjects/ProfileController", fileName = "ProfileController",
        order = 99)]
    [Serializable]
    public class ProfileController : ScriptableObject
    {
        //------ Variables ---------
        [SerializeField] private static List<Profile> _profileList;

        #region Properties

        /// <summary>
        /// Предоставляет список созданных профилей
        /// </summary>
        public static List<Profile> ProfileList
        {
            get
            {
                if (_profileList != null)
                {
                    if (_profileList.Count > 0)
                    {
                        _profileList = _profileList.FindAll(x => !string.IsNullOrEmpty(x.Name));
                        if (_profileList.Count > 0) return _profileList;
                    }
                }

                _profileList = GetAssetFiles<Profile>(Pathf.ProfilesDataPath, Consts.c_profiles_formatName).ToList();

                return _profileList;
            }
        }

        private static IEnumerable<T> GetAssetFiles<T>(string path, string endWith) where T : class
        {
            var profilesPath =
                Directory.GetFiles(path).Where(str => str.EndsWith(endWith));

            foreach (var profPath in profilesPath)
            {
                yield return SaveLoad.Load<T>(profPath);
            }
        }

        /// <summary>
        /// Текущее состояние профиля игрока
        /// </summary>
        public static Profile CurrentProfile { get; set; }

        #endregion

        #region Methods

        public static Profile GetOrAdd(string profileName)
        {
            return Get(profileName) ?? Add(profileName);
        }

        private static Profile Add(string profileName)
        {
            var profile = new Profile(profileName);
            _profileList.Add(profile);

            if (_profileList.IndexOf(profile) < 0) return null;

            SaveProfile(profile);

            return profile;
        }


        private static Profile Get(string profileName)
        {
            return ProfileList?.SingleOrDefault(x => x?.Name == profileName);
        }

        public static bool Remove(Profile profile)
        {
            if (!ProfileList.Contains(profile)) return false;

            SaveLoad.DeleteFile(Path.Combine(Pathf.ProfilesDataPath,
                Consts.c_profiles_profileName + ProfileList.IndexOf(profile)));

            ProfileList.Remove(profile);

            return true;
        }

        public static void RemoveAll()
        {
            var files = Directory.GetFiles(Pathf.ProfilesDataPath);
            foreach (var file in files)
            {
                Debug.Log(file);
                SaveLoad.DeleteFile(file);
            }

            ProfileList.Clear();
        }

        public static void SaveProfile(Profile prof)
        {
            if (prof == null) return;
            if (!ProfileList.Contains(prof))
            {
                Debug.Log($"Профиль {prof.Name} отсутствует в списке профайлов {ProfileList}");
                return;
            }

            var file = Path.Combine(Pathf.ProfilesDataPath,
                Consts.c_profiles_profileName + ProfileList.IndexOf(prof) + Consts.c_profiles_formatName);
            SaveLoad.Save(prof, file);
        }

        #endregion
    }
}