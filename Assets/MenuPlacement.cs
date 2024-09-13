using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlacement : MonoBehaviour
{
    
    public float distanceInFront = 0.25f;

    void Update()
    {
        
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 menuPosition = cameraPosition + cameraForward * distanceInFront;

       
        transform.position = menuPosition;

        
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;

       
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

       
        Vector3 eulerRotation = targetRotation.eulerAngles;
        // eulerRotation.x = 0; 
        // eulerRotation.z = 0; 

        
        transform.rotation = Quaternion.Euler(eulerRotation);

        
        transform.Rotate(30, 180, 0);
    }
}
