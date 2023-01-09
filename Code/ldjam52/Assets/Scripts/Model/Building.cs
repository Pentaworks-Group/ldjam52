using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Model
{
    public abstract class Building
    {
        public String TemplateReference { get; set; }
        public Vector3 Position { get; set; }
    }
}
