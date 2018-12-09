using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlyBattle;
using Gamelogic.Extensions.Algorithms;
using UnityEditor;
using UnityEngine;

namespace ScriptablePattern
{
    [CreateAssetMenu(menuName = "MyAssets/ScriptableObjects/GameItems", fileName = "GameItems", order = 1)]
    public class GameItems : ScriptableObject
    {
        public List<ItemInfo> Planes;
        public List<ItemInfo> SkyDrivers;

        private Dictionary<string, ItemInfo> _allItemsInfo;

        private void OnEnable()
        {
            GetAllItemsInfo();
        }

        private void GetAllItemsInfo()
        {
            _allItemsInfo = Planes.ToDictionary(x => x.Guid);
            _allItemsInfo.AddRange(SkyDrivers.ToDictionary(x => x.Guid));
        }

        public ItemInfo GetItem(string guid)
        {
            if (guid == null) return null;
            return _allItemsInfo.TryGetValue(guid, out var item) ? item : null;
        }

        private void OnValidate()
        {
            var cache = Planes.FindAll(x => x.Guid == Guid.Empty.ToString() || string.IsNullOrEmpty(x.Guid));
            foreach (var plane in cache)
            {
                plane.Guid = Guid.NewGuid().ToString();
            }

            cache = SkyDrivers.FindAll(x => x.Guid == Guid.Empty.ToString() || string.IsNullOrEmpty(x.Guid));
            foreach (var driver in cache)
            {
                driver.Guid = Guid.NewGuid().ToString();
            }
        }
    }
}