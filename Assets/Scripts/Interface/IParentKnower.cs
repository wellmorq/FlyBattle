using UnityEngine;

namespace FlyBattle.Interface
{
    public interface IParentKnower
    {
        /// <summary>
        /// Сообщает родителю об объекте
        /// </summary>
        /// <param name="parent">GO родителя</param>
        void SetParent(GameObject parent);
    }
}