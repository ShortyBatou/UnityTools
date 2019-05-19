using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMvmt : MonoBehaviour {

    // le temps que met la camera pour arriver jusqu'à la posisiton
    public Vector2 smoothTime;

    // decalage entre jouer et caméra
    public Vector2 decalage;
    
    // la taille de la caméra
    public float size;
    public float smoothTimeSize;


    public bool follow;

    public S_InfoCam infoCam;

    private Vector2 focusPosition;
    private Vector2 position;
    private Vector2 velocity;

    private Camera cam;
    private GameObject joueur;

    void Start () {
        cam = gameObject.GetComponent<Camera>();
        size = cam.orthographicSize;
        joueur = GameObject.FindGameObjectWithTag("Player");
        follow = true;

        infoCam = new S_InfoCam(size,decalage,smoothTime,smoothTimeSize);

    }
	
	
	void FixedUpdate ()
    {   
        if (joueur == null)
        {
            joueur = GameObject.FindGameObjectWithTag("Player");
            
        }

        float currentSize = Mathf.Lerp(cam.orthographicSize, infoCam.size, Time.deltaTime * smoothTimeSize);
        cam.orthographicSize = currentSize;

        if (follow && joueur!=null)
        {
            // on calcule la trajectoire que doit avoir notre caméra par rapport au joueur
            position.x = Mathf.SmoothDamp(transform.position.x, joueur.transform.position.x + infoCam.decalage.x, ref velocity.x, infoCam.smoothTime.x);
            position.y = Mathf.SmoothDamp(transform.position.y, joueur.transform.position.y + infoCam.decalage.y, ref velocity.y, infoCam.smoothTime.y);

            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
        else
        {
            position.x = Mathf.SmoothDamp(transform.position.x, focusPosition.x, ref velocity.x, infoCam.smoothTime.x);
            position.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref velocity.y, infoCam.smoothTime.y);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

    }


    public void SetDecalage(Vector2 decalage)
    {
        infoCam.decalage = decalage;
    }

    public void SetSize(float size)
    {
        infoCam.size = size;
    }
    public Vector2 GetPosition()
    {
        return position;
    }

    public void FocusEnable(Vector2 newPosition, Vector2 smoothPosition, float size, float smoothTimeSize)
    {
        focusPosition = newPosition;
        infoCam.size = size;
        infoCam.smoothTime = smoothPosition;
        infoCam.smoothTimeSize = smoothTimeSize;
        decalage = new Vector2(0, 0);
        follow = false;
    }
    public void FocusDisable(S_InfoCam info)
    {
        infoCam = info;
        follow = true;
    }
}
