using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{

    private FMOD.Studio.EventInstance fmodinstance;

    [FMODUnity.EventRef]
    public string closeEvent;

    // Start is called before the first frame update
    public void Kill()
    {
        Destroy(gameObject);
    }

    public void closeSound()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(closeEvent);
        fmodinstance.start();
    }
}
