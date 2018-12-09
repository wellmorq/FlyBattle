using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.UI
{
    public class SPGameEndController : MonoBehaviour
    {
        private void OnEnable()
        {
            if (GameManager.Instance != null) GameManager.Instance.GameDef += ShowDefPanel;
        }
        
        private void OnDisable()
        {
            if (GameManager.Instance != null) GameManager.Instance.GameDef -= ShowDefPanel;
        }

        private void ShowDefPanel()
        {
            gameObject.ChangeChildActive(true);
        }
        
        
    }
}