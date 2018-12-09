using System;
using FlyBattle.Interface;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Player
{
    public class PlaneHealth : ObjectHealth, IHealth
    {
        /// <summary>
        /// Виды состояний самолёта
        /// </summary>
        public enum HealthState
        {
            Bad,
            Well,
            Fine
        }

        [SerializeField] private HealthState _currentHealthState = HealthState.Fine;

        public GameObject[] SmokeEffects;
        public GameObject DestroyPlayerEffect;

        [SerializeField] private int _maxHealthPoints, _currentHealthPoints = 3;

        private float _step = -1; // шаг HP между состояниями HealthState
        private int _stateCount; // количество состояний HealthState


        private void Awake()
        {
            _stateCount = Enum.GetValues(typeof(HealthState)).Length;
        }

        private void Start()
        {
            _maxHealthPoints = 3; //todo обращение к БД
        }

        public HealthState CurrentHealthState
        {
            get { return _currentHealthState; }
            private set
            {
                _currentHealthState = value;
                foreach (var eff in SmokeEffects)
                {
                    eff.SetActive(false);
                }

                var num = (int) value;
                if (num < SmokeEffects.Length) SmokeEffects[num].SetActive(true);
            }
        }

        /// <summary>
        /// Изменяет кол-во ХП и применяет передаваемые эффекты
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="effects"></param>
        public void OnTakeDamage(IHealthChange obj, params Action<IHealth>[] effects)
        {
            print("I was damaged, " + LayerMask.LayerToName(gameObject.layer)); //todo debug
            ChangeHealthPoints(Mathf.FloorToInt(obj.DamageCount));

            TakeDamageEvent(this, obj);

            foreach (var e in effects)
            {
                e.Invoke(this);
            }
        }

        private void ChangeHealthPoints(int value)
        {
            _currentHealthPoints += value;

            var state = CalculateState();
            if (state >= 0) CurrentHealthState = (HealthState) CalculateState();
            else OnDestroyObject();
        }

        private int CalculateState()
        {
            if (Mathf.Approximately(_step, -1))
            {
                _step = (float) _maxHealthPoints / _stateCount;
            }

            int i = _stateCount;
            while (i > 0)
            {
                i--;
                if (_currentHealthPoints > i * _step) return i;
            }

            return -1;
        }

        /// <summary>
        /// Уничтожает объект
        /// </summary>
        public void OnDestroyObject(int life = 1, DestroyedObject state = DestroyedObject.Plane)
        {
            var position = transform.position;
            ObjectPool.Spawn(DestroyPlayerEffect, new Vector2(position.x, position.y));

            DestroyObjectEvent(this, new ObjectDestroyEventArgs(life, position, state));
            gameObject.Recycle();
        }
    }
}