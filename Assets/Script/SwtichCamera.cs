using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwtichCamera : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;

    // Start is called before the first frame update
    void Start()
    {
        vcam1.Priority = 1;
        vcam2.Priority = 0;
        StartCoroutine(SwitchAfterTime() );
    }
    IEnumerator SwitchAfterTime()
    {
        yield return new WaitForSeconds(5);
        vcam1.Priority = 0;
        vcam2.Priority =1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
