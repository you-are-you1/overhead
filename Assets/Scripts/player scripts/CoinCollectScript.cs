using UnityEngine;


public class CoinCollectScript : MonoBehaviour
{
    public static int coinsInCurrentLevel;
    public static int totalCoins = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinsInCurrentLevel = 0;
    }

    // Update is called once per frame
    private void collectCoin(CoinScript c)
    {
        coinsInCurrentLevel++;
    }

    private void saveCoinsToTotal(Ascend a)
    {
        totalCoins += coinsInCurrentLevel;
        
    }

    private void OnEnable()
    {
        CoinScript.OnCollectCoinEvent += collectCoin;
        Ascend.NextLevelEvent += saveCoinsToTotal;
    }

    private void OnDisable()
    {
        CoinScript.OnCollectCoinEvent -= collectCoin;
        Ascend.NextLevelEvent -= saveCoinsToTotal;
    }
}
