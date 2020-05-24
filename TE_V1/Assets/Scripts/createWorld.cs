using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createWorld : MonoBehaviour
{
    public GameObject planet;
    private Planet[] planetScript;
    GameObject[] new_planet;
    int numPlanet;
    private Vector3 lastPosition = new Vector3(0, 0, 0);
    private Vector3 thisPosition = new Vector3(0, 0, 0);
    private float lastScale = 1;
    private float thisScale = 1;

    void Start()
    {
        numPlanet = 20;
        //planetScript = new Planet[numPlanet];
        new_planet = new GameObject[numPlanet];

        for (int i = 0; i < numPlanet; i++)
        {
            new_planet[i] = Instantiate(planet,
                new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)),
                Quaternion.identity);
            thisScale *= Random.Range(0.5f, 1.5f);
            new_planet[i].transform.localScale = thisScale * new Vector3(1, 1, 1);
            lastPosition = thisPosition;
            lastScale = thisScale;
        }
    }

    void Update()
    {
        //new_planet[2].transform.Translate(0, 0, 0.1f);
        //planetScript[2].planetRadius -= 0.1f;
        //planetScript[2].Initialize();
        //Debug.Log(planetScript[2].planetRadius);

    }
}
