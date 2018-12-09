using System.Collections.Generic;
using FlyBattle.Controllers;
using FlyBattle.Interface;
using Gamelogic.Extensions;
using Gamelogic.Extensions.Algorithms;
using ScriptablePattern;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    public class StoreController : MonoBehaviour
    {
        enum Items
        {
            Planes,
            Drivers
        }

        [SerializeField] private Button _submitButton;
        [SerializeField] private List<IItemPicker> _itemContainer;

        #region Subscription

        private void OnEnable()
        {
            _submitButton.onClick.AddListener(SubmitButtonClick);
        }

        private void OnDisable()
        {
            _submitButton.onClick.RemoveListener(SubmitButtonClick);
        }

        #endregion

        private void Start()
        {
            _itemContainer = transform.GetChildren().ConvertAll(x => x.GetComponent<IItemPicker>());
            _itemContainer.RemoveAllBut(x => x != null);
            
            _itemContainer[(int) Items.Planes].Create(Database.Instance.GameItems.Planes);
            _itemContainer[(int) Items.Drivers].Create(Database.Instance.GameItems.SkyDrivers);

            SetupAll();
            if (Database.Instance.GameItems.GetItem(ProfileController.CurrentProfile.PlaneInfo)?.Prefab == null) SubmitButtonClick();
        }

        private void SubmitButtonClick()
        {
            ProfileController.CurrentProfile.PlaneInfo = _itemContainer[(int) Items.Planes].GetCurrentItem().Guid;
            ProfileController.CurrentProfile.PurchasedPlanes = _itemContainer[(int) Items.Planes].GetPurchasedList();

            ProfileController.CurrentProfile.SkyDriverInfo = _itemContainer[(int) Items.Drivers].GetCurrentItem().Guid;
            ProfileController.CurrentProfile.PurchasedSkyDrivers = _itemContainer[(int) Items.Drivers].GetPurchasedList();

            SetupAll();
            ProfileController.SaveProfile(ProfileController.CurrentProfile);
        }

        public void UpdateSubmitButtonState()
        {
            _submitButton.interactable = _itemContainer.TrueForAll(x => x.CheckSubmit());
        }

        private void SetupAll()
        {
            _itemContainer[(int) Items.Planes].Setup(ProfileController.CurrentProfile.PlaneInfo,
                ProfileController.CurrentProfile.PurchasedPlanes);
            _itemContainer[(int) Items.Drivers].Setup(ProfileController.CurrentProfile.SkyDriverInfo,
                ProfileController.CurrentProfile.PurchasedSkyDrivers);
        }
    }
}