using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class safeDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        if (Application.isPlaying == false) { return; }

        //Debug.Log("//////////////   safe destroy:  " + this.gameObject.GetInstanceID());

        tagging2.singleton.removeAllTagsAndZone(this.gameObject);

        if (this.gameObject.GetComponent<IupdateCallable>() == null) { return; }
        worldScript.singleton.removeIupdateCallableFromItsList(this.gameObject.GetComponent<IupdateCallable>());
    }
}
