using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using FlyBattle;
using Gamelogic.Extensions;
using ScriptablePattern;
using UnityEngine;

namespace FlyBattle
{
    [Serializable]
    public class Profile : IEquatable<Profile>
    {
        [SerializeField] private string _name;
        [SerializeField] private string _planeInfo;
        [SerializeField] private string _skyDriverInfo;
        [SerializeField] private int _money;
        [SerializeField] private int _exp;
        [SerializeField] private float[] playerColor;
        [SerializeField] private float[] enemyColor;

        [SerializeField]
        private int _winCount;
        [SerializeField]
        private int _loseCount;

        public int Win
        {
            get => _winCount;
            set
            {
                if (value >= _winCount) _winCount = value;
                else throw new ArgumentException();
            }
        }

        public int Lose
        {
            get => _loseCount;
            set
            {
                if (value >= _loseCount) _loseCount = value;
                else throw new ArgumentException();
            }
        }

        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public string PlaneInfo
        {
            get => _planeInfo;
            set
            {
                if (value != null) _planeInfo = value;
            }
        }

        public string SkyDriverInfo
        {
            get => _skyDriverInfo;
            set
            {
                if (value != null) _skyDriverInfo = value;
            }
        }

        public int Money
        {
            get => _money;
            set
            {
                if (value >= 0) _money = value;
                else Debug.LogException(new UnityException("Деньги ушли в минус!"));
            }
        }

        public int Exp
        {
            get => _exp;
            set
            {
                if (value >= _exp) _exp = value;
                else Debug.LogException(new UnityException("Опыт ушёл в минус!"));
            }
        }

        public List<string> PurchasedPlanes
        {
            get => _purchasedPlanes;
            set
            {
                if (value != null) _purchasedPlanes = value;
            }
        }

        public List<string> PurchasedSkyDrivers
        {
            get => _purchasedSkyDrivers;
            set
            {
                if (value != null) _purchasedSkyDrivers = value;
            }
        }

        [SerializeField]
        private List<string> _purchasedPlanes;
        [SerializeField]
        private List<string> _purchasedSkyDrivers;

        public Color PlayerColor
        {
            get
            {
                var col = new Color();
                for (int i = 0; i < 4; i++)
                {
                    col[i] = playerColor[i];
                }

                return col;
            }
            set
            {
                Color col = value;
                for (int i = 0; i < 4; i++)
                {
                    playerColor[i] = col[i];
                }
            }
        }

        public Color EnemyColor
        {
            get
            {
                var col = new Color();
                for (int i = 0; i < 4; i++)
                {
                    col[i] = enemyColor[i];
                }

                return col;
            }
            set
            {
                Color col = value;
                for (int i = 0; i < 4; i++)
                {
                    enemyColor[i] = col[i];
                }
            }
        }

        public Profile(string name)
        {
            PurchasedPlanes = new List<string>();
            PurchasedSkyDrivers = new List<string>();
            Exp = 0;
            Money = 0;
            Name = name;
            //PlaneInfo = new ItemInfo(null);
            //SkyDriverInfo = new ItemInfo(null);
            playerColor = new[] {40f, 40f, 180f, 1f};
            enemyColor = new[] {180f, 40f, 40f, 1f};
        }

        public bool Equals(Profile other)
        {
            return other != null && other.Name == this.Name;
        }
    }
}