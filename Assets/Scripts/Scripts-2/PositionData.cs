using UnityEngine;
using System.Collections;

public class PositionData : MonoBehaviour
{
    public int trackingFreq = 1;
    private float timer = 0f;

    public Vector3[] posArray;

    private int arrayIt = -1;
    private bool resettingArray = false;

    void Update()
    {
        timer += Time.deltaTime;

        if (!resettingArray && timer >= trackingFreq)
        {
            TrackPosition();
        }
    }

    void TrackPosition()
    {
        timer = 0;

        ArrayList auxArray = new ArrayList();

        if (posArray != null) auxArray.AddRange(posArray);

        auxArray.Add(transform.position);

        posArray = auxArray.ToArray(typeof(Vector3)) as Vector3[];
        arrayIt++;
        Debug.Log(posArray[arrayIt] + "  Iteration =  " + arrayIt);
    }

    public void ResetPosArray()
    {
        resettingArray = true;
        posArray = null;
        arrayIt = -1;
        resettingArray = false; 
    }
}

