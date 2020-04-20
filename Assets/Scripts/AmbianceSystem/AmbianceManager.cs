using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbianceSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbianceManager : MonoBehaviour
    {
        public delegate void AmbianceManagerEvent();
        public delegate void AmbianceEvent(AudioClip clip, bool loop = false);
        public static AmbianceManagerEvent Reset;
        public static AmbianceEvent Switch;

        public AudioClip DefaultClip;

        AudioSource Audio;

        void Awake()
        {
            Audio = GetComponent<AudioSource>();
        }

        void OnEnable()
        {
            Switch += SwitchTo;
            Reset += ResetToDefault;
        }

        void OnDisable()
        {
            Switch -= SwitchTo;
            Reset -= ResetToDefault;
        }

        void Update()
        {
            if (!Audio.isPlaying)
                ResetToDefault();
        }

        void SwitchTo(AudioClip clip, bool loop = true)
        {
            Audio.clip = clip;
            Audio.loop = loop;
            Audio.Play();
        }

        void ResetToDefault()
        {
            SwitchTo(DefaultClip);
        }
    }
}
