using System.Collections.Generic;
using FlyBattle;
using FlyBattle.Controllers;
using FlyBattle.UI;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayer2 : SimpleMenu
{
    [SerializeField] private InputField _nameField;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    [SerializeField] private Button _submitButton;

    [SerializeField] private ObjectSelector<Profile> profileSelector;

    #region Subscriptions

    private void OnEnable()
    {
        if (profileSelector == null)
            profileSelector = new ObjectSelector<Profile>(
                ProfileController.ProfileList,
                new List<Profile> {ProfileController.CurrentProfile}); // список запрещенных профилей


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
        var battleLoader = _submitButton.GetComponent<BattleLoader>();
        if (!battleLoader)
        {
            Debug.LogException(new UnityException($"Missing {nameof(BattleLoader)} component on Submit Button"));
            return;
        }
        
        battleLoader.enemyProfile = profileSelector.SubmitButtonClick(_nameField.text);
        battleLoader.SendGameData();
    }

    private void UpdateUI()
    {
        _nameField.text = profileSelector.GetCurrent() == null ? _nameField.text : profileSelector.GetCurrent().Name;
        _leftArrow.interactable = profileSelector.LeftArrowActiveSelf();
        _rightArrow.interactable = profileSelector.RightArrowActiveSelf();
        _submitButton.interactable = profileSelector.SubmitButtonActiveSelf(_nameField.text);
    }
}