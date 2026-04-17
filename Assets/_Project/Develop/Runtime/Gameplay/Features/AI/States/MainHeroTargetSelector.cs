using System.Collections.Generic;
using System.Linq;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class MainHeroTargetSelector : ITargetSelector
    {
        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
            => targets.FirstOrDefault(target => target.HasComponent<IsMainHero>());
    }
}