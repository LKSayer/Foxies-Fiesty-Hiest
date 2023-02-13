using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int fruit = 0;
    [SerializeField] private TextMeshProUGUI ShinyText;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
           Destroy(collision.gameObject);
           fruit++;
           ShinyText.text = "Shinies: " + fruit;
        }

        if (fruit is 4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
}
