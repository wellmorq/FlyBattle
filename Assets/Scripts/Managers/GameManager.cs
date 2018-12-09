using System;
using FlyBattle.Controllers;
using FlyBattle.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Init,
    Start,
    Play,
    Pause,
    End,
    Finish
}

namespace FlyBattle.Utils
{
    public class GameManager : Singleton<GameManager>
    {
        private static bool _created;
        public GameData GameData { get; set; }

        private void Awake()
        {
            if (Instance != this)
            {
                gameObject.SetActive(false);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            _created = true;
        }

        //-------------------------------------------------------------

        #region Properties

        public GameState CurrentState { get; private set; } = (GameState) (-1);
        public bool isInvoked = false;
        public ILevelController LevelController { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Starts loading the level data with the round preset
        /// </summary>
        /// <param name="data">Данные раунда</param>
        public void LoadNewLevel(GameData data)
        {
            GameData = data;
            SceneController.Instance.FadeAndLoadScene(data.Level);
        }

        /// <summary>
        /// Change current state
        /// </summary>
        /// <param name="state">new state</param>
        public void ChangeGameState(GameState state)
        {
            if (CurrentState == state) return;
            CurrentState = state;
            changeState(state);
            isInvoked = false;
            StateChanged?.Invoke(state);
        }

        private void changeState(GameState state)
        {
            switch (state)
            {
                case GameState.Init: // Setup in SceneController
                    Instance.OnGameInit();
                    break;
                case GameState.Start:
                    Instance.OnGameStart();
                    break;
                case GameState.Play:
                    Instance.OnGamePlay();
                    break;
                case GameState.Pause:
                    Instance.OnPauseInvoke();
                    break;
                case GameState.End:
                    Instance.OnGameEnd();
                    break;
                case GameState.Finish:
                    Instance.RemoveRedundancies();
                    break;
                default:
                    Debug.LogException(new UnityException("Передан неверный GameState"));
                    break;
            }
        }

        /// <summary>
        /// Get or Create Level Controller
        /// </summary>
        private ILevelController LevelControllerInitialize()
        {
            if (LevelController != null) return LevelController;
            LevelController = GameObject.FindWithTag(Consts.c_game_gameController_name)
                ?.GetComponent<ILevelController>();
            if (LevelController != null) return LevelController;

            if (SceneManager.GetActiveScene().name != SceneNames.Menu.ToString())
                LevelController = BattleController.Create(GameData);
            else
                Debug.LogException(new UnityException(
                    "Не найден объект типа ILevelController. Убедитесь, что MenuManager присутствует в сцене"));
            return LevelController;
        }

        /// <summary>
        /// Clear pointers
        /// </summary>
        private void LevelControllerClear()
        {
            LevelController?.Clear();
            LevelController = null;
        }

        #endregion

        #region Events

        public event Action<GameState> StateChanged = delegate {  };
        public event Action GameStart = delegate { };
        public event Action GameInit = delegate { };
        public event Action GamePlay = delegate { };
        public event Action GameEnd = delegate { };
        public event Action GameDef = delegate { };
        public event Action GameWin = delegate { };
        public event Action PauseInvoke = delegate { };
        public event Action ResumeInvoke = delegate { };
        public event Action ButtonClick = delegate { };

        private void OnGameInit()
        {
            LevelControllerInitialize()?.Reset();
            GameInit?.Invoke();
        }

        private void OnGameStart()
        {
            GameStart?.Invoke();
        }

        private void OnGamePlay()
        {
            GamePlay?.Invoke();
        }

        private void OnGameEnd()
        {
            GameEnd?.Invoke();
        }

        internal void OnPauseInvoke()
        {
            PauseInvoke?.Invoke();
        }

        internal void OnResumeInvoke()
        {
            ResumeInvoke?.Invoke();
        }

        // --------------------------------------------------------------------------
        // Special events for unstate situations
        public void OnGameDef()
        {
            GameDef?.Invoke();
        }

        public void OnGameWin()
        {
            GameWin?.Invoke();
        }

        public void OnButtonClick()
        {
            ButtonClick?.Invoke();
        }

        #endregion

        //-------------------------------------------------------------

        /*
         *  Listener block that toggles the playing state, depending on the fulfillment of the conditions ILevelController`а.
         */

        /// <summary>
        /// Контролирует переключение игровых состояний, при выполнении условий ILevelController`ом.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Передано неверное состояние</exception>
        private void GameStateListener()
        {
            if (LevelController == null) return;

            if (!LevelController.CheckReady(CurrentState))
                return;

            isInvoked = true;
            
            switch (CurrentState)
            {
                case GameState.Init:
                    print(CurrentState.ToString());
                    ChangeGameState(GameState.Start);
                    print(CurrentState.ToString());
                    break;
                case GameState.Start:
                    print(CurrentState.ToString());
                    ChangeGameState(GameState.Play);
                    print(CurrentState.ToString());
                    break;
                case GameState.Play:
                    print(CurrentState.ToString());
                    ChangeGameState(GameState.End);
                    print(CurrentState.ToString());
                    break;
                case GameState.Pause:
                    print(CurrentState.ToString());
                    ChangeGameState(GameState.Play);
                    print(CurrentState.ToString());
                    break;
                case GameState.End:
                    SceneController.Instance.RestartScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
        }

        private void Update()
        {
            GameStateListener();
        }

        //-------------------------------------------------------------

        // Cleanup events
        public void RemoveEvent(out Action e)
        {
            e = null;
        }

        public void RemoveEvent(out Action<GameObject> e)
        {
            e = null;
        }

        /// <summary>
        /// Call it when changing level
        /// </summary>
        private void RemoveRedundancies()
        {
            LevelControllerClear();
            GameStart = null;
            GameInit = null;
            GamePlay = null;
            GameEnd = null;
            GameDef = null;
            GameWin = null;
            PauseInvoke = null;
            ResumeInvoke = null;
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        private void OnApplicationQuit()
        {
            RemoveRedundancies();
            Destroy(gameObject);
        }
    }
}