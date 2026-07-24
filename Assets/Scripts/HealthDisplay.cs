using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public Image heart1;
    public Image heart2;
    public Image heart3;

    public void UpdateHearts(int health)
    {
        heart1.enabled = health >= 1;
        heart2.enabled = health >= 2;
        heart3.enabled = health >= 3;
    }
}