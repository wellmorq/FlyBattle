using System.Collections;
using System.Collections.Generic;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Controllers
{
    public class PvPController : BattleController
    {
        protected Consts.BattleType _gameType;
        
        protected override BattleController Set(GameData data)
        {
            base.Set(data);
            
            _gameType = data.Type;
            return this;
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}