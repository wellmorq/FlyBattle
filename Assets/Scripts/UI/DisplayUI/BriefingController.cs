using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlyBattle.Interface;
using FlyBattle.Utils;
using Gamelogic.Extensions.Algorithms;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.Controllers
{
    // Если меняется количество состояний брифинга - добавить и заполнить новую строку массива PathToFolderFace.
    // Для заполнения количества сообщений - добавить в файл локализатора
    public enum BriefingNumber
    {
        Start,
        Middle,
        End
    }

    public class BriefingController : MonoBehaviour
    {
        public Image BriefingFace;
        public Text BriefingText;

        [Header("Скорость печати, символ/сек")] [SerializeField]
        private float SpeedText = 10f;

        // Временная структура, пока не запилю локализатор-----------------------------------
        [Space(5)]
        [Header("Путь к папка/файлам с лицом спикера")]
        [Space(12)]
        [Header("Заполнять по порядку вывода геймплея. Каждое поле - отдельный брифинг")]
        public string[] PathToFolderFace = new string[System.Enum.GetValues(typeof(BriefingNumber)).Length];

        [Space(5)] [Header("Текст для вывода на экран")] [Tooltip("Одна строка = одна фраза.")] [TextArea(3, 15)]
        public string[] TempTextStrings = new string[System.Enum.GetValues(typeof(BriefingNumber)).Length];
        // ---------------------------------------------------------------------------------

        public static StatusController StatusController; // return current status and processing it
        // ---------------------------------------------------------------------------------

        // Можно написать свою коллекцию
        private readonly Sprite[][] _allFaces = new Sprite[System.Enum.GetValues(typeof(BriefingNumber)).Length][];
        private string[][] _allMessages;

        private Sprite[] _facesCurrent;
        private string[] _messagesCurrent;


        private bool _isSpeaking; // Запущен ли брифинг

        private IEnumerator _pointFace;
        private IEnumerator _pointMessage;

        // Инстанс корутины "медленной печати"
        private Coroutine c_Print;

        //--------------------------------------

        #region Subscriptions

        private void OnEnable()
        {            
            StatusController = new StatusController();
            
            GameManager.Instance.GameInit += BriefingLoadResources;
            GameManager.Instance.GameStart += BriefingStart;
        }

        private void OnDisable()
        {
            GameManager.Instance.GameInit -= BriefingLoadResources;
            GameManager.Instance.GameStart -= BriefingStart;
        }

        // Убрал отписку от события, т.к. это сделано централизованно в GameManager

        #endregion

        // --- Запуск брифинга

        private void BriefingStart(BriefingNumber num)
        {
            BriefingPrepare(_allFaces[(int) num], _allMessages[(int) num]);
        }

        private void BriefingStart()
        {
            BriefingStart(BriefingNumber.Start);
        }

        private void BriefingStart(Sprite[] sprites, string[] messages)
        {
            BriefingPrepare(sprites, messages);
        }

        // --- 

        private void BriefingPrepare(Sprite[] sprites, string[] messages)
        {
            if (_isSpeaking) return;
            StatusController.UpStatus(StatusController.Status.Running);

            if (gameObject != null) gameObject.ChangeChildActive(true);
            _isSpeaking = true;
            _facesCurrent = sprites;
            _messagesCurrent = messages;
            _pointFace = _facesCurrent.GetEnumerator();
            _pointMessage = _messagesCurrent.GetEnumerator();
            StartCoroutine(WaitForSpaceKeyDown());
            NextSpeakFrame();
        }

        private void BriefingEnd()
        {
            _isSpeaking = false;
            gameObject.ChangeChildActive(false);
            StatusController.UpStatus(StatusController.Status.Completed);

            //GameManager.Instance.OnResumeInvoke(); // todo при паузе! 
        }

        private void BriefingLoadResources() //todo сделать корутиной
        {
            // инициализация иконок спикера
            for (int i = 0; i < PathToFolderFace.Length; i++)
            {
                _allFaces[i] = Resources.LoadAll<Sprite>(PathToFolderFace[i]);
            }

            // todo тут метод заполнения строковых переменных из метода локализатора / Реализовать!
            // Объявляем размерность первого ранга = заданной размерности массива
            _allMessages = new string[TempTextStrings.Length][];
            // Инициализирую вложенные массивы
            for (int j = 0; j < _allMessages.GetLength(0); j++)
            {
                _allMessages[j] = TempTextStrings[j].Split('\n');
            }
        }

        private void NextSpeakFrame()
        {
            bool f_exist = false;
            bool m_exist = false;

            //todo предусмотреть, что может быть пустое сообщение. Оставлять тогда последнее сообщение и менять лицо спикера

            if (_pointFace.MoveNext())
            {
                // todo Тут смена лица спикера. Либо эффекты сюда, либо в аниматор и активировать отсюда.
                BriefingFace.sprite = (Sprite) _pointFace.Current;
                f_exist = true;
            }

            if (_pointMessage.MoveNext())
            {
                if (!System.String.IsNullOrEmpty((string) _pointMessage.Current))
                {
                    c_Print = StartCoroutine(PrintTextSlowly((string) _pointMessage.Current, 1 / SpeedText));
                    m_exist = true;
                }
            }
            // Если картинок будет больше, чем сообщений - просто выводим сообщение полностью
            else BriefingText.text = _messagesCurrent.Last();

            if (!f_exist && !m_exist) BriefingEnd();
        }

        private IEnumerator PrintTextSlowly(string text, float speed)
        {
            int i = 0;
            int count = text.Length;

            while (++i < count)
            {
                BriefingText.text = text.Substring(0, i);
                yield return new WaitForSeconds(speed);
            }
        }

        private IEnumerator WaitForSpaceKeyDown()
        {
            while (_isSpeaking)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StopCoroutine(c_Print);
                    NextSpeakFrame();
                }

                yield return null;
            }
        }
    }
}