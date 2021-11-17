using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_curved_building : MonoBehaviour
{
    [SerializeField][Range(0, 20)] float width;
    [SerializeField][Range(0, 20)] float height;
    [SerializeField] float numberOfIterations = 1;
    [SerializeField] GameObject[] controlPoints;
    [SerializeField] GameObject testPrefab;
    [SerializeField] Material meshMat;
    [SerializeField] public Texture tex;

    private Vector3[] controlCoor;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] UVs;
    // Start is called before the first frame update
    void Start()
    {
        CreateVertices();
        CreateUVs();
        CreateTriangles();
        Build();
    }

    
    //Get the coordinates froms the control game objects to avoid multiples call to GetComponent<Transform>
    private void ControlCoordinatesInit()
    {
        controlCoor = new Vector3[controlPoints.Length];
        int i = 0;
        foreach(GameObject go in controlPoints)
        {
            controlCoor[i++] = go.transform.position;
        }
    }

    //Function that create aa curve using n control points
    private void CreateCurve()
    {
        if(controlPoints == null)
        {
            Debug.LogError("CreateCurve : Control points missing");
            return;
        }
        ControlCoordinatesInit();
        float u = 1f / 4f;
        float v = 3f / 4f;
        Debug.Log("u = " + u + " v = " + v);
        List<Vector3> newLine = new List<Vector3>();
        //Ajout du point de depart
        //newLine.Add(points[0]);
        for (int iteration = 0; iteration < numberOfIterations; iteration++)
        {
            for (int i = 0; i < controlCoor.Length - 1; i++)
            {
                //On doit instancier 2 nouveaux points
                Vector3 pi34 = v * controlCoor[i];
                //Debug.Log("Pi 3/4 : " + pi34);
                Vector3 pi14 = u * controlCoor[i + 1];
                //Debug.Log("Pi+1 1/4 : " + pi14);
                newLine.Add(pi34 + pi14);
                pi14 = u * controlCoor[i];
                pi34 = v * controlCoor[i + 1];
                newLine.Add(pi14 + pi34);
            }
            //pour la prochaine iteration on travail sur la nouvelle liste.
            controlCoor = new Vector3[newLine.Count];
            newLine.CopyTo(controlCoor);
            newLine.Clear();
        }
        //newLine.Add(points[points.Count - 1]);

    }

    //Use the the list of points created by CreateCurve() to add the new point.
    private void CreateVertices()
    {
        //Create the initial curve
        CreateCurve();

        if(controlPoints.Length == controlCoor.Length)
        {
            Debug.LogError("BuildCurvedBuilding : CreateCurve returned the tab without upating it");
            return;
        }

        //Initiating vertices tab
        vertices = new Vector3[controlCoor.Length * 4];

        //Passing the 1st line
        controlCoor.CopyTo(vertices, 0);

        //Setup the boundries
        Vector3 bottomRightCorner = new Vector3(width, 0, 0);
        Vector3 topCorner = new Vector3(0, height, 0);
        Vector3 topRightCorner = new Vector3(width, height, 0);

        //We need 3 more lines in order to generate the triangles for the whole building.
        int origWidth = controlCoor.Length, origHeight = controlCoor.Length * 2, heightWidth = controlCoor.Length * 3;
        for (int orig = 0; orig < controlCoor.Length; orig++, origWidth++, origHeight++, heightWidth++)
        {
            vertices[origWidth] = vertices[orig] + bottomRightCorner;
            vertices[origHeight] = vertices[orig] + topCorner;
            vertices[heightWidth] = vertices[orig] + topRightCorner;
        }

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    GameObject go = Instantiate(testPrefab, vertices[i], Quaternion.identity);
        //    go.GetComponent<MeshRenderer>().material.color = Color.cyan;
        //    go.name = i.ToString();
        //}
    }
    private void CreateUVs()
    {
        int origWidth = controlCoor.Length, origHeight = controlCoor.Length * 2, heightWidth = controlCoor.Length * 3;
        UVs = new Vector2[controlCoor.Length * 4];
        var uvXCoord = 1 / (controlCoor.Length * 2) - 1;
        for(int i = 0; i < controlCoor.Length; i ++, origWidth++, origHeight++, heightWidth++)
        {
            UVs[origHeight] = new Vector2(i * uvXCoord, 0);
            UVs[i] = new Vector2(i * uvXCoord, 1);
            UVs[heightWidth] = new Vector2(1-(i * uvXCoord), 0);
            UVs[origWidth] = new Vector2(1 - (i * uvXCoord), 1);
        }
    }
    //Use the vertices tab to create the triangles
    private void CreateTriangles()
    {
        if(vertices == null)
        {
            Debug.LogError("CreatingTriangles : vertices tab not defined");
            return;
        }

        //Initiating triangles tab
        int nbCarresPerSide = (vertices.Length / 4);
        int nbTriangles = nbCarresPerSide -1;
        nbTriangles = ((nbTriangles * 2) * 4) + 4;
        triangles = new int[nbTriangles * 3];

        int ti = 0;
        //Each iteration we make all 4 sides of a part of the building
        for (int i = 0; i < nbCarresPerSide - 1; i++, ti+=24)
        {
            //Right side
            triangles[ti] = i;
            triangles[ti+1] = i + nbCarresPerSide * 2;
            triangles[ti+2] = i + 1;
            triangles[ti+3] = i + (nbCarresPerSide * 2);
            triangles[ti+4] = i + (nbCarresPerSide * 2) + 1;
            triangles[ti + 5] = i + 1;

            //Top side
            triangles[ti + 6] = i + nbCarresPerSide * 2;
            triangles[ti + 7] = i + nbCarresPerSide * 3;
            triangles[ti + 8] = i + (nbCarresPerSide * 2) + 1;
            triangles[ti + 9] = i + nbCarresPerSide * 3;
            triangles[ti + 10] = i + (nbCarresPerSide * 3) + 1;
            triangles[ti + 11] = i + (nbCarresPerSide * 2) + 1;

            //Left side
            triangles[ti + 12] = i + nbCarresPerSide * 3;
            triangles[ti + 13] = i + nbCarresPerSide;
            triangles[ti + 14] = i + (nbCarresPerSide * 3) + 1;
            triangles[ti + 15] = i + nbCarresPerSide;
            triangles[ti + 16] = i + nbCarresPerSide + 1;
            triangles[ti + 17] = i + (nbCarresPerSide * 3) + 1;

            //Bot side
            triangles[ti + 18] = i + nbCarresPerSide;
            triangles[ti + 19] = i;
            triangles[ti + 20] = i + nbCarresPerSide + 1;
            triangles[ti + 21] = i;
            triangles[ti + 22] = i + 1;
            triangles[ti + 23] = i + nbCarresPerSide + 1;
            int tmps = ti + 23;
            Debug.Log("ti = " + tmps);
        }
        //Set up the front and back boundries
        Debug.Log("ti = " + ti + " triangle[ti-1] "+triangles[ti-1] + " triangle[ti+1] " + triangles[ti+1]);
        Debug.Log("triangles  = " + triangles.Length);
        triangles[ti] = nbCarresPerSide * 2;
        triangles[ti + 1] = 0;
        triangles[ti + 2] = nbCarresPerSide;
        triangles[ti + 3] = nbCarresPerSide * 2;
        triangles[ti + 4] = nbCarresPerSide;
        triangles[ti + 5] = nbCarresPerSide * 3;
  
        triangles[ti + 6] = (nbCarresPerSide-1);
        triangles[ti + 7] = (nbCarresPerSide * 2) + (nbCarresPerSide - 1);
        triangles[ti + 8] = nbCarresPerSide + (nbCarresPerSide - 1);
        triangles[ti + 9] = nbCarresPerSide + (nbCarresPerSide - 1);
        triangles[ti + 10] = (nbCarresPerSide * 2) + (nbCarresPerSide - 1);
        triangles[ti + 11] = (nbCarresPerSide * 3) + (nbCarresPerSide - 1);
    }
    
    private void Build()
    {
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = triangles;
        msh.uv = UVs;
        msh.RecalculateNormals();
        MeshFilter mr = gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = meshMat;
        mr.mesh = msh;
    }

}
