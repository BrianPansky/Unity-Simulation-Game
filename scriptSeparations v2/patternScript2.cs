using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }






    public List<Vector3> spatialRowSimpleOrigin(int theNumberOfPlacesInRow, int theSpacing, int theAxis)
    {
        //creates a list of vectors, each spaced evenly, at origin of this object
        //along given axis

        float fx = 0;
        float fy = 0;
        float fz = 0;

        List<Vector3> thePositionList = new List<Vector3>();
        //Vector3 previousVector3 = new Vector3();

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

        //foreach (var thisPosition in thePositionList)
        {
            //Debug.Log(thisPosition);
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



}
