using UnityEngine;

namespace Necromancer.Data
{
    /// <summary>
    /// Данные этапа (врата/подземелье).
    /// </summary>
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Necromancer/Data/Level Data", order = 4)]
    public class LevelData : ScriptableObject
    {
        public int levelId;
        public string levelName;
        public int requiredHeroLevel;
        public int waveCount;
        public WaveData[] waves;
        public int soulReward;
        public int goldReward;
        public EnemyData boss; // null если этап без босса
        public bool unlocksCatastropheMode;
    }

    [System.Serializable]
    public class WaveData
    {
        public int waveIndex;
        public EnemyData[] enemyTypes;
        public int[] counts;
        public float spawnIntervalSeconds;
        public float delayBeforeNextWaveSeconds;
    }
}
