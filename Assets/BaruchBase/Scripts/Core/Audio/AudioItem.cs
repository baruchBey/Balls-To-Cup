using UnityEngine;

namespace Baruch.Audio
{
    [System.Serializable]
    public struct AudioItem
    {
        public AudioClip Clip;

        public void Play(AudioSource source)
        {
            source.clip = Clip;
            source.Play();
        }
    }
}