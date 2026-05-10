using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    private int reliquias = 0;
    private float health = 50f;
    private float mana = 100f;

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        manaSlider.value = mana;
    }

    public void PlusLife(int curePower)
    {
        health += curePower;
        if (health > 100f)
        {
            health = 100f;
        }
    }

}
