using System;
using System.Collections.Generic;

namespace FlyBattle.Interface
{
    public interface IItemPicker
    {
        /// <summary>
        /// Инициализация пикера
        /// </summary>
        /// <param name="item"></param>
        /// <param name="purchasedItems"></param>
        void Setup(string item, List<string> purchasedItems);

        void Create(List<ItemInfo> items);

        bool CheckSubmit();
        ItemInfo GetCurrentItem();
        List<string> GetPurchasedList();
    }
}