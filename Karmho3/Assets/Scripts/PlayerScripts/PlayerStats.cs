using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Sprite fullPotion;
    [SerializeField] private Sprite emptyPotion;
    public Image[] potionImages;

    [Header("Player Stats")]
    [SerializeField] private int potions = 3;
    private int maxPotions = 3;
    public int reliquias = 0;    
    private float health = 100f;
    private float mana = 100f;
    private GameMenager gameMenager;

    private void Awake()
    {
        gameMenager = GameMenager.Instance;
        if(gameMenager.managerReliquias != reliquias)
        {
            reliquias = gameMenager.managerReliquias;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        manaSlider.value = mana;
        UpdatePotionUI();
    }

    #region Life

    public void PlusLife(int curePower)
    {
        if(potions > 0)
        {
            health += curePower;
            potions--;            
        }
        
        if (health > 100f)
        {
            health = 100f;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            health = 0f;
            
        }
    }

    #endregion

    #region Potions
    public void UpdatePotionUI()
    {
        for (int i = 0; i < potionImages.Length; i++) //Cria um loop para atualizar a UI das poções
        {
            if(i < potions) //Seta as poções cheias dependendo de quantas poções o jogador tem
            {
                potionImages[i].sprite = fullPotion;
            }
            else //Seta as poções vazias dependendo de quantas poções o jogador tem
            {
                potionImages[i].sprite = emptyPotion;
            }

            if (i < maxPotions) //Ativa ou desativa a imagem se o jogador não tiver nada de poção ou tiver mais do que o máximo de poções
            {
                potionImages[i].enabled = true;
            }
            else
            {
                potionImages[i].enabled = false;
            }
        }
    }

    #endregion

}
