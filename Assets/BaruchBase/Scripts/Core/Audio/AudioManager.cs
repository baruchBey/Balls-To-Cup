using UnityEngine;
using Baruch.Audio;
using System.Collections.Generic;

namespace Baruch
{
    public class AudioManager : Singleton<AudioManager>,IInit
    {
        //TODO Repick sounds


        [Save]public static bool AudioEnabled = default;

        private static IReadOnlyDictionary<AudioItemType, AudioItem> _audioDictionary;

        //A,C,E  
        private static readonly float[] _aMin = new float[] { 0.84f, 1f, 1.26f };

        [SerializeField] private AudioItem _pop;
        [SerializeField] private AudioItem _whoop;
        [SerializeField] private AudioItem _blob;
        [SerializeField] private AudioItem _ching;

        private static AudioSource[] _audioSources;
        private static int _counter;

        public void Init()
        {
            _audioSources = GetComponents<AudioSource>();

            _audioDictionary = new Dictionary<AudioItemType, AudioItem>
            {
                {AudioItemType.Pop , _pop },
                {AudioItemType.Whoop , _whoop},
                {AudioItemType.Blob , _blob},
                {AudioItemType.Ching , _ching},
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