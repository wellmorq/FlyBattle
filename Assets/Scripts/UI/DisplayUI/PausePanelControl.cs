using System.Collections;
using FlyBattle.Controllers;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.UI
{
    public class PausePanelControl : Singleton<PausePanelControl>
    {
        [SerializeField] private float playingTimeScale = 1f;
        [SerializeField] private float pauseTimeScale = 0f;
        [SerializeField] private float stepDuration = 1f;

        private Coroutine c_Slower = null;
        public static StatusController StatusController;


        private void OnEnable()
        {
            StatusController = new StatusController();
            StatusController.UpStatus(StatusController.Status.NotStarted);

            var instance = GameManager.Instance;
            if (instance == null) return;

            instance.PauseInvoke += PauseOn;
            instance.GameEnd += OnEndEvent;
            instance.ResumeInvoke += PauseOff;
        }

        private void OnDisable()
        {
            var instance = GameManager.Instance;
            if (instance == null) return;

            instance.PauseInvoke -= PauseOn;
            instance.GameEnd -= OnEndEvent;
            instance.ResumeInvoke -= PauseOff;
        }

        public void PauseOn()
        {
            ShowPausePanel();

            if (c_Slower != null)
                StopCoroutine(c_Slower);
            c_Slower = StartCoroutine(SlowerTime(pauseTimeScale));
        }

        public void PauseOff()
        {
            HidePausePanel();

            if (c_Slower != null)
                StopCoroutine(c_Slower);
            c_Slower = StartCoroutine(SlowerTime(playingTimeScale));
        }

        public void ButtonClick()
        {
            GameManager.Instance.OnButtonClick();
        }

        private void HidePausePanel()
        {
            Instance.gameObject.ChangeChildActive(false);
        }

        private void ShowPausePanel()
        {
            Instance.gameObject.ChangeChildActive(true);
        }

        private IEnumerator SlowerTime(float finalTime)
        {
            if (GameManager.Instance.GameData.Type == Consts.BattleType.MultiPlayer &&
                GameManager.Instance.CurrentState != GameState.End) yield break;

            StatusController.UpStatus(StatusController.Status.Running);
            var pauseSpeed = Mathf.Abs(Time.timeScale - finalTime) / stepDuration;

            while (!Mathf.Approximately(Time.timeScale, finalTime))
            {
                Time.timeScale = Mathf.MoveTowards(Time.timeScale, finalTime, pauseSpeed * Time.unscaledDeltaTime);
                yield return null;
            }

            StatusController.UpStatus(StatusController.Status.Completed);
        }

        private void OnEndEvent()
        {
            StartCoroutine(EndGameCoroutine(pauseTimeScale));
        }
        
        private IEnumerator EndGameCoroutine(float time)
        {
            stepDuration = 3f;
            yield return StartCoroutine(SlowerTime(time));
            GameObject.FindWithTag(Consts.c_game_gameController_name).GetComponent<BattleController>().SetReady(false);
            Time.timeScale = 1f;
        }
    }
}