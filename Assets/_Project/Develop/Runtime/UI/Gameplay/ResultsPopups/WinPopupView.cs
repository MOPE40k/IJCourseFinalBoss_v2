using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class WinPopupView : PopupViewBase
    {
        public event Action ContinueClicked = null;

        [SerializeField] private TMP_Text _titleText = null;
        [SerializeField] private List<Transform> _stars = null;

        public void SetTitle(string text)
            => _titleText.SetText(text);

        public void OnContinueClicked()
            => ContinueClicked.Invoke();

        protected override void ModifyShowAnimation(Sequence animation)
        {
            base.ModifyShowAnimation(animation);

            foreach (Transform star in _stars)
                animation
                    .Append(star.DOScale(1f, 0.3f).SetEase(Ease.OutBack).From(0f))
                    .Join(star.DOLocalRotate(Vector3.forward * 360f, 0.3f, RotateMode.LocalAxisAdd)
                        .SetEase(Ease.OutCubic)
                        .From(Vector3.zero))
                    .AppendInterval(0.1f);
        }
    }
}