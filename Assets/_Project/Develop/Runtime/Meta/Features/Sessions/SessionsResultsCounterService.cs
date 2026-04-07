using System.Collections.Generic;
using System.Linq;
using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Meta.Features.Sessions
{
    public class SessionsResultsCounterService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        // Runtime
        private readonly Dictionary<SessionEndConditionTypes, ReactiveVariable<int>> _sessionsResults = null;

        public SessionsResultsCounterService(
            Dictionary<SessionEndConditionTypes, ReactiveVariable<int>> sessionsResults,
            PlayerDataProvider playerDataProvider)
        {
            _sessionsResults = new Dictionary<SessionEndConditionTypes, ReactiveVariable<int>>(sessionsResults);

            playerDataProvider.RegisterReader(this);
            playerDataProvider.RegisterWriter(this);
        }

        // Runtime
        public SessionEndConditionTypes[] AvailableSessionEndConditions => _sessionsResults.Keys.ToArray();

        public IReadOnlyVariable<int> GetCondition(SessionEndConditionTypes type)
            => _sessionsResults[type];

        public void Add(SessionEndConditionTypes type)
            => _sessionsResults[type].Value++;

        public void Reset()
        {
            foreach (KeyValuePair<SessionEndConditionTypes, ReactiveVariable<int>> pair in _sessionsResults)
                pair.Value.Value = 0;
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (KeyValuePair<SessionEndConditionTypes, int> sessionResult in data.SessionsResultsData)
                if (_sessionsResults.ContainsKey(sessionResult.Key))
                    _sessionsResults[sessionResult.Key].Value = sessionResult.Value;
                else
                    _sessionsResults.Add(sessionResult.Key, new ReactiveVariable<int>(sessionResult.Value));
        }

        public void WriteTo(PlayerData data)
        {
            foreach (KeyValuePair<SessionEndConditionTypes, ReactiveVariable<int>> sessionResult in _sessionsResults)
                if (data.SessionsResultsData.ContainsKey(sessionResult.Key))
                    data.SessionsResultsData[sessionResult.Key] = sessionResult.Value.Value;
                else
                    data.SessionsResultsData.Add(sessionResult.Key, sessionResult.Value.Value);
        }
    }
}