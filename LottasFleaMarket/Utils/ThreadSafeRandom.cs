using System;

namespace LottasFleaMarket.Utils {
    public class ThreadSafeRandom {
        private static readonly Random _global = new Random();
        [ThreadStatic] private static Random _local;

        public ThreadSafeRandom() {
            if (_local != null) return;
            int seed;
            lock (_global) {
                seed = _global.Next();
            }
            _local = new Random(seed);
        }

        public int Next(int maxValue = -1) {
            return maxValue > 0 ? _local.Next(maxValue) : _local.Next();
        }

        public int Next(int minValue, int maxValue) {
            return _local.Next(minValue, maxValue);
        }
        
    }
}