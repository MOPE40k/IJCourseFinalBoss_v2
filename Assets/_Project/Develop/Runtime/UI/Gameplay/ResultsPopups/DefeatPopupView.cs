using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class DefeatPopupView : PopupViewBase
    {
        // Delegates
        public event Action ContinueClicked = null;
        public event Action RestartClicked = null;

        [Header("References:")]
        [SerializeField] private TMP_Text _titleText = null;
        [SerializeField] private Button _continueButton = null;
        [SerializeField] private Button _restartButton = null;

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        public void SetTitle(string text)
            => _titleText.SetText(text);

        public void OnContinueButtonClicked()
            => ContinueClicked.Invoke();

        public void OnRestartButtonClicked()
            => RestartClicked.Invoke();

        protected override void OnPreShow()
        {
            base.OnPreShow();

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
    }
}