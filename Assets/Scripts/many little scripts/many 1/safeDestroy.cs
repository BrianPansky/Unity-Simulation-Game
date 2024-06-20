using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            //this.gameObject.SetActive(false);
            //Debug.Log("................................preparing to destroy this object:  " + this.gameObject.GetInstanceID() + this.gameObject);

            tagging2.singleton.removeALLtags(this.gameObject);
            //removeIupdateCallableFromItsList
            if (this.gameObject.GetComponent<IupdateCallable>() != null)
            {
                worldScript.singleton.removeIupdateCallableFromItsList(this.gameObject.GetComponent<IupdateCallable>());
            }


            //Debug.Log("destroy this object:  " + this.gameObject.GetInstanceID() + this.gameObject);
            //UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
