using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour {

    private CameraMvmt cam;
    private Vector2 position;
    public GameObject objPosition;
    public Vector2 smoothPosition;
    public float size;
    public float smoothTimeSize;
    private bool allReadyFocus;
    private S_InfoCam info;

    void Start()
    {
        position = objPosition.transform.position;
        allReadyFocus = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !allReadyFocus)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMvmt>();
            info = cam.infoCam;
            print(info.size);
            cam.FocusEnable(position, smoothPosition, size, smoothTimeSize);
            allReadyFocus = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && allReadyFocus)
        {
            cam.FocusDisable(info);
            allReadyFocus = false;
        }
    }
}
