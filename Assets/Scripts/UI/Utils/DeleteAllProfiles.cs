using FlyBattle.Controllers;
using ScriptablePattern;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.Utils
{
    public class DeleteAllProfiles : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(ProfilesDelete);
        }

        private void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(ProfilesDelete);
        }

        private void ProfilesDelete()
        {
            ProfileController.RemoveAll();
        }
    }
}