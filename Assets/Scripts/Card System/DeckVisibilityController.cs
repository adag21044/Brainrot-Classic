using UnityEngine;
using UnityEngine.UI;

public class DeckVisibilityController : MonoBehaviour
{
    public GameObject deckView; // Image bileşeni barındıran UI GameObject

    public void HideDeckView()
    {
        Debug.Log("Hiding deck sprite...");
        if (deckView != null)
        {
            Image img = deckView.GetComponent<Image>();
            if (img != null)
            {
                Debug.Log("Hiding deck sprite only");
                img.enabled = false;
            }
            else
            {
                Debug.LogWarning("Image component not found on deckView!");
            }
        }
        else
        {
            Debug.LogWarning("deckView is null!");
        }
    }
}
