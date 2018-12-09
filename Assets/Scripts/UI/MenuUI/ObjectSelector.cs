using System;
using System.Collections.Generic;
using System.Linq;
using FlyBattle.Controllers;
using UnityEngine;

namespace FlyBattle.UI
{
    [Serializable]
    public class ObjectSelector<T>
    {
        [SerializeField] private IEnumerable<T> profiles;
        [SerializeField] private T current;
        [SerializeField] private int index = -1;

        public ObjectSelector(IEnumerable<T> list, IEnumerable<T> forbidden = null)
        {
            profiles = list;
            if (forbidden != null) profiles = profiles.Except(forbidden);
        }


        public void CheckNameInputField(string text)
        {
            current = default(T);
            var flag = string.IsNullOrEmpty(text);
            if (flag) index = -1;
        }

        public T LeftArrowClick()
        {
            if (index <= 0) return default;
            current = profiles.ElementAtOrDefault(--index);
            return current;
        }

        public T RightArrowClick()
        {
            if (index >= profiles.Count() - 1) return default;
            current = profiles.ElementAtOrDefault(++index);
            return current;
        }

        public Profile SubmitButtonClick(string nameField)
        {
            return string.IsNullOrWhiteSpace(nameField) ? default : ProfileController.GetOrAdd(nameField);
        }

        public bool LeftArrowActiveSelf()
        {
            return profiles.ElementAtOrDefault(index - 1) != null;
        }

        public bool RightArrowActiveSelf()
        {
            return profiles.ElementAtOrDefault(index + 1) != null;
        }

        public T GetCurrent()
        {
            if (current == null) return default;
            return current;
        }

        public bool SubmitButtonActiveSelf(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}