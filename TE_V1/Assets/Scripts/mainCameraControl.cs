using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainCameraControl : MonoBehaviour
{

    public GameObject planetCamera;
    planetCameraControl PC_Script;
    public GameObject cubeSplit;
    public int Cube_Num { get; set; }

    public float speedH = 4.0f;
    public float speedV = 4.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Start()
    {
        PC_Script = planetCamera.GetComponent<planetCameraControl>();
        //Debug.Log(PC_Script.Small_cubes);
        //for (int i = 0; i < PC_Script.Small_cubes; i++)
        //{
        //    Instantiate(cubeSplit, this.transform.position, Quaternion.identity);
        //}
        Debug.Log(Cube_Num);
        for (int i = 0; i < 40; i++)
        {
            Instantiate(cubeSplit, this.transform.position, Quaternion.identity);
        }
    }

    void Update()
    {

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        transform.Translate(0, moveHorizontal / 6, moveVertical / 6);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("planet"))
        {
            Debug.Log("changeScene");
            SceneManager.LoadScene("planetScene");         
        }

        if (other.gameObject.CompareTag("walls"))
        {
            this.transform.Translate(0, 0, -100);
        }
    }



}
