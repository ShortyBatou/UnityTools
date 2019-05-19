using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    private static GameObject temporary;
    private void Start()
    {
        temporary = this.gameObject;
    }
    public static void CleanTemporaryObjects()
    {
        foreach (Transform child in temporary.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
