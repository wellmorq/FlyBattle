using System.Linq;
using FlyBattle.Controllers;
using FlyBattle.Utils;
using ScriptablePattern;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    [RequireComponent(typeof(Button))]
    public class BattleLoader : MonoBehaviour
    {
        #region Variables

        [Tooltip("Тип противостояния")]
        public Consts.BattleType battleType = Consts.BattleType.PvE;

        [Tooltip("Загружаемый уровень")]
        public SceneNames levelName;

        [Tooltip("Профиль противника")]
        public Profile enemyProfile;

        [Tooltip("Максимальное количество раундов")]
        public int maxRoundCount = 1;

        [SerializeField] private bool banAutoStart;
        
        private Button _btn;

        #endregion

        [SerializeField] private MaxRoundChecker roundChecker;

        private void OnEnable()
        {
            _btn = gameObject.GetComponent<Button>();
            if (!banAutoStart) _btn.onClick.AddListener(SendGameData); // if BanAutoStart if disactive - start on click
            
            if (string.IsNullOrEmpty(enemyProfile.Name)) enemyProfile = Database.Instance.AIPrefs.AIProfiles.First();
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SendGameData()
        {
            GameManager.Instance.LoadNewLevel(GameDataCreator());
        }

        private GameData GameDataCreator()
        {
            if (roundChecker != null) maxRoundCount = roundChecker.MaxRound;
            var sceneName = Consts.c_utils_scenesName + "/" + levelName.ToString();
            var data = new GameData(battleType, maxRoundCount, ProfileController.CurrentProfile, enemyProfile, sceneName);
            return data;
        }
    }
}