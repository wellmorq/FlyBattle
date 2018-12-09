using System;
using FlyBattle.Interface;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Player
{
    public class ObjectHealth : MonoBehaviour
    {
        public static event Action<IHealth, IHealthChange> TakeDamage = delegate { };
        public static event Action<IHealth, ObjectDestroyEventArgs> ObjectDestroy = delegate { };

        protected static void TakeDamageEvent(IHealth health, IHealthChange e)
        {
            TakeDamage?.Invoke(health, e);
        }

        protected static void DestroyObjectEvent(IHealth obj, ObjectDestroyEventArgs e)
        {
            ObjectDestroy?.Invoke(obj, e);
        }
    }
}