using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.CommonViews
{
    public class Bar : MonoBehaviour, IView
    {
        [Header("References:")]
        [SerializeField] private Slider _slider = null;
        [SerializeField] private Image _filler = null;

        public void UpdateValue(float sliderValue)
            => _slider.value = sliderValue;

        public void SetFillerColor(Color color)
            => _filler.color = color;
    }
}
