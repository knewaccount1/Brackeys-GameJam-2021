using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float selfDestructTimer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, selfDestructTimer);
    }

}
