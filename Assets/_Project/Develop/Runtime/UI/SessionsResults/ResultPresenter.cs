using System;
using Assets._Project.Develop.Runtime.Configs.Meta.SessionResult;
using Assets._Project.Develop.Runtime.Meta.Features.Sessions;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.UI.SessionsResults
{
    public class ResultPresenter : IPresenter
    {
        // References
        private readonly IReadOnlyVariable<int> _result = null;
        private readonly SessionEndConditionTypes _resultType = SessionEndConditionTypes.Win;
        private readonly SessionResultIconConfig _config = null;
        private readonly IconTextView _view = null;

        // Runtime
        public IconTextView View => _view;
        private IDisposable _disposable = null;

        public ResultPresenter(
            IReadOnlyVariable<int> result,
            SessionEndConditionTypes resultType,
            SessionResultIconConfig config,
            IconTextView view)
        {
            _result = result;
            _resultType = resultType;
            _config = config;
            _view = view;
        }

        public void Initialize()
        {
            _view.SetIcon(_config.GetSpriteFor(_resultType));

            UpdateValue(_result.Value);

            _disposable = _result.Subscribe(OnValueChanged);
        }

        public void Dispose()
            => _disposable.Dispose();

        private void OnValueChanged(int oldValue, int newValue)
            => UpdateValue(newValue);

        private void UpdateValue(int newValue)
            => _view.SetText(newValue.ToString());
    }
}