using FlyBattle.Controllers;
using FlyBattle.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    public class PvPGameEndController : MonoBehaviour
    {
        [SerializeField] private Text player1Name;
        [SerializeField] private Text player2Name;
        [SerializeField] private Text gameResult;
        [SerializeField] private Text score;
        [SerializeField] private Text currentPlayerName;
        [SerializeField] private Image panel;
        [SerializeField] private Button buttonExit;

        [SerializeField] private Color colorWin = Color.blue;
        [SerializeField] private Color colorLose = Color.red;


        private void OnEnable()
        {
            buttonExit.onClick.AddListener(GoToMenu);

            if (GameManager.Instance == null) return;
            GameManager.Instance.GameWin += ShowWinPanel;
            GameManager.Instance.GameDef += ShowDefPanel;
        }

        private void OnDisable()
        {
            buttonExit.onClick.RemoveListener(GoToMenu);

            if (GameManager.Instance == null) return;
            GameManager.Instance.GameWin -= ShowWinPanel;
            GameManager.Instance.GameDef -= ShowDefPanel;
        }

        private void ShowWinPanel()
        {
            EndGameLoadInfo();
            gameResult.text = "Victory!"; //todo Локализатор
            panel.color = colorWin;
            gameObject.ChangeChildActive(true);
        }

        private void ShowDefPanel()
        {
            EndGameLoadInfo();
            gameResult.text = "Defeat!"; //todo Локализатор
            panel.color = colorLose;
            gameObject.ChangeChildActive(true);
        }

        private void EndGameLoadInfo()
        {
            var data = GameManager.Instance.GameData;
            player1Name.text = data.Player1.Name;
            player2Name.text = data.Player2?.Name;
            currentPlayerName.text = ProfileController.CurrentProfile.Name;
            score.text = $"{data.P1W} : {data.P2W}"; //todo ЛокализаторМенеджер
        }

        public void GoToMenu()
        {
            SceneController.Instance.FadeAndLoadScene(SceneNames.Menu);
        }
    }
}