using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 8;
    public Material material_a;


    //public ShapeGenerator shapeGenerator;
    [HideInInspector]
    private float planetRadius;
    public float iniRadius = 20;
    public float P_R
    {
        get { return planetRadius; }
        set { planetRadius = value; }
    }
   
    Color planetColour;
    Noise noise = new Noise();

    float strength = 2f;
    [Range(1, 8)]
    int numLayers = 1;
    float baseRoughness;
    float roughness = 2;
    float persistence = .5f;
    float minValue;
    public Vector3 centre;
    public Vector3 Position_of_hole = new Vector3(1, 0, 0);

    [HideInInspector]
    public Vector3 hitPosition;
    [SerializeField, HideInInspector]
    public MeshFilter[] meshFilters;
    //public TerrainFace[] terrainFaces;
    public GameObject[] meshObjs;
    new MeshCollider collider;
    new Rigidbody rigidbody;

    //public Mesh mesh;
    public Vector3[] vertices;
    int[] triangles;


    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= roughness;
            amplitude *= persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - minValue);
        return noiseValue * strength;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {

        float elevation;

        elevation = Evaluate(pointOnUnitSphere);

        return pointOnUnitSphere * planetRadius * (1 + elevation);
    }


    void Awake()
    {
        planetRadius = iniRadius;//Random.Range(15, 30);
        meshFilters = new MeshFilter[6];
        meshObjs = new GameObject[6];
        strength = 0.3f;
        baseRoughness = 1f;
        roughness = 2;
        persistence = .5f;
        planetColour = new Color(Random.Range(0.0f,1.0f), 0.4f, 0.6f, 0.3f);
        Initialize();
    }

    void Update()
    {
        if (planetRadius > 20.1f)
        {
            //Debug.Log(settings.centre + "centrrrre");
            hitPosition = centre;
            planetRadius -= 0.1f;
            baseRoughness *= Random.Range(0.8f, 1.2f);
            Initialize();
        }
        else
        {
            baseRoughness = 1f;
        }
        if (Vector3.Distance(CalculatePointOnPlanet(Position_of_hole), hitPosition) < 10)
        {
            Debug.Log("hithithit");
            gameObject.SetActive(false);

            for (int i = 0; i < 6; i++)
            {
                Destroy(meshFilters[i].sharedMesh);
                SceneManager.LoadScene("worldScene");
            }
        }
    }

    public void Initialize()
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshObjs[i] == null)
            {
                meshObjs[i] = new GameObject("mesh");
                meshObjs[i].transform.parent = transform;
                meshObjs[i].transform.position = this.transform.position;
                meshObjs[i].transform.localScale = this.transform.localScale;
                meshObjs[i].tag = "Bounce";
            }

            if (meshFilters[i] == null)
            {
                meshFilters[i] = meshObjs[i].AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshObjs[i].AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                rigidbody = meshObjs[i].AddComponent<Rigidbody>();
                collider = meshObjs[i].AddComponent<MeshCollider>();
            }

            meshFilters[i].sharedMesh = ConstructMesh(directions[i], meshFilters[i].sharedMesh);
            collider = meshObjs[i].GetComponent<MeshCollider>();
            collider.sharedMesh = meshFilters[i].sharedMesh;
            collider.isTrigger = false;
            rigidbody = meshObjs[i].GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial.color = planetColour;

        }
    }

    public void PleaseSmash()
    {
        Debug.Log("please smash" + hitPosition);
        planetRadius = 21;
        centre = hitPosition;
    }

    public Mesh ConstructMesh(Vector3 localUp, Mesh mesh)
    {
        Vector3 axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        Vector3 axisB = Vector3.Cross(localUp, axisA);

        vertices = new Vector3[resolution * resolution];
        triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution;
                    triangles[triIndex + 2] = i + resolution + 1;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + resolution + 1;
                    triangles[triIndex + 5] = i + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

}