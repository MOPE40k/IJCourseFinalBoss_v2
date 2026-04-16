using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class MainHeroTargetProvider : ITargetProvider
    {
        // Runtime
        private Entity _mainHero = null;

        public MainHeroTargetProvider(MainHeroHolderService mainHeroHolderService)
            => _mainHero = mainHeroHolderService.MainHero;

        public bool TryGetTarget(out Entity target)
        {
            if (_mainHero is null)
            {
                target = null;

                return false;
            }

            target = _mainHero;

            return true;
        }
    }
}
