using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternScript2 : MonoBehaviour
{
    public static patternScript2 singleton;

    void Awake()
    {
        singletonify();
    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }



    public List<Vector3> makeLinePattern1(int howMany, int theSpacing)
    {
        List<Vector3> thisList = new List<Vector3>();

        int whichPositionWeAreOn = 0;

        while (howMany > 0)
        {
            thisList.Add(new Vector3(0, 0, whichPositionWeAreOn * theSpacing));
            whichPositionWeAreOn++;
            howMany--;
        }

        return thisList;
    }
    public List<Vector3> makeLinePattern2(int howMany, int theSpacing, float sideOffset)
    {
        List<Vector3> thisList = new List<Vector3>();

        int whichPositionWeAreOn = 0;

        while (howMany > 0)
        {
            thisList.Add(new Vector3(sideOffset, 0, whichPositionWeAreOn * theSpacing));
            whichPositionWeAreOn++;
            howMany--;
        }

        return thisList;
    }

    public List<Vector3> spatialRowSimpleOrigin(int theNumberOfPlacesInRow, int theSpacing, int theAxis)
    {
        //creates a list of vectors, each spaced evenly, at origin of this object
        //along given axis

        float fx = 0;
        float fy = 0;
        float fz = 0;

        List<Vector3> thePositionList = new List<Vector3>();

        while (theNumberOfPlacesInRow > 0)
        {
            //thePositionList
            thePositionList.Add(new Vector3(fx,fy,fz));
            
            if (theAxis == 1)
            {
                fx += theSpacing;
            }
            else if (theAxis == 2)
            {
                fy += theSpacing;
            }
            else if (theAxis == 3)
            {
                fz += theSpacing;
            }

            
            theNumberOfPlacesInRow -= 1;
        }

        return thePositionList;
    }

    public List<Vector3> moveRowToPosition(List<Vector3> theRow, Vector3 thePosition)
    {
        List < Vector3 > newRow = new List<Vector3>();

        foreach (var thisPosition in theRow)
        {
            newRow.Add(thisPosition + thePosition);
        }

        return newRow;
    }





    public Vector3 randomNearbyVector(Vector3 positionToBeNear, float spreadFactor =1f)
    {
        Vector3 vectorToReturn = positionToBeNear;
        float initialDistance = 0f;
        float randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(initialDistance + randomAdditionalDistance* spreadFactor, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(0, 0, initialDistance + randomAdditionalDistance* spreadFactor);

        return vectorToReturn;
    }

}
