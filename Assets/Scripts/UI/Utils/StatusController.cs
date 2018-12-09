using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Algorithms;

namespace FlyBattle.Utils
{
    public class StatusController
    {
        public enum Status
        {
            NotStarted,
            Running,
            Completed
        }
        // ---------------------------------------------------------------------------------

        public event Action<StatusController.Status> OnStatusChange;

        private Dictionary<int, Status> _statusStore; // Массив состояний для BattleController`а
        private int _keyIndex = -1; // Текущий указатель для BattleController`а (кэширует)

        // ---------------------------------------------------------------------------------

        public StatusController()
        {
            GameManager.Instance.StateChanged += OnGameStateChanged;
        }

        ~StatusController()
        {
            GameManager.Instance.StateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState state)
        {
            if (GetStatus().IsComplete()) UpStatus(Status.NotStarted);
        }

        /// <summary>
        /// На какой стадии выполнения находится Декларатор уровня?
        /// </summary>
        /// <returns>
        /// -1 : ещё не приступал,d
        ///  0 : на этапе выполнения,
        ///  1 : выполнен
        /// </returns>
        public StatusController.Status GetStatus()
        {
            var value = Status.NotStarted;

            if (_statusStore == null || _statusStore.IsEmpty())
                _statusStore = new Dictionary<int, Status> {{++_keyIndex, value}};

            if (_keyIndex == -1) _keyIndex = _statusStore.LastOrDefault().Key;
            if (_statusStore.TryGetValue(_keyIndex, out value))
            {
                //if (value.IsComplete()) UpStatus(StatusController.Status.NotStarted);
                return value;
            }

            return UpStatus(StatusController.Status.NotStarted);
        }

        public StatusController.Status UpStatus(StatusController.Status i)
        {
            if (_statusStore == null || _statusStore.IsEmpty())
            {
                _statusStore = new Dictionary<int, StatusController.Status> {{++_keyIndex, i}};
                return i;
            }

            if (_keyIndex == -1) _keyIndex = _statusStore.LastOrDefault().Key;

            if (_statusStore.IsEmpty() || !_statusStore.TryGetValue(_keyIndex, out var result) || result > 0)
            {
                _statusStore.Add(++_keyIndex, i);
                return i;
            }

            _statusStore.Remove(_keyIndex);
            _statusStore.Add(_keyIndex, i);

            OnStatusChange?.Invoke(i);
            return i;
        }
    }

    public static class StatusControllerExtensions
    {
        public static bool IsComplete(this StatusController.Status stat)
        {
            return stat == StatusController.Status.Completed;
        }

        public static bool IsRunning(this StatusController.Status stat)
        {
            return stat == StatusController.Status.Running;
        }

        public static bool IsNotStarted(this StatusController.Status stat)
        {
            return stat == StatusController.Status.NotStarted;
        }
    }
}