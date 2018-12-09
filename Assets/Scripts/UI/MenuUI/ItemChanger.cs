using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FlyBattle.Controllers;
using FlyBattle.Interface;
using JetBrains.Annotations;
using ScriptablePattern;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    public class ItemChanger : MonoBehaviour, IItemPicker
    {
        [SerializeField] private Button leftArrow;
        [SerializeField] private Button rightArrow;
        [SerializeField] private Image panelImage;
        [SerializeField] private Image itemImage;
        [SerializeField] private GameObject closed;

        [SerializeField] private List<ItemInfo> itemPrefabList;
        [SerializeField] private ItemInfo currentItem;

        private ItemInfo _selectedItem;
        private List<string> _purchasedItems = new List<string>();
        private int _index = -1;

        #region Subscriptions

        private void OnEnable()
        {
            leftArrow.onClick.AddListener(LeftArrowClick);
            rightArrow.onClick.AddListener(RightArrowClick);
        }

        private void OnDisable()
        {
            leftArrow.onClick.RemoveListener(LeftArrowClick);
            rightArrow.onClick.RemoveListener(RightArrowClick);
        }

        #endregion

        private void Start()
        {
            itemImage.type = Image.Type.Simple;
            itemImage.preserveAspect = true;
            itemImage.SetNativeSize();

        }

        private void RightArrowClick()
        {
            if (_index < itemPrefabList.Count - 1)
            {
                currentItem = itemPrefabList[++_index];
            }

            UpdateUI();
        }

        private void LeftArrowClick()
        {
            if (_index > 0)
            {
                currentItem = itemPrefabList[--_index];
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            leftArrow.interactable = _index > 0;
            rightArrow.interactable = _index < itemPrefabList.Count - 1;
            closed.SetActive(!(CanPurchase() || IsPurchased()));
            itemImage.sprite = currentItem?.Prefab.GetComponent<SpriteRenderer>().sprite;

            transform.parent.GetComponent<StoreController>()?.UpdateSubmitButtonState();
        }

        private bool CanPurchase()
        {
            return !IsPurchased() && currentItem.Money <= ProfileController.CurrentProfile.Money &&
                   ProfileController.CurrentProfile.Exp >= currentItem.Exp;
        }

        private bool IsPurchased()
        {
            return _purchasedItems.Contains(currentItem.Guid);
        }


        public void Setup(string item, List<string> purchasedItems)
        {
            currentItem = Database.Instance.GameItems.GetItem(item);
            _selectedItem = Database.Instance.GameItems.GetItem(item);
            _purchasedItems = purchasedItems;


            if (currentItem == null)
            {
                if (_purchasedItems == null) _purchasedItems = new List<string>();
                if (_purchasedItems.Contains(item))
                    _purchasedItems.Remove(item);
                _index = 0;
                currentItem = itemPrefabList.First();
                _purchasedItems.Add(currentItem.Guid);
            }
            else _index = itemPrefabList.IndexOf(Database.Instance.GameItems.GetItem(item));

            UpdateUI();
        }

        public void Create(List<ItemInfo> items)
        {
            itemPrefabList = items;
            if (itemPrefabList.Count < 1)
                Debug.LogException(new UnityException("Пустой список префабов!"));
        }

        public bool CheckSubmit()
        {
            return currentItem != Database.Instance.GameItems.GetItem(ProfileController.CurrentProfile.PlaneInfo) &&
                   (IsPurchased() || CanPurchase());
        }

        public ItemInfo GetCurrentItem()
        {
            return currentItem;
        }

        public List<string> GetPurchasedList()
        {
            return _purchasedItems;
        }
    }
}