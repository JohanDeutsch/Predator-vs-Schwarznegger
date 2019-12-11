using System.Collections;
using UnityEngine;

public class SpawnSchwarz : MonoBehaviour
{
    public GameObject schwarz;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        while (Controller.isOnGame)
        {
            Instantiate(schwarz, new Vector2(Random.Range(-2.76f, 2.76f), 5.75f),Quaternion.identity );
            Controller.scoreIncrement();
            yield return new WaitForSeconds(GameVariables.curent_spawnloop_speed);
        }
    }
}
