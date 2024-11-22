using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLooper : MonoBehaviour
{
    [SerializeField] public double loopStartTime;
    [SerializeField] public double loopEndTime;

    private int loopStartSamples;
    private int loopEndSamples;
    private int loopLengthSamples;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        loopStartSamples = (int)(loopStartTime * audioSource.clip.frequency);
        loopEndSamples = (int)(loopEndTime * audioSource.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;
    }

    private void Update()
    {
        if (audioSource.timeSamples >= loopEndSamples) { audioSource.timeSamples -= loopLengthSamples; }
    }
}