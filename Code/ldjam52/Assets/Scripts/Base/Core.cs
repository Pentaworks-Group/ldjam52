using System;

namespace Assets.Scripts.Base
{
    public class Core
    {
        private static readonly Lazy<Scripts.Core.Game> lazyGame = new Lazy<Scripts.Core.Game>(true);
        public static Scripts.Core.Game Game
        {
            get
            {
                return lazyGame.Value;
            }
        }
    }
}
