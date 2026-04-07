using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class DesktopInput : IInputService
    {
        // Consts
        private const string KeyboardHorizontalAxisName = "Horizontal";
        private const string KeyboardVerticalAxisName = "Vertical";
        private const KeyCode MouseFireButton = KeyCode.Mouse0;

        // Runtime
        public bool IsEnabled { get; set; } = true;

        public Vector3 Direction
        {
            get
            {
                if (IsEnabled == false)
                    return Vector3.zero;

                return new Vector3(
                    Input.GetAxisRaw(KeyboardHorizontalAxisName),
                    0f,
                    Input.GetAxisRaw(KeyboardVerticalAxisName));
            }
        }

        public Vector3 MousePosition
        {
            get
            {
                if (IsEnabled == false)
                    return Vector3.zero;

                return Input.mousePosition;
            }
        }

        public bool IsFire
        {
            get
            {
                if (IsEnabled == false)
                    return false;

                return Input.GetKeyDown(MouseFireButton);
            }
        }
    }
}
