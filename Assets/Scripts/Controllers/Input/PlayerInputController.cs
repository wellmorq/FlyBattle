using System.Collections;
using System.Collections.Generic;
using FlyBattle.Controllers;
using FlyBattle.UI;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Controllers
{
    public class PlayerInputController : InputController
    {
        protected override void CancelCheck()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                switch (GameManager.Instance.CurrentState)
                {
                    case GameState.Play:
                        GameManager.Instance.ChangeGameState(GameState.Pause);
                        break;
                    case GameState.Pause:
                        GameManager.Instance.ChangeGameState(GameState.Play);
                        GameManager.Instance.OnResumeInvoke();
                        break;
                }
            }
        }

        protected override void MoveCheck()
        {
            h_Input = Input.GetAxis("Horizontal");
            Receiver?.Move(h_Input);
        }

        protected override void FireCheck()
        {
            if (Input.GetButtonDown("Fire1")) Receiver?.Shoot();
        }
    }
}