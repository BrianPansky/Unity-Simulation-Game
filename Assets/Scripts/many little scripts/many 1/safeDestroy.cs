using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        if (Application.isPlaying == false) { return; }

        tagging2.singleton.removeAllTagsAndZone(this.gameObject);

        if (this.gameObject.GetComponent<IupdateCallable>() == null) { return; }
        worldScript.singleton.removeIupdateCallableFromItsList(this.gameObject.GetComponent<IupdateCallable>());
    }
}
