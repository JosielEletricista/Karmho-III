using System.Collections;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    bool waiting = false;

    public void Stop(float duration)
    {
        if (waiting)
        {
            return;
        }
        Time.timeScale = 0f; //para o tempo
        StartCoroutine(Wait(duration)); //chama a coroutine para esperar o tempo definido
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f; //retorna o tempo ao normal
        waiting = false;
    }

}
