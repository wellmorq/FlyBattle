using System;
using BeforeRefactoring;
using UnityEditor.PackageManager;
using UnityEngine;

namespace FlyBattle.Utils
{
    public enum DestroyedObject
    {
        Plane,
        SkyDriver
    }
    
    public class ObjectDestroyEventArgs : EventArgs
    {
        public int LifeDestroy = -1;
        public Vector3 ObjectPosition;

        public DestroyedObject CurrentDestroyedObject;
        
        public ObjectDestroyEventArgs(int life, Vector3 position, DestroyedObject state)
        {
            LifeDestroy = life;

            ObjectPosition = position;
            CurrentDestroyedObject = state;
        }
    }
}