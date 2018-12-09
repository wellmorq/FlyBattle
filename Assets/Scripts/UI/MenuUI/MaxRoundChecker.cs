using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    public class MaxRoundChecker : MonoBehaviour
    {
        #region Subscriptions

        private void OnEnable() => _maxRoundInput.onEndEdit.AddListener(SetMaxRoundAmount);
        private void OnDisable() => _maxRoundInput.onEndEdit.RemoveListener(SetMaxRoundAmount);

        #endregion

        /// <summary>
        /// Максимальное количество раундов. Значение предназначено для формирования GameData
        /// </summary>
        public int MaxRound { get; set; } = 1;

        [SerializeField] private InputField _maxRoundInput;

        public void UpRoundAmount()
        {
            if (MaxRound < 8) MaxRound += 2;
            UpdateUI();
        }

        public void DownRoundAmount()
        {
            if (MaxRound > 2) MaxRound -= 2;
            UpdateUI();
        }

        private void SetMaxRoundAmount(string text)
        {
            MaxRound = !string.IsNullOrEmpty(text) ? int.Parse(text) : 1;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _maxRoundInput.text = MaxRound.ToString();
        }
    }
}