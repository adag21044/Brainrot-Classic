using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AIManager aiManager;
    public HandManager playerHand;
    public HandManager ai1Hand;
    public HandManager ai2Hand;
    public HandManager ai3Hand;

    private TurnOwner currentTurn;

    void Start()
    {
        ChooseRandomStarter();
        StartTurn();
    }

    void ChooseRandomStarter()
    {
        int roll = Random.Range(0, 4);
        currentTurn = (TurnOwner)roll;
        Debug.Log($"Game starts with: {currentTurn}");
    }

    void StartTurn()
    {
        switch (currentTurn)
        {
            case TurnOwner.Player:
                Debug.Log("Player's Turn");
                // Player'a input aç
                break;
            case TurnOwner.AI1:
                aiManager.DrawCardForAI(ai1Hand);
                break;
            case TurnOwner.AI2:
                aiManager.DrawCardForAI(ai2Hand);
                break;
            case TurnOwner.AI3:
                aiManager.DrawCardForAI(ai3Hand);
                break;
        }

        // Her turdan sonra sıradakine geç
        Invoke(nameof(NextTurn), 2f);
    }

    void NextTurn()
    {
        currentTurn = (TurnOwner)(((int)currentTurn + 1) % 4);
        StartTurn();
    }
}
