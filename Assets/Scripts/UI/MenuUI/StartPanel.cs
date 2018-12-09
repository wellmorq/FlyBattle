using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using FlyBattle.Controllers;
using ScriptablePattern;
using UnityEngine;
using UnityEngine.UI;

namespace FlyBattle.UI
{
    public class StartPanel : SimpleMenu
    {
        [SerializeField] private InputField _nameField;
        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;
        [SerializeField] private Button _submitButton;

        [SerializeField] private ObjectSelector<Profile> profileSelector;

        #region Subscriptions

        private void OnEnable()
        {
            if (profileSelector == null) profileSelector = new ObjectSelector<Profile>(ProfileController.ProfileList);

            _leftArrow.onClick.AddListener(LeftArrowClick);
            _rightArrow.onClick.AddListener(RightArrowClick);
            _submitButton.onClick.AddListener(SubmitButtonClick);
            _nameField.onValueChanged.AddListener(CheckNameInputField);

            UpdateUI();
        }

        private void OnDisable()
        {
            _leftArrow.onClick.RemoveListener(LeftArrowClick);
            _rightArrow.onClick.RemoveListener(RightArrowClick);
            _submitButton.onClick.RemoveListener(SubmitButtonClick);
            _nameField.onValueChanged.RemoveListener(CheckNameInputField);
        }

        #endregion

        private void CheckNameInputField(string text)
        {
            profileSelector.CheckNameInputField(text);
            UpdateUI();
        }

        private void LeftArrowClick()
        {
            profileSelector.LeftArrowClick();
            UpdateUI();
        }

        private void RightArrowClick()
        {
            profileSelector.RightArrowClick();
            UpdateUI();
        }

        private void SubmitButtonClick()
        {
            ProfileController.CurrentProfile = profileSelector.SubmitButtonClick(_nameField.text);
        }

        private void UpdateUI()
        {
            _nameField.text = profileSelector.GetCurrent() == null ? _nameField.text : profileSelector.GetCurrent().Name;
            _leftArrow.interactable = profileSelector.LeftArrowActiveSelf();
            _rightArrow.interactable = profileSelector.RightArrowActiveSelf();
            _submitButton.interactable = profileSelector.SubmitButtonActiveSelf(_nameField.text);
        }
    }
}