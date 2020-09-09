using FlyBattle.Interface;
using FlyBattle.Utils;
using Gamelogic.Extensions;
using FlyBattle.Player;
using FlyBattle.UI;
using ScriptablePattern;
using UnityEngine;

namespace FlyBattle.Controllers
{
    public abstract class BattleController : GLMonoBehaviour, ILevelController
    {
        public PlaneManager Player1 { get; protected set; }
        public PlaneManager Player2 { get; protected set; }

        protected GameData TempData;

        /// <summary>
        /// Создание котролера уровня и инициализация его полей
        /// </summary>
        /// <param name="data">Данные уровня</param>
        /// <returns></returns>
        public static BattleController Create(GameData data)
        {
            var obj = new GameObject
            {
                name = Consts.c_game_gameController_name,
                tag = Consts.c_game_gameController_name
            };

            DontDestroyOnLoad(obj);

            BattleController controller = null;

            switch (data.Type)
            {
                case Consts.BattleType.SinglePlayer:
                    controller = obj.AddComponent<SinglePlayerController>().Set(data);
                    break;
                case Consts.BattleType.MultiPlayer:
                    controller = obj.AddComponent<PvPController>().Set(data);
                    break;
                case Consts.BattleType.PvP:
                    controller = obj.AddComponent<PvPController>().Set(data);
                    break;
                case Consts.BattleType.PvE:
                    controller = obj.AddComponent<PvEController>().Set(data);
                    break;
                default:
                    Debug.LogException(new UnityException("Неизвестный тип инициализации контроллера уровня"));
                    break;
            }

            return controller;
        }

        protected virtual BattleController Set(GameData data)
        {
            //var instance = GameManager.Instance;
           // ObjectHealth.ObjectDestroy += ObjectDestroy;
           // instance.GamePlay += () => SetReady(true);

            //TempData = data;

            //Player1 = new PlaneManager(TempData.Player1, TempData.Type, this.gameObject);
            if (TempData.Type != Consts.BattleType.SinglePlayer)
                Player2 = new PlaneManager(TempData.Player2, TempData.Type, this.gameObject);

            return this;
        }

        //--- Имплементация интерфейса

        public virtual bool CheckReady(GameState state)
        {
            var flag = false;
            switch (state)
            {
                case GameState.Init:
                    flag = Fader.StatusController.GetStatus().IsComplete();
                    break;
                case GameState.Start:
                    flag = RoundDeclaring.StatusController.GetStatus().IsComplete();
                    break;
                case GameState.Play:
                    flag = Player1?.Life <= 0 || Player2?.Life <= 0;
                    break;
                case GameState.Pause:
                    break;
                case GameState.End:
                    flag = GameEnd();
                    break;
                default:
                    return false;
            }

            return flag;
        }

        private bool GameEnd()
        {
            if (GameManager.Instance.isInvoked)
                return
                    PausePanelControl.StatusController.GetStatus().IsComplete()
                    &&
                    GameIsOver();

            GameManager.Instance.isInvoked = true;
            PlaneManager roundWinner = null;
            if (Player1?.Life > 0) roundWinner = Player1;
            if (Player2?.Life > 0) roundWinner = Player2;
            if (roundWinner != null)
            {
                roundWinner.Win++;
                if (roundWinner.Profile == Player1.Profile) TempData.P1W++;
                else TempData.P2W++;
                TempData.roundWinner = roundWinner.Profile;
            }

            TempData.indexRound++;

            GameManager.Instance.GameData = TempData;
            // Save profile changes!
            GetAndSaveProfilesChanges();
            // calculate winners if end game
            return PausePanelControl.StatusController.GetStatus().IsComplete() && GameIsOver();
        }

        private bool GameIsOver()
        {
            var winC = CheckMaxWinCount();
            if (winC <= TempData.MaxRound - winC && TempData.indexRound <= TempData.MaxRound) return true;

            // Game is over

            if (GetBattleWinner()?.Profile == ProfileController.CurrentProfile)
            {
                GameManager.Instance.OnGameWin();
            }
            else
            {
                GameManager.Instance.OnGameDef();
            }
            
            return false;

            int CheckMaxWinCount() //calculate max win count
            {
                int maxWinCount;
                if (Player2 != null) maxWinCount = Player1.Win > Player2.Win ? Player1.Win : Player2.Win;
                else maxWinCount = Player1.Win;

                return maxWinCount;
            }
        }

        public void SetReady(bool flag)
        {
            Player1?.ChangeReady(flag);
            Player2?.ChangeReady(flag);
        }

        private void ObjectDestroy(IHealth health, ObjectDestroyEventArgs e)
        {
            if (GameManager.Instance.CurrentState != GameState.Play) return;
            if (e.CurrentDestroyedObject == DestroyedObject.Plane)
            {
                Player1?.PlayerObjectDestroy(health, e);
                Player2?.PlayerObjectDestroy(health, e);
            }
            else
            {
                Player1?.PlayerObjectCreate(health, GameObject.FindWithTag(Consts.c_game_spawnPoint_player1));
                Player2?.PlayerObjectCreate(health, GameObject.FindWithTag(Consts.c_game_spawnPoint_player2));
            }
        }

        protected void GetAndSaveProfilesChanges()
        {
            var winner = GetBattleWinner();
            if (Player1 == winner)
            {
                Player1.Profile.Win++;
                if (Player2 != null) Player2.Profile.Lose++;
            }
            
            else if (Player2 == winner)
            {
                Player1.Profile.Lose++;
                if (Player2 != null) Player2.Profile.Win++;
            }

            else
            {
                Player1.Profile.Lose++;
                if (Player2 != null) Player2.Profile.Lose++; 
            }
            
            ProfileController.SaveProfile(Player1.Profile);
            ProfileController.SaveProfile(Player2?.Profile);
        }

        private PlaneManager GetBattleWinner()
        {
            PlaneManager roundWinner = null;
            if (Player2 != null)
            {
                if (Player1.Win > Player2.Win)
                {
                    roundWinner = Player1;
                }
                else if (Player2.Win > Player1.Win)
                {
                    roundWinner = Player2;
                }
            }

            else
            {
                if (Player1.Plane != null && Player1.Plane.activeSelf)
                roundWinner = Player1;
            }

            return roundWinner;
        }

        public virtual void Clear()
        {
            ObjectHealth.ObjectDestroy -= ObjectDestroy;
            Destroy(gameObject);
        }

        public virtual void Reset()
        {
            Player1.Life = Consts.c_game_liveMaxCount;
            Player1.Setup(GameObject.FindWithTag(Consts.c_game_spawnPoint_player1));

            if (Player2 == null) return;

            Player2.Life = Consts.c_game_liveMaxCount;
            Player2?.Setup(GameObject.FindWithTag(Consts.c_game_spawnPoint_player2));
        }
    }
}