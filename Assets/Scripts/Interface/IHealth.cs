using System;
using FlyBattle.Utils;

namespace FlyBattle.Interface
{
    public interface IHealth
    {
        void OnTakeDamage(IHealthChange obj, params Action<IHealth>[] effects);
        void OnDestroyObject(int life = 1, DestroyedObject state = DestroyedObject.Plane);
    }
}