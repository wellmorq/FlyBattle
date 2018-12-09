using FlyBattle.Interface;
using UnityEngine;

namespace FlyBattle.Controllers
{
    public abstract class InputController : MonoBehaviour
    {
        /// <summary>
        /// Horizontal input variable
        /// </summary>
        protected float h_Input;

        /// <summary>
        /// Plane or SkyDriver or another controllers
        /// </summary>
        public IControllerReceiver Receiver { protected get; set; }

        void Update()
        {
            CancelCheck();
            FireCheck();
        }

        private void FixedUpdate()
        {
            MoveCheck();
        }

        protected abstract void CancelCheck();
        protected abstract void MoveCheck();
        protected abstract void FireCheck();
    }
}