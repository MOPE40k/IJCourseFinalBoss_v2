using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaContainer : MonoBehaviour
    {
        [SerializeField] private RectTransform _parentRectTransform = null;
        private Rect _lastSafeArea = new();

        private void Awake()
        {
            _parentRectTransform ??= GetComponent<RectTransform>();

            UpdateArea();
        }

        private void Update()
        {
            if (Screen.safeArea != _lastSafeArea)
                UpdateArea();
        }

        private void UpdateArea()
        {
            _lastSafeArea = Screen.safeArea;

            Vector2 anchorMin = _lastSafeArea.position;
            Vector2 anchorMax = _lastSafeArea.position + _lastSafeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;

            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _parentRectTransform.anchorMin = anchorMin;
            _parentRectTransform.anchorMax = anchorMax;
        }
    }
}