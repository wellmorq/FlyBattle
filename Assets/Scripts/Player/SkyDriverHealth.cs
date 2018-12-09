using System;
using FlyBattle.Interface;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Player
{
    public class SkyDriverHealth : ObjectHealth, IHealth
    {
        public void OnTakeDamage(IHealthChange obj, params Action<IHealth>[] effects)
        {
            if (GetComponent<SkyDriverController>().IsLanding) return;
            
            TakeDamageEvent(this, obj);
            OnDestroyObject(0);
        }

        public void OnDestroyObject(int life = 1, DestroyedObject state = DestroyedObject.Plane)
        {
            //ObjectPool.Spawn(DestroyPlayerEffect, new Vector2(position.x, position.y));
            //ObjectDestroy?.Invoke(this, new PlaneDestroyEventArgs(life, position));

            DestroyObjectEvent(this, new ObjectDestroyEventArgs(life, transform.position, state));
            gameObject.Recycle();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var layer = other.gameObject.layer;
            switch (layer)
            {
                case 13: // LeftWall
                    OnDestroyObject(-1, DestroyedObject.SkyDriver);
                    break;
                case 14: // RightWall
                    OnDestroyObject(-1, DestroyedObject.SkyDriver);
                    break;
            }
        }
    }
}