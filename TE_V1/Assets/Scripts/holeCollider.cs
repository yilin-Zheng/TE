using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeCollider : MonoBehaviour
{
    public GameObject planet;
    private Planet planetScript;
    //ShapeGenerator shapeGenerator;

    void Start()
    {
        planetScript = planet.GetComponent<Planet>();
        Debug.Log(planet);
        //shapeGenerator = new ShapeGenerator(planetScript.settings);

        this.transform.position = planetScript.CalculatePointOnPlanet(planetScript.Position_of_hole);
        Debug.Log(this.transform.position + "thissss possssition");
        //this.transform.localScale = new Vector3(planetScript.settings.planetRadius,
        //    planetScript.settings.planetRadius,
        //    planetScript.settings.planetRadius) ;
        this.transform.localScale = new Vector3(5,5,5);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
