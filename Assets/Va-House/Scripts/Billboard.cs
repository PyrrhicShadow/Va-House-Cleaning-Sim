using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  From a unity billboarding guide found here: https://gamedevbeginner.com/billboards-in-unity-and-how-to-make-your-own/
/// </summary>
public class Billboard : MonoBehaviour
{
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        Vector3 newRotation = mainCamera.transform.eulerAngles;
        newRotation.x = 0;
        newRotation.z = 0;
        transform.eulerAngles = newRotation;
    }
}