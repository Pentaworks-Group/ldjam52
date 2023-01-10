using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core;

namespace Assets.Extensions
{
    public static class TileDirectionExtensions
    {
        public static TileDirection Invert(this TileDirection tileDirection)
        {
            switch (tileDirection)
            {
                case TileDirection.Top: return TileDirection.Bottom;
                case TileDirection.Left: return TileDirection.Right;
                case TileDirection.Bottom: return TileDirection.Top;
                case TileDirection.Right: return TileDirection.Left;
            }

            throw new ArgumentException();
        }
    }
}
