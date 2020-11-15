using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comp : IComparer<GameObject> {
    public int Compare(GameObject x, GameObject y) {
        return (int)(x.transform.position.x - y.transform.position.x); 
    }
}

public class StayDeadAcrossTimeScript : MonoBehaviour
{
    private string uniqueStageNamePlusAlienIDNum;
    Comp comparer = new Comp();

    public static string alienNameForID(int IDnum) {
        // scene name involved so player pref is unique across scenes regardless of time period
        return "Living_TimeAlien_" + SceneManager.GetActiveScene().name + IDnum;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] aliensHere = GameObject.FindGameObjectsWithTag("XarlnerAlien");
        Array.Sort(aliensHere, comparer);
        // using order along X axis in scene as arbitrary but deterministic way to number aliens
        int orderedID = Array.IndexOf(aliensHere, gameObject);

        uniqueStageNamePlusAlienIDNum = alienNameForID(orderedID);

       if(AlreadyRemoved()) {
            Debug.Log("ALREADY ZAPPED, REMOVING: "+ uniqueStageNamePlusAlienIDNum);
            Destroy(gameObject);
       }
    }

    public void WreckForAllTime() {
        if (AlreadyRemoved()==false) {
            Debug.Log("NOW ZAPPING, REMOVING: " + uniqueStageNamePlusAlienIDNum);
            PlayerPrefs.SetInt(uniqueStageNamePlusAlienIDNum, 0);
        }
    }

    bool AlreadyRemoved() {
        return PlayerPrefs.GetInt(uniqueStageNamePlusAlienIDNum, 1) == 0;
    }
}
