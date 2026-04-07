using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression
{
    public class LevelsProgressionService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        // Consts
        private const int FirstLevel = 1;

        // Runtime
        private readonly List<int> _completedLevels = new();

        public LevelsProgressionService(PlayerDataProvider playerDataProvider)
        {
            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);
        }

        // Runtime
        public bool IsLevelCompleted(int levelNumber)
            => _completedLevels.Contains(levelNumber);

        public void AddLevelToCompleted(int levelNumber)
        {
            if (IsLevelCompleted(levelNumber))
                return;

            _completedLevels.Add(levelNumber);
        }

        public bool CanPlay(int levelNumber)
            => levelNumber == FirstLevel || PreviousLevelCompleted(levelNumber);

        private bool PreviousLevelCompleted(int levelNumber)
            => IsLevelCompleted(levelNumber - 1);

        public void ReadFrom(PlayerData data)
        {
            _completedLevels.Clear();

            _completedLevels.AddRange(data.CompletedLevels);
        }

        public void WriteTo(PlayerData data)
        {
            data.CompletedLevels.Clear();

            data.CompletedLevels.AddRange(_completedLevels);
        }
    }
}
