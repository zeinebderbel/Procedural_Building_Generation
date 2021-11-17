using UnityEngine;

public class PrimitiveBuilding : MonoBehaviour
{
    public Material mat;
    public Vector3[] vertices;
    public int[] triangles;
    int nbSides;
    //Radius
    float r;
    //Height
    float h;
    public Vector2[] uvs;
    Vector3 center;
    private bool roof = false;
    //Setting the parameters of user's given values
    public void Initialize(int? nbSides, float r, float h, Vector3 center)
    {
        this.nbSides = nbSides.Value;
        this.r = r;
        this.h = h;
        this.center = center;
    }

    public void BuildPrimitive()
    {
        //Setting the vertices' and uvs length
        int nbVertex = nbSides * 2 + 2;
        vertices = new Vector3[nbVertex];
        uvs = new Vector2[nbVertex];

        var pi = Mathf.PI;
        float teta;
        //The uvs X coordination variation step
        float uvXCoord = 1f / (nbSides - 1);

        //Creating vertices and uvs
        for (int i = 0; i < nbSides * 2; i = i + 2)
        {
            teta = pi * i / nbSides;
            vertices[i] = center + new Vector3(r * Mathf.Cos(teta), h, r * Mathf.Sin(teta));
            vertices[i + 1] = center + new Vector3(r * Mathf.Cos(teta), 0, r * Mathf.Sin(teta));
            uvs[i] = new Vector2(i * uvXCoord, 0);
            uvs[i + 1] = new Vector2(i * uvXCoord, 1);
        }
        //Creating the top and bottom centers vertices (we don't need the uvs here because we don't want to include the top and bottom surfaces for texturing)
        vertices[nbSides * 2] = center + new Vector3(0, h, 0);
        vertices[nbSides * 2 + 1] = center;

        //Setting triangles length
        triangles = new int[nbSides * 12];
        int ti = 0;
        //Creating sides triangles
        for (int x = 0, vi = 0; x < nbSides; x++, vi += 2)
        {
            if (x == nbSides - 1)
            {
                triangles[ti] = 0;
                triangles[ti + 1] = vi + 1;
                triangles[ti + 2] = vi;
                triangles[ti + 3] = 0;
                triangles[ti + 4] = 1;
                triangles[ti + 5] = vi+1;
            }
            else
            {
                triangles[ti] = vi + 1;
                triangles[ti + 1] = vi;
                triangles[ti + 2] = vi + 2;
                triangles[ti + 3] = vi + 3;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = vi + 2;
            }
            ti += 6;
        }

        //Creating top and bottom surfaces triangles
        for (int x = 0, vi = 0; x < nbSides; x++, vi += 2)
        {
            if (x == nbSides - 1)
            {
                triangles[ti] = nbSides * 2;
                triangles[ti + 1] = 0;
                triangles[ti + 2] = vi;
                triangles[ti + 3] = nbSides * 2 + 1;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = 1;
            }
            else
            {
                triangles[ti] = nbSides * 2;
                triangles[ti + 1] = vi + 2;
                triangles[ti + 2] = vi;
                triangles[ti + 3] = nbSides * 2 + 1;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = vi+3;

            }

            ti += 6;
        }

        //Finishing up with configuring our object with the right mesh 
        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;
        
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<Renderer>().material = mat;
        msh.uv = uvs;
        msh.RecalculateNormals();
    }

    public void BuildRoof()
    {
        //Build the roof of the building as a Primitive
        
        vertices = new Vector3[nbSides + 1];
        var pi = Mathf.PI;
        float teta;
        for (int i = 0, angle = 0; i < nbSides; i++, angle+=2)
        {
            teta = pi * angle / nbSides;
            vertices[i] = center + new Vector3(r * Mathf.Cos(teta), 0, r * Mathf.Sin(teta));
        }
        vertices[nbSides] = center + new Vector3(0, h, 0);
        triangles = new int[nbSides * 3];
        //One triangle per side
        for (int i = 0, ti = 0; i < nbSides; i++, ti+=3)
        {
            if(i == nbSides - 1)
            {
                triangles[ti] = 0;
                triangles[ti + 1] = i;
                triangles[ti + 2] = nbSides;
            }
            else
            {
                triangles[ti] = i+1;
                triangles[ti + 1] = i;
                triangles[ti + 2] = nbSides;
            }
        }
        Mesh msh = new Mesh();
        roof = true;
        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<Renderer>().material = mat;
        msh.RecalculateNormals();

    }
}
