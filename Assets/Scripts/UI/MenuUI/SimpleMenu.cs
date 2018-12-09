using UnityEngine;

namespace FlyBattle.UI
{
    public abstract class SimpleMenu : MonoBehaviour
    {
        private Canvas _canvas;
        private Canvas Canvas => _canvas ? _canvas : _canvas = GetComponent<Canvas>();

        internal void Open()
        {
            if (Canvas) Canvas.enabled = true;
            else gameObject.SetActive(true);
        }

        internal void Close()
        {
            if (Canvas) Canvas.enabled = false;
            else gameObject.SetActive(false);
        }
    }
}