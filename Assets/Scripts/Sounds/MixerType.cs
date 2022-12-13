﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class MixerType
{
    [SerializeField] private MixerTypes _mixerType;
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private int _soundsCountLimit;

    public MixerTypes Type => _mixerType;
    public AudioMixerGroup Mixer => _mixer;
    public int SoundsCountLimit => _soundsCountLimit;
}
