using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    // Start is called before the first frame update
    public void Kill()
    {
        Destroy(gameObject);
    }
}
