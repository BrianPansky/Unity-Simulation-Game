using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
}





public class nestedLayerDebug
{
    //put one in each layer to manage which level you want to print from, etc
    //it does not need to be input into initialization functions
    //just do this is higher level (after making lower level, or wherever needed):
    //      higherLevelDebugThing.
    //only issue is it then can't debug inside of their initialization functions.......

    int layerNumber = 0;
    int layerToPrintCutoff = 0;
    bool debugPrintBool = false;

    nestedLayerDebug(int desiredLayerDepthToPrint)
    {
        layerToPrintCutoff = desiredLayerDepthToPrint;
    }

    nestedLayerDebug(nestedLayerDebug upperLevelDebugInfo)
    {
        layerNumber = upperLevelDebugInfo.layerNumber + 1;
    }




    void debugDownload(nestedLayerDebug lowerLayerDebugInfo)
    {
        //place this BEFORE wherever debug printouts are needed

        if(lowerLayerDebugInfo == null) { lowerLayerDebugInfo = new nestedLayerDebug(0); }

        lowerLayerDebugInfo.layerNumber = layerNumber + 1;
        lowerLayerDebugInfo.debugPrintBool = debugPrintBool;

    }

    void debugStartPoint(nestedLayerDebug upperLevelDebugInfo)
    {
        //place this BEFORE wherever debug printouts are needed

        layerNumber = upperLevelDebugInfo.layerNumber + 1;
        debugPrintBool = upperLevelDebugInfo.debugPrintBool;

    }

    void conditionalPrint(string toPrint)
    {
        if (debugPrintBool == false) { return; }
        if (layerNumber > layerToPrintCutoff) { return; }

        Debug.Log(toPrint);
    }
}
