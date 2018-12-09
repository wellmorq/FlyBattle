using FlyBattle.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace FlyBattle.Controllers
{
    public class SoundController : Singleton<SoundController>
    {
        [SerializeField]
        private AudioMixer _mixer;

        [SerializeField] private AudioSource _source;
        
        #region StaticMethods

        public static void ChangeAudioAndPlay(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            if (source.enabled) source.Play();
        }

        #endregion
    }
}