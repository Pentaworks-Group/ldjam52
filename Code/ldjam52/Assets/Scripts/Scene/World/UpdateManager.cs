using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scene.World
{
    static class UpdateManager
    {
        private static IUpdateME[] fieldBehaviours;
        private static int index = 0;
        //private static TileBehaviour[] tileBehaviours = new TileBehaviour[1024];
        //private static int tileIndex = 0;

        public static void RegisterBehaviour(IUpdateME fieldBehaviour)
        {
            fieldBehaviours[index] = fieldBehaviour;
            index++;
        }

        public static void ResetArrays(int size)
        {
            fieldBehaviours = new IUpdateME[size];
            index = 0;
            //tileIndex = 0;
        }

        //public static void RegisterTileBehaviour(TileBehaviour fieldBehaviour)
        //{
        //    tileBehaviours[tileIndex] = fieldBehaviour;
        //    tileIndex++;
        //}

        public static void UpdateBehaviours()
        {
            for (int i = 0; i < index; i++)
            {
                fieldBehaviours[i].UpdateME();
            }
            //for (int i = 0; i < tileIndex; i++)
            //{
            //    tileBehaviours[i].UpdateME();
            //}
        }
    }
    
}
