using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourNonLineaire : MonoBehaviour
{
    //[SerializeField] [Range(0, 10)] int floorsNumber = 1;
    [SerializeField] [Range(0, 10)] float step;
    [SerializeField] string floorsAxiom;
    [SerializeField] string footprintAxiom;
    [SerializeField] [Range(1, 10)] float floorWidth = 1f;
    [SerializeField] [Range(1, 10)] float floorHeight = 1f;
    [SerializeField] [Range(1, 10)] float roofHeight = 1f;
    [SerializeField] [Range(3, 15)] private int polygoneMinSides;
    [SerializeField] [Range(3, 15)] private int polygoneMaxSides;
    public GameObject buildingPrefab;
    private Vector3 center;
    private int currentFloor;
    //Will contain the list of all the floors excluding the entrance and the roof
    PrimitiveBuilding[] floors;
    // Start is called before the first frame update
    void Start()
    {
        //floors = new PrimitiveBuilding[floorsNumber];
        center = Vector3.zero;
        currentFloor = 0;
        ApplyRules();
    }

    private void ApplyRules()
    {
        int currentFloorSides = 0;
        //Read the axiom and build the building according to the rules given
        foreach(char member in floorsAxiom)
        {
            switch(member)
            {
                case '-':
                    //Decrease the floor width
                    floorWidth -= step;
                    break;

                case '+':
                    //Increase the floor width
                    floorWidth += step;
                    break;

                case 'P':
                    //Build a floor
                    currentFloorSides = BuildFloor(null, false);
                    break;
                case 'C':
                    //Build a cubical floor
                    currentFloorSides = BuildFloor(4, false);
                    currentFloorSides = 4;
                    break;
                case 'S':
                    //Build Sphere building
                    currentFloorSides = BuildFloor(10, false);
                    currentFloorSides = 10;
                    break;
                case 'R':
                    //Build the roof
                    BuildFloor(currentFloorSides, true);
                    break;


            }
        }
    }


    private int BuildFloor(int? nbSides, bool isRoof)
    {
        //Use the PrimitiveBuilding script to build one of the floors
        Vector3 currentCenter = center + new Vector3(0, floorHeight * currentFloor, 0);
        PrimitiveBuilding pb = GameObject.Instantiate(buildingPrefab, center, Quaternion.identity).GetComponent<PrimitiveBuilding>();
        int sides = 0;
        if(!isRoof)
        {
            if (nbSides == null)
            {
                sides = UnityEngine.Random.Range(polygoneMinSides, polygoneMaxSides);
                pb.Initialize(sides,
                    floorWidth,
                    floorHeight,
                    currentCenter);
            }
            else
            {
                sides = (int)nbSides;
                pb.Initialize(nbSides, floorWidth, floorHeight, currentCenter);
            }
            currentFloor++;
            pb.BuildPrimitive();
        }
        else
        {
            if (nbSides == null)
            {
                Debug.LogError("BuilfFloor : error number of sides is needed for the roof");
                return -1;
            }
            else
            {
                pb.Initialize(nbSides, floorWidth, roofHeight, currentCenter);
            }
            currentFloor++;
            pb.BuildRoof();
        }
        return sides;

    }
}
