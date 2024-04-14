using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{

    public bool populated;
    // Start is called before the first frame update
    void Start()
    {
        populated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool setIsPopulated(bool isPopulated)
    {
        populated = isPopulated;

        return populated;
    }


}
