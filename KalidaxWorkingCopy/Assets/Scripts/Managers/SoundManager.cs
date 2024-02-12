using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Create public audio clips for the MP3s to be dragged into
    public AudioSource source;
    public AudioClip UIConfirmAudioClip;
    public AudioClip UIDenyAudioClip;
    public AudioClip EInteractionAudioClip;
    public AudioClip ItemsSelectedAudioClip;
    public AudioClip PodUIClickAudioClip;


    // Start is called before the first frame update

    //Create a class for each sound. Each class simply plays its corresponding audio clip
    //In every place a sound is needed, these scrips will be called
    public void PlayUIConfirmSound()
    {
        source.PlayOneShot(UIConfirmAudioClip);
    }

    public void PlayUIDenySound()
    {
        source.PlayOneShot(UIDenyAudioClip);
    }

    public void PlayInteractSound()
    {
        source.PlayOneShot(EInteractionAudioClip);
    }

    public void PlayItemSelectSound()
    {
        source.PlayOneShot(ItemsSelectedAudioClip);
    }

    public void PlayPodClickSound()
    {
        source.PlayOneShot(PodUIClickAudioClip);
    }

}
