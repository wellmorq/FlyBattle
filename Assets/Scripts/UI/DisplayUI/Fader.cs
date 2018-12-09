using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlyBattle.Utils;
using Gamelogic.Extensions.Algorithms;
using UnityEngine;

namespace FlyBattle.UI
{
    public class Fader : MonoBehaviour
    {
        /*
         * Обязательно помещать в корень интерфейса, содержащего Canvas
         * Иначе чревато ошибками
         */

        [Tooltip("Продолжительность затемнения")]
        public float FadeDuration;

        public bool isFading { get; private set; }

        public static StatusController StatusController;

        private CanvasGroup _faderCanvasGroup;
        // ---------------------------------------------------------------------------------

        private static Dictionary<int, int> _statusStore; // Массив состояний для BattleController`а
        private static int _keyIndex = -1; // Текущий указатель для BattleController`а (кэширует)

        // ---------------------------------------------------------------------------------

        public IEnumerator Fade(float finalAlpha)
        {
            if (_faderCanvasGroup == null) FadeAwakening();

            StatusController.UpStatus(StatusController.Status.Running);
            isFading = true;
            _faderCanvasGroup.blocksRaycasts = true;
            float fadeSpeed = Mathf.Abs(_faderCanvasGroup.alpha - finalAlpha) / FadeDuration;

            while (!Mathf.Approximately(_faderCanvasGroup.alpha, finalAlpha))
            {
                _faderCanvasGroup.alpha = Mathf.MoveTowards(_faderCanvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);
                yield return null;
            }
            _faderCanvasGroup.blocksRaycasts = false;
            isFading = false;
            StatusController.UpStatus(StatusController.Status.Completed);
        }

        private void FadeAwakening()
        {
            _faderCanvasGroup = GetComponent<CanvasGroup>();
            _faderCanvasGroup.alpha = 1f;
            _faderCanvasGroup.blocksRaycasts = false;
            isFading = false;

            StatusController = new StatusController();
        }
    }
}