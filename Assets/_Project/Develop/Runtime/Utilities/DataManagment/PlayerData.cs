using Assets._Project.Develop.Runtime.Meta.Features.Sessions;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment
{
    public class PlayerData : ISaveData
    {
        // Runtime
        public Dictionary<CurrencyTypes, int> WalletData = null;
        public Dictionary<SessionEndConditionTypes, int> SessionsResultsData = null;
        public List<int> CompletedLevels = null;
    }
}
