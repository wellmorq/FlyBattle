using System;
using System.Collections;
using FlyBattle.UI;
using FlyBattle.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FlyBattle.Controllers
{
    public class SceneController : Singleton<SceneController>
    {
        [SerializeField] private Fader _fader;

        public Fader Fader
        {
            get
            {
                if (_fader != null) return _fader;
                _fader = GameObject.Find(Consts.c_game_fader_name)?.GetComponent<Fader>();
                if (_fader != null) return _fader;
                Debug.LogException(new UnityException("Компонент Fader отсутствует в сцене!"));
                return null;
            }
        }


        private void Start()
        {
            StartCoroutine(OnLoadLevel());
        }

        //--------------------------------------------------------------

        public void FadeAndLoadScene(string sceneName)
        {
            if (Fader.isFading) return;

            if (Application.CanStreamedLevelBeLoaded(sceneName)) // Если уровень может быть загружен
            {
                GameManager.Instance.ChangeGameState(GameState.Finish);
                StartCoroutine(FadeAndSwitchScenes(sceneName));
            }
            else
            {
                Debug.LogException(
                    new UnityException(
                        $"Загружаемая сцена {sceneName} не существует!")); //todo реализовать запрос на скачивание сцены через нетворк менеджер
            }
        }

        public void FadeAndLoadScene(SceneNames scene)
        {
            FadeAndLoadScene(scene.ToString());
        }

        public void RestartScene()
        {
            if (Fader.StatusController.GetStatus().IsNotStarted())
            StartCoroutine(FadeAndSwitchScenes(SceneManager.GetActiveScene().name));
        }

        //--------------------------------------------------------------

        #region Corutines

        /// <summary>
        /// Запуск в начале уровня. Даёт старт всему игровому циклу!
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnLoadLevel()
        {
            GameManager.Instance.ChangeGameState(GameState.Init);
            yield return StartCoroutine(Fader.Fade(0));
        }

        private IEnumerator FadeAndSwitchScenes(string sceneName)
        {
            yield return StartCoroutine(Fader.Fade(1f));
            yield return StartCoroutine(LoadScene(sceneName));
            StartCoroutine(OnLoadLevel());
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        #endregion
    }
}