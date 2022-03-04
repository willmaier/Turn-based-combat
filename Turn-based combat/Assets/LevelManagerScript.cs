using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    private bool isGameOver = false;
    private bool canPlayerAttack;

    public int playerHealth = 20;
    public int enemyHealth = 20;

    public Slider playerSlider;
    public Slider enemySlider;

    public Text playerHealthText;
    public Text enemyHealthText;

    [SerializeField] Button[] attacks;

    public AudioClip safeSFX;
    public AudioClip riskySFX;
    public AudioClip enemySFX;

    // Start is called before the first frame update
    void Start()
    {
        canPlayerAttack = true;
        playerSlider.value = playerHealth;
        enemySlider.value = enemyHealth;  
        

        foreach (Button btn in attacks)
        {
            Button attack = btn;
            btn.onClick.AddListener(() => playerAttack(attack));
        }
    }

    void Update()
    {
        if(playerHealth <= 0 || enemyHealth <= 0)
        {
            isGameOver = true;
        }    
    }

    void playerAttack(Button attack)
    {
        if (!isGameOver && canPlayerAttack)
        {
            if (attack.gameObject.tag == "SafeAttack")
            {
                // safe attack
                canPlayerAttack = false;
                AudioSource.PlayClipAtPoint(safeSFX, transform.position);
                enemyHealth--;
                enemySlider.value = enemyHealth;
                enemyHealthText.text = enemyHealth.ToString();
                Debug.Log("Enemy health: " + enemyHealth);
                if(enemyHealth > 0)
                {
                    Invoke("enemyAttack", 3.0f);
                }
            }
            else
            {
                // risky attack
                canPlayerAttack = false;
                AudioSource.PlayClipAtPoint(riskySFX, transform.position);
                enemyHealth -= 5;
                enemySlider.value = enemyHealth;
                enemyHealthText.text = enemyHealth.ToString();
                int rng = Random.Range(1, 5);
                if (rng == 1)
                {
                    playerHealth -= 5;
                    playerSlider.value = playerHealth;
                    playerHealthText.text = playerHealth.ToString();
                }
                Debug.Log("Enemy health: " + enemyHealth + " Player health: " + playerHealth + " " + rng);
                if (enemyHealth > 0)
                {
                    Invoke("enemyAttack", 3.0f);
                }
            }
        }
    }

    void enemyAttack()
    {
        AudioSource.PlayClipAtPoint(enemySFX, transform.position);
        playerHealth -= 2;
        playerSlider.value = playerHealth;
        playerHealthText.text = playerHealth.ToString();
        canPlayerAttack = true;
    }
}
