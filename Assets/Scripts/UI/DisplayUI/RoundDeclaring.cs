using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlyBattle.Controllers;
using FlyBattle.Interface;
using FlyBattle.Utils;
using Gamelogic.Extensions.Algorithms;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    public class RoundDeclaring : MonoBehaviour
    {
        [SerializeField] private Text player1, player2, score, roundNumber, countDown;

        [Space(5)] [Tooltip("Сколько ждать перед началом обратного отсчета")] [SerializeField]
        private int waitBeforeCountDown;

        [Tooltip("Количество цифр обратного отсчета")] [SerializeField]
        private int countDownNumbers = 3;

        //------------------------------------------------------------
        public static StatusController StatusController;
        //------------------------------------------------------------

        #region Subscriptions

        private void OnEnable()
        {
            StatusController = new StatusController();
            
            GameManager.Instance.GameInit += DeclaringLoadInfo;
            GameManager.Instance.GameStart += StartDeclaring;
        }

        private void OnDisable()
        {
            if (!GameManager.Instance) return;
            GameManager.Instance.GameInit -= DeclaringLoadInfo;
            GameManager.Instance.GameStart -= StartDeclaring;
        }

        #endregion

        private void DeclaringLoadInfo()
        {
            var data = GameManager.Instance.GameData;
            
            player1.text = data.Player1.Name;
            if (data.Player2 == null)
                Debug.LogException(new UnityException("Не заполнен Профиль2, либо запущен режим SinglePlayer!"));
            player2.text = data.Player2.Name ?? string.Empty;
            score.text = $"{data.P1W} : {data.P2W}";

            string str = "Round: {0} of {1}"; //todo ЛокализаторМенеджер
            roundNumber.text = string.Format(str, data.indexRound + 1, data.MaxRound); // Люди считают с 1
            countDown.text = string.Empty;
        }

        private void StartDeclaring()
        {
            StatusController.UpStatus(StatusController.Status.Running);
            gameObject.ChangeChildActive(true);
            StartCoroutine(Declarer());
        }

        private void EndDeclaring()
        {
            gameObject.ChangeChildActive(false);
            StatusController.UpStatus(StatusController.Status.Completed);
        }

        private IEnumerator Declarer()
        {
            yield return new WaitForSeconds(waitBeforeCountDown);
            yield return StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
            while (countDownNumbers > 0)
            {
                countDown.text = countDownNumbers.ToString();
                countDown.fontSize = 300;
                var i = 1f; // 1 сек
                while (i > 0)
                {
                    i -= Time.deltaTime;
                    countDown.fontSize -= (int) (Time.deltaTime * 300);
                    yield return null;
                }

                countDownNumbers--;
            }

            EndDeclaring();
        }
    }
}