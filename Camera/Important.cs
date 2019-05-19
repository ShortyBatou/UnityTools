using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Important : MonoBehaviour {

    public GameObject objectPosition;
    [Range(0,1)]
    public float facteur;
    private GameObject joueur;
    private Vector2 position;
    private Vector2 positionJoueur;
    private CameraMvmt cam;
    private S_InfoCam info;

    private void Start()
    {
        position = objectPosition.transform.position;
        joueur = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMvmt>();
        if(facteur == 0 || facteur > 1 )
        {
            
            facteur = 0.5f;
        }
    }

    void Update () {

            if(joueur == null)
            {
                joueur = GameObject.FindGameObjectWithTag("Player");
            }
            positionJoueur = joueur.transform.position;
            position = objectPosition.transform.position;
            cam.SetDecalage(position - (positionJoueur * (1-facteur) + position * facteur)) ;
        
	}


}
