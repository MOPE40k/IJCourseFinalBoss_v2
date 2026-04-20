using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntity : MonoBehaviour
    {
        // References
        private CollidersRegistryService _collidersRegistryService = null;

        // Runtime
        private Entity _linkedEntity = null;

        public Entity LinkedEntity => _linkedEntity;

        public void Initialize(CollidersRegistryService collidersRegistryService)
            => _collidersRegistryService = collidersRegistryService;

        public void Link(Entity entity)
        {
            _linkedEntity = entity;

            MonoComponentsRegister(entity);

            ViewsLink(entity);

            CollidersRegister(entity);
        }

        public void Cleanup(Entity entity)
        {
            ViewsCleanup(entity);

            CollidersUnregister(entity);

            _linkedEntity = null;
        }

        private void MonoComponentsRegister(Entity entity)
        {
            MonoEntityRegistrator[] registrators = GetComponentsInChildren<MonoEntityRegistrator>();

            if (registrators != null)
                foreach (MonoEntityRegistrator registrator in registrators)
                    registrator.Register(entity);
        }

        private void ViewsLink(Entity entity)
        {
            EntityView[] views = GetComponentsInChildren<EntityView>();

            if (views != null)
                foreach (EntityView view in views)
                    view.Link(entity);
        }

        private void ViewsCleanup(Entity entity)
        {
            EntityView[] views = GetComponentsInChildren<EntityView>();

            if (views != null)
                foreach (EntityView view in views)
                    view.Cleanup(entity);
        }

        private void CollidersRegister(Entity entity)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            if (colliders != null)
                foreach (Collider collider in colliders)
                    _collidersRegistryService.Register(collider, entity);
        }

        private void CollidersUnregister(Entity entity)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            if (colliders != null)
                foreach (Collider collider in colliders)
                    _collidersRegistryService.Unregister(collider);
        }
    }
}