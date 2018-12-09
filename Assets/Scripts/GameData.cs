using System;
using System.Text;

namespace FlyBattle
{
    [Serializable]
    public struct GameData
    {
        public readonly Consts.BattleType Type;
        public readonly int MaxRound;
        public readonly Profile Player1;
        public readonly Profile Player2;

        public int indexRound;
        public int P1W;
        public int P2W;

        public Profile roundWinner;

        public readonly string Level;

        public GameData(
            Consts.BattleType battleType,
            int maxRoundCount,
            Profile playerProfile,
            Profile enemyProfile,
            string level)
        {
            Type = battleType;
            MaxRound = maxRoundCount;
            Player1 = playerProfile;
            Player2 = enemyProfile;
            Level = level;

            indexRound = 0;
            P1W = 0;
            P2W = 0;
            roundWinner = null;
        }

        public override string ToString()
        {
            return new StringBuilder(Type.ToString() + " " + MaxRound + " " +
                                     Player1.Name + " vs " + (Player2 != null ? Player2.Name : "null") + " " +
                                     DateTime.Now.ToString()).ToString();
        }
    }
}