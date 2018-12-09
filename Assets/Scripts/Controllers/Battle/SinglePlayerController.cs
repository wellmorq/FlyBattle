using System.Runtime.InteropServices;
using FlyBattle.UI;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Controllers
{
    public class SinglePlayerController : BattleController
    {
        protected override BattleController Set(GameData data)
        {
            base.Set(data);
            return this;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override bool CheckReady(GameState state)
        {
            bool flag;
            switch (state)
            {
                case GameState.Start:
                    flag = BriefingController.StatusController.GetStatus().IsComplete();
                    break;
                case GameState.End:
                    flag = Fader.StatusController.GetStatus().IsComplete();
                    break;
                default:
                    flag = base.CheckReady(state);
                    break;
            }

            return flag;
        }

        // todo DEBUG -----------------------------------

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G)) GameManager.Instance.ChangeGameState(GameState.End);
            if (Input.GetKeyDown(KeyCode.H)) GameManager.Instance.ChangeGameState(GameState.Finish);
        }
#endif
    }
}