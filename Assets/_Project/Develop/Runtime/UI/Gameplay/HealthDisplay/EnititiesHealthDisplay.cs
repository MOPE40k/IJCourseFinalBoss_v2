using Assets._Project.Develop.Runtime.UI.CommonViews;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay
{
    public class EnititiesHealthDisplay : ElementsListView<BarWithText>
    {
        private Camera _camera = null;

        private void Awake()
            => _camera = Camera.main;

        public void UpdatePositionFor(BarWithText bar, Vector3 worldPosition)
        {
            Vector3 newPosition = _camera.WorldToScreenPoint(worldPosition);

            bar.transform.position = newPosition;
        }
    }
}