using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // The target the camera will follow

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

}
