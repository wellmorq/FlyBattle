using System;
using UnityEngine;

namespace FlyBattle
{
    [Serializable]
    public class ItemInfo : IEquatable<ItemInfo>
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public int Money;
        [SerializeField] public int Exp;
        [SerializeField] public string Describe;

        [SerializeField] private string guid;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public ItemInfo(GameObject prefab)
        {
            Prefab = prefab;
        }

        public bool Equals(ItemInfo other)
        {
            return other != null && other.Prefab.Equals(Prefab);
        }
    }
}