using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsefulThings
{
    public class LookAtCamera : MonoBehaviour
    {
        Transform lookTarget;
        
        // Start is called before the first frame update
        void Start()
        {
            lookTarget = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(lookTarget);
        }
    }
}
    
