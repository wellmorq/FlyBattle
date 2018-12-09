using UnityEngine;

namespace FlyBattle.Interface
{
    public interface IEngine
    {
        void EnableEngine();
        void DisableEngine();

        void StopEngine(float t = 2f, GameObject obj = null);

        bool isStarted { get; }
    }
}