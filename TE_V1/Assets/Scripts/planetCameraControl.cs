using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class planetCameraControl : MonoBehaviour
{
    //public int Small_cubes { get; set; }
    int Small_cubes;

    public GameObject mainCamera;
    mainCameraControl MC_script;
    public GameObject mark;
    public GameObject SplitCube;
    public GameObject planet;
    //[HideInInspector]
    //public Vector3 hitPosition;
    private Planet planetScript;
    //NoiseSettings noiseSettings;
    public float speedH = 4.0f;
    public float speedV = 4.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    
    void Start()
    {
        planetScript = planet.GetComponent<Planet>();
        MC_script = mainCamera.GetComponent<mainCameraControl>();

        Small_cubes = 0;
    }

    void Update()
    {
        MC_script.Cube_Num = Small_cubes;
       
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        transform.Translate(0,moveHorizontal/6, moveVertical/6);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bounce"))
        {
            
            Instantiate(mark, this.transform.position + new Vector3(0, 0, 2), this.transform.rotation);
            cube_split();
            planetScript.hitPosition = this.transform.position;
            this.transform.position *= 0.6f;
            Debug.Log("Bounce and hitPosition = " + planetScript.hitPosition);
            Debug.Log(other.gameObject.transform.position);
       
            planetScript.PleaseSmash();         
        }

    }

    void cube_split()
    {

        for (int i = 0; i < 10; i++)
        {
            Instantiate(SplitCube, this.transform.position + new Vector3(0, 0, 1), this.transform.rotation);
            Small_cubes++;
        }
        Debug.Log("new cube created  " + Small_cubes);
    }


}

