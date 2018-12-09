using UnityEngine;

namespace FlyBattle.Controllers
{
    public class LocalInputController: InputController
    {
        protected override void CancelCheck()
        {
        }

        protected override void MoveCheck()
        {
            h_Input = Input.GetAxis("Horizontal2");
            Receiver?.Move(h_Input);
        }

        protected override void FireCheck()
        {
            if (Input.GetButtonDown("Fire2")) Receiver?.Shoot();
        }
    }
}