using UnityEngine;

public class DoorID : MonoBehaviour
{
    public int doorID;
    private int trueDoorID = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        trueDoorID = doorID;
    }

    
}
