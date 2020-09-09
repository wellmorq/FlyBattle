using FlyBattle.Interface;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Controllers
{
    public class PvEController : BattleController
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
            //bool flag;
            switch (state)
            {
                default:
                    flag = base.CheckReady(state);
                    break;
            }

            return flag;
        }
    }
}