using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPigeonAnimation : MonoBehaviour
{
    private FMOD.Studio.EventInstance fmodinstance;

    [FMODUnity.EventRef]
    public string wingsEvent;

    [FMODUnity.EventRef]
    public string footstepsPigeonEvent;

    [FMODUnity.EventRef]
    public string landingEvent;

    [FMODUnity.EventRef]
    public string cooingEvent;


    public void WingsAudioTrigger()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(wingsEvent);
        fmodinstance.start();
    }

    public void FootstepsPigeonAudioTrigger()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(footstepsPigeonEvent);
        fmodinstance.start();
    }

    public void landingEventAudioTrigger()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(landingEvent);
        fmodinstance.start();
    }

    public void cooingEventAudioTrigger()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(cooingEvent);
        fmodinstance.start();
    }
}
