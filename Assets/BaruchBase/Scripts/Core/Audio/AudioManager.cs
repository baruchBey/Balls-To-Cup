using UnityEngine;
using Baruch.Audio;
using System.Collections.Generic;

namespace Baruch
{
    public class AudioManager : Singleton<AudioManager>,IInit
    {


        [Save] public static bool AudioEnabled = true;


        private static IReadOnlyDictionary<AudioItemType, AudioItem> _audioDictionary;

        //A,C,E  
        private static readonly float[] _aMin = new float[] { 0.84f, 1f, 1.26f };

        [SerializeField] private AudioItem _pop;
        [SerializeField] private AudioItem _blob;
        [SerializeField] private AudioItem _ching;

        private static AudioSource[] _audioSources;
        private static int _counter;

        public void Init()
        {
            _audioSources = GetComponents<AudioSource>();

            _audioDictionary = new Dictionary<AudioItemType, AudioItem>
            {
                {AudioItemType.StarPop , _pop },
                {AudioItemType.BallInCup , _blob},
                {AudioItemType.LevelCompleteGlass , _ching},
            };
        }

        

        public void Play(AudioItemType itemType)
        {
            if (!AudioEnabled)
                return;

            float pitch = _aMin[++_counter % _aMin.Length];

            AudioSource currentSource = default;
            foreach (var item in _audioSources)
            {
                if (item.isPlaying)
                    continue;
                currentSource = item;
                break;
            }
            if (!currentSource)
                return;

            currentSource.pitch = pitch;
            _audioDictionary[itemType].Play(currentSource);
        }

    }
}