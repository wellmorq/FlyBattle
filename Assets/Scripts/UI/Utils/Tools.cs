using UnityEngine;

namespace FlyBattle.Utils
{
    public static class Tools
    {
        
    }

    public static class ToolsExtensions
    {
        /// <summary>
        /// Изменяем активность дочерних объектов
        /// </summary>
        /// <param name="obj">Родитель</param>
        /// <param name="flag">вкл или выкл</param>
        /// <returns></returns>
        public static void ChangeChildActive(this GameObject obj, bool flag)
        {
            var trans = obj.transform;
            var count = trans.childCount;
            for (int i = 0; i < count; i++)
            {
                trans.GetChild(i).gameObject.SetActive(flag);
            }
        }
    }
}