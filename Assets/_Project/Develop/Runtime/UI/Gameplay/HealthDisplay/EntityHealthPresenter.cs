using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay
{
    public class EntityHealthPresenter : IPresenter
    {
        // Consts
        private readonly Color MainHeroTeamColor = Color.green;
        private readonly Color EnemiesTeamColor = Color.red;

        // References
        private readonly Entity _entity = null;
        private readonly BarWithText _bar = null;

        private ReactiveVariable<float> _maxHealth = null;
        private ReactiveVariable<float> _currentHealth = null;
        private ReactiveVariable<Teams> _team = null;

        private List<IDisposable> _disposables = null;

        public EntityHealthPresenter(Entity entity, BarWithText bar)
        {
            _entity = entity;
            _bar = bar;
        }

        public BarWithText Bar => _bar;

        public void Initialize()
        {
            _maxHealth = _entity.MaxHealth;
            _currentHealth = _entity.CurrentHealth;

            _team = _entity.Team;

            _disposables = new List<IDisposable>()
            {
                _currentHealth.Subscribe(OnCurrentHealthChanged),
                _currentHealth.Subscribe(OnCurrentHealthChanged),
                _currentHealth.Subscribe(OnCurrentHealthChanged)
            };

            UpdateHealth();
            UpdateFillerColorBy(_team.Value);
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();
        }

        private void UpdateHealth()
        {
            _bar.UpdateSlider(_currentHealth.Value / _maxHealth.Value);
            _bar.UpdateText(_currentHealth.Value.ToString("0"));
        }

        private void UpdateFillerColorBy(Teams team)
        {
            if (team == Teams.MainHero)
                _bar.SetFillerColor(MainHeroTeamColor);
            else if (team == Teams.Enemies)
                _bar.SetFillerColor(EnemiesTeamColor);
        }

        private void OnMaxHealthChanged(float oldValue, float newValue)
            => UpdateHealth();

        private void OnCurrentHealthChanged(float oldValue, float newValue)
            => UpdateHealth();

        private void OnTeamChanged(Teams oldValue, Teams newValue)
            => UpdateFillerColorBy(newValue);
    }
}