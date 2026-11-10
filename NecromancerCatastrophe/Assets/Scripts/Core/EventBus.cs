using System;
using System.Collections.Generic;
using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Централизованная	event-шина. Разделяет логику и UI, позволяет
    /// компонентам взаимодействовать без жёстких ссылок.
    /// Использование: EventBus.Publish(new SoulCollectedMsg { Amount = 5 });
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _handlers[type] = list;
            }
            list.Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    _handlers.Remove(type);
            }
        }

        public static void Publish<T>(T message)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
                return;

            // Копируем список, чтобы избежать ModifyDuringEnumeration
            var copy = list.ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                try
                {
                    ((Action<T>)copy[i])(message);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[EventBus] Handler exception: {e}");
                }
            }
        }

        public static void Clear()
        {
            _handlers.Clear();
        }
    }

    // ==================== СООБЩЕНИЯ (MESSAGES) ====================

    public struct SoulCollectedMsg
    {
        public int Amount;
        public Vector3 Position;
    }

    public struct EnemyDiedMsg
    {
        public int GoldReward;
        public int SoulReward;
        public Vector3 Position;
        public EnemyData Data;
    }

    public struct MinionSpawnedMsg
    {
        public MinionInstance Minion;
    }

    public struct MinionDiedMsg
    {
        public MinionInstance Minion;
        public Vector3 Position;
    }

    public struct CatastropheFillChangedMsg
    {
        public float FillNormalized; // 0..1
        public bool ReachedMax;
    }

    public struct CatastropheActivatedMsg
    {
        public bool Active;
    }

    public struct PlayerLevelUpMsg
    {
        public int NewLevel;
    }

    public struct PlayerDiedMsg { }
    public struct WaveCompletedMsg { public int WaveIndex; }
    public struct StageCompletedMsg { public int StageId; }
    public struct SkillUsedMsg { public string SkillId; }
    public struct CurrencyChangedMsg { public string CurrencyId; public int Amount; public int NewTotal; }
}
