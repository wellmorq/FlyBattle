using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle
{
    public class DestroyObj : MonoBehaviour
    {
        public float time = 1f;

        void Start()
        {
            Invoke(nameof(Recycler), time);
        }

        private void Recycler()
        {
            gameObject.Recycle();
        }
    }
}