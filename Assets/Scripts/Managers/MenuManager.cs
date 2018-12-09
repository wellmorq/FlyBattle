using System;
using System.Collections.Generic;
using FlyBattle.Controllers;
using FlyBattle.Interface;
using FlyBattle.Utils;
using ScriptablePattern;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace FlyBattle.UI
{
    public class MenuManager : MonoBehaviour, ILevelController
    {
        public static event Action<SimpleMenu> ChangePage = delegate { };
        public static event Action ChangeOnPreviousPage = delegate { };

        [Tooltip("С этой страницы начинается загрузка и отчет меню")]
        public SimpleMenu StartPage;

        [SerializeField] private SimpleMenu _menuPage; // Меню для загрузки, если профиль уже выбран

        private static SimpleMenu _currentPage;
        private Stack<SimpleMenu> _stackPages;

        private void Start()
        {
            Reset();
        }

        public void OnNextPage(SimpleMenu newPage)
        {
            if (_currentPage != null)
            {
                _stackPages.Push(_currentPage);
                _currentPage.Close();
            }

            newPage.Open();
            _currentPage = newPage;
            ChangePage(newPage);
        }

        public void OnPreviousPage()
        {
            var newPage = _stackPages?.Pop();
            if (newPage != null)
            {
                _currentPage.Close();
                _currentPage = newPage;
                _currentPage.Open();
            }

            ChangeOnPreviousPage();
        }

        public void Reset()
        {
            OnNextPage(ProfileController.CurrentProfile == null ? StartPage : _menuPage);

            if (_stackPages == null) _stackPages = new Stack<SimpleMenu>();
        }

        public bool CheckReady(GameState state)
        {
            bool flag = false;
            switch (state)
            {
                case GameState.Init:
                    flag = Fader.StatusController.GetStatus().IsComplete();
                    break;
                case GameState.Start:
                    break;
                case GameState.Play:
                    break;
                case GameState.Pause:
                    break;
                case GameState.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            return flag;
        }

        public void Clear()
        {
            _currentPage = null;
            _stackPages.Clear();
        }
    }
}