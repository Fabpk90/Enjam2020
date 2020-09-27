using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerNPC : MonoBehaviour
{

    private FMOD.Studio.EventInstance fmodinstance;

    [FMODUnity.EventRef]
    public string footstepsEvent;

    public void AudioFootsteps()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(footstepsEvent);
        fmodinstance.start();
    }

    // Update is called once per frame
    void Update()
    {
       fmodinstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }
}
