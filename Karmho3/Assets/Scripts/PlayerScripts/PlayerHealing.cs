using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealing : MonoBehaviour
{
    [SerializeField] private int theCure = 20;
   
    public void Heal(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GetComponent<PlayerStats>().PlusLife(theCure);
        }
    }
}
