using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("Game", 1, 1);
    }
    private void Game()
    {
        ObjectPool.Instance.SpawnFromPool("pickup", new Vector3(Random.Range(-9, 9), 0.5f, Random.Range(-9, 9)), new Quaternion());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
