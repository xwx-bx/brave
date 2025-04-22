using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace WorldTime
{
    public class WorldTimeWatcher : MonoBehaviour
    {
        public static WorldTimeWatcher instance;
        [SerializeField]
        private WorldTime _worldTime;

        [SerializeField]
        private List<Schedule> _schedule;

        private int x = 0;

        private void Start()
        {
            _worldTime.WorldTimeChanged += CheckSchedule;
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= CheckSchedule;
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule =
                _schedule.FirstOrDefault(s =>
                    s.Hour == newTime.Hours &&
                    s.Minute == newTime.Minutes);

            schedule?._action?.Invoke();
        }

        private void Update()
        {
            if(x == 0)
            {
                checkIcon(PlayerManager.instance.time);
            }
        }
        private void checkIcon(TimeSpan time)
        {
            if (time.Hours >= 6 && time.Hours < 18)
            {
                _schedule[0]._action.Invoke();
            }
            else
            {
                _schedule[1]._action.Invoke();
            }
        }
        [Serializable]
        private class Schedule
        {
            public int Hour;
            public int Minute;
            public UnityEvent _action;

        }
    }
}
