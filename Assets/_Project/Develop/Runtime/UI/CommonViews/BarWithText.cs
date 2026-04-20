using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.CommonViews
{
    public class BarWithText : MonoBehaviour, IView
    {
        [Header("References:")]
        [SerializeField] private Bar _bar = null;
        [SerializeField] private TMP_Text _text = null;

        public void UpdateText(string text)
            => _text.SetText(text);

        public void UpdateSlider(float value)
            => _bar.UpdateValue(value);

        public void SetFillerColor(Color color)
            => _bar.SetFillerColor(color);
    }
}
