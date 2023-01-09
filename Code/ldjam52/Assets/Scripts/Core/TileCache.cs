using System.Collections.Concurrent;

namespace Assets.Scripts.Core
{
    public class TwoDimensionCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TValue>> actualCache = new ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TValue>>();

        public TValue this[TKey key1, TKey key2]
        {
            get
            {
                return Get(key1, key2);
            }
            set
            {
                Put(key1, key2, value);
            }
        }

        /// <summary>
        /// Gets TValue directly.
        /// Does NOT check if keys exist and thus will thorw.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public TValue Get(TKey key1, TKey key2)
        {
            return this.actualCache[key1][key2];
        }

        public TValue GetSafe(TKey key1, TKey key2)
        {
            if (this.actualCache.TryGetValue(key1, out var innerConatiner))
            {
                if (innerConatiner.TryGetValue(key2, out var storedValue))
                {
                    return storedValue;
                }
            }

            return default;
        }

        public void Put(TKey key1, TKey key2, TValue value)
        {
            this.actualCache[key1][key2] = value;
        }

        public void PutSafe(TKey key1, TKey key2, TValue value)
        {
            if (!this.actualCache.TryGetValue(key1, out var innerContainer))
            {
                innerContainer = new ConcurrentDictionary<TKey, TValue>();

                this.actualCache[key1] = innerContainer;
            }

            innerContainer[key2] = value;
        }
    }
}
