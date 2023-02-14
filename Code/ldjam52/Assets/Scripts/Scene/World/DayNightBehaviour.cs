using System;

namespace Assets.Scripts.Scene.World
{
    using Assets.Scripts.Core;

    using UnityEngine;

    public class DayNightBehaviour : MonoBehaviour
    {
        private GameState gameState;
        public GameObject sun;

        public float radius = 6;

        public Color daytimeSkyColor = new Color(0.31f, 0.88f, 1f);
        public Color middaySkyColor = new Color(0.58f, 0.88f, 1f);
        public Color nighttimeSkyColor = new Color(0.04f, 0.19f, 0.27f);

        private Single totalDayDuration;
        private Single dayTimeDuration;
        private Single nightTimeDuration;
        private Single duskDuration;

        private float startOfDusk;
        private float startOfNighttime;
        private float startOfSunset;
        private Vector3 centerPoint = Vector3.zero;

        private float relativeTime = 0;
        private float timeOfDay = 0;

        void Update()
        {
            if (Time.timeScale != 0)
            {
                relativeTime = (relativeTime + Time.deltaTime) % totalDayDuration;

                Camera.main.backgroundColor = CalculateSkyColor();

                this.timeOfDay = relativeTime / totalDayDuration;

                this.gameState.World.TimeOfDay = this.timeOfDay;

                float sunangle = this.timeOfDay * 360;

                sun.transform.position = Vector3.zero + Quaternion.Euler(-70, 0, sunangle) * (radius * Vector3.right);

                sun.transform.LookAt(centerPoint);
            }
        }

        private Color CalculateSkyColor()
        {
            float time = relativeTime;

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

        private void Start()
        {
            this.gameState = Base.Core.Game.State;

            this.totalDayDuration = gameState.GameMode.World.TotalDayDuration;
            this.timeOfDay = gameState.World.TimeOfDay;
            this.relativeTime = this.timeOfDay * this.totalDayDuration;

            this.dayTimeDuration = this.totalDayDuration / 2f;
            this.nightTimeDuration = this.dayTimeDuration * 0.7f;
            this.duskDuration = this.dayTimeDuration * 0.15f;

            this.startOfDusk = dayTimeDuration / totalDayDuration;
            this.startOfNighttime = startOfDusk + duskDuration / totalDayDuration;
            this.startOfSunset = startOfNighttime + nightTimeDuration / totalDayDuration;

            this.centerPoint = new Vector3(this.gameState.World.Width / 2, 0, this.gameState.World.Height / 2);
        }
    }
}
