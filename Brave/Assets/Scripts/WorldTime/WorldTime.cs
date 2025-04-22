using UnityEngine;
using System;
using System.Collections;

namespace WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField]
        private float _dayLength;

        private TimeSpan _currentTime;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        private int x = 0;

        private void Start()
        {
            _currentTime = PlayerManager.instance.time;
            StartCoroutine(AddMinute());
        }

       
        private void Update()
        {
            if(x == 0)
            {
                _currentTime = PlayerManager.instance.time;
                x = 1;
            }
            if (PlayerManager.instance.player.stats.isDead == false)
            {
                PlayerManager.instance.time = _currentTime;
            }
        }
        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, _currentTime);
            yield return new WaitForSeconds(_minuteLength * 60);
            StartCoroutine(AddMinute());
        }
        
    }
}
