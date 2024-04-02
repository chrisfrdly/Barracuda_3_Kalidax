using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BackgroundMusicSelector
{
    None, TitleTheme, GameTheme
}
public class BackgroundMusicSelect : MonoBehaviour
{
    
    [SerializeField] private BackgroundMusicSelector backgroundMusicSelector;

    private void Start()
    {

        //Play Background Music
        AudioManager.instance.BgPlayOnAwake(backgroundMusicSelector, 0.05f, 0.4f, 0.2f);
    }

        
}

