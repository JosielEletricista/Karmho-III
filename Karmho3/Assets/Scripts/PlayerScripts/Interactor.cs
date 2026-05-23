using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField]private bool onDoor;
    private DoorID currentDoor;


    public void DoorInteract(InputAction.CallbackContext context)
    {
        if (context.performed && onDoor && currentDoor != null)
        {
            GameMenager.Instance.CarregarCena("Fase" + currentDoor.doorID);
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Door"))
        {
            onDoor = true;
            currentDoor = collision.GetComponent<DoorID>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            onDoor = false;
            currentDoor = null;
        }
    }

}
