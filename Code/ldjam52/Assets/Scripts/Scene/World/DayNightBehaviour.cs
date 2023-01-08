using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scene.World
{
    using UnityEngine;
    using System.Collections;

    public class DayNightBehaviour : MonoBehaviour
    {
        void Start()
        {
        }

        public GameObject sun;

        public float radius = 6;

        public Color daytimeSkyColor = new Color(0.31f, 0.88f, 1f);
        public Color middaySkyColor = new Color(0.58f, 0.88f, 1f);
        public Color nighttimeSkyColor = new Color(0.04f, 0.19f, 0.27f);

        // 1 Day => 60 sec

        // implementing minecraft PC defaults
        public const float daytimeRLSeconds = 30f;
        public const float duskRLSeconds = 4.5f;
        public const float nighttimeRLSeconds = 21f;
        public const float sunsetRLSeconds = 4.5f;
        public const float gameDayRLSeconds = daytimeRLSeconds + duskRLSeconds + nighttimeRLSeconds + sunsetRLSeconds;

        public const float startOfDaytime = 0;
        public const float startOfDusk = daytimeRLSeconds / gameDayRLSeconds;
        public const float startOfNighttime = startOfDusk + duskRLSeconds / gameDayRLSeconds;
        public const float startOfSunset = startOfNighttime + nighttimeRLSeconds / gameDayRLSeconds;


        private float timeRT = 0;
        public float TimeOfDay // game time 0 .. 1
        {
            get { return timeRT / gameDayRLSeconds; }
            set { timeRT = value * gameDayRLSeconds; }
        }

        void Update()
        {
            timeRT = (timeRT + Time.deltaTime) % gameDayRLSeconds;
            Camera.main.backgroundColor = CalculateSkyColor();
            
            float sunangle = TimeOfDay * 360;

            sun.transform.position = Vector3.zero + Quaternion.Euler(-70, 0, sunangle) * (radius * Vector3.right);
            sun.transform.LookAt(Vector3.zero);
        }

        Color CalculateSkyColor()
        {
            float time = TimeOfDay;

            if (time <= 0.25f)
            {
                return Color.Lerp(daytimeSkyColor, middaySkyColor, time / 0.25f);
            }

            if (time <= 0.5f)
            {
                return Color.Lerp(this.middaySkyColor, this.daytimeSkyColor, (time - 0.25f) / 0.25f);
            }

            if (time <= startOfNighttime)
            {
                return Color.Lerp(daytimeSkyColor, nighttimeSkyColor, (time - startOfDusk) / (startOfNighttime - startOfDusk));
            }

            if (time <= startOfSunset)
            {
                return nighttimeSkyColor;
            }

            return Color.Lerp(nighttimeSkyColor, daytimeSkyColor, (time - startOfSunset) / (1.0f - startOfSunset));
        }

    }

}
