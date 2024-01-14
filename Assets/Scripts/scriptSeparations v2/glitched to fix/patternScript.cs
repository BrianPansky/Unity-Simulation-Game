using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternScript : MonoBehaviour
{
    //patterns in space, time, and relational structures



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello?");
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public List<Vector3> spatialRowSimpleOrigin(int theNumberOfRows, int theSpacing, int theAxis)
    {
        //creates a list of vectors, each spaced evenly, at origin of this object
        //along given axis
        List<List<float>> listOfManualVectors = new List<List<float>>();
        List<float> manualVector = new List<float>();
        float fx = 0;
        float fy = 0;
        float fz = 0;

        List<Vector3> thePositionList = new List<Vector3>();
        //Vector3 previousVector3 = new Vector3();

        while (theSpacing > 0)
        {
            //thePositionList
            manualVector.Add(fx);
            manualVector.Add(fy);
            manualVector.Add(fz);

            listOfManualVectors.Add(manualVector);

            if(theAxis == 1)
            {
                fx += theSpacing;
            }
            else if(theAxis == 2)
            {
                fy += theSpacing;
            }
            else if(theAxis == 3)
            {
                fz += theSpacing;
            }

            manualVector.Clear();

            theSpacing -= 1;
        }

        foreach(var thisManualVector in listOfManualVectors)
        {
            thePositionList.Add(new Vector3(thisManualVector[0], thisManualVector[1], thisManualVector[2]));
        }

        return thePositionList;
    }
}
