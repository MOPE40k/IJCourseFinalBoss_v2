using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.Control
{
    public class MousePositionOnPlane : IEntityComponent
    {
        public ReactiveVariable<Vector3> Value = null;
    }
}