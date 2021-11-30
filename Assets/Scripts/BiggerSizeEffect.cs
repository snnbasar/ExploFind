using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BiggerSizeEffect : MonoBehaviour
{
    
    public async void Effect(float objectSizeMultiplayer, float speed)
    {
        Instantiate(GameManager.instance._particle, transform);
        Vector3 newSize = transform.localScale * objectSizeMultiplayer;
        float x = 0;
        while (x < newSize.x)
        {
            x = transform.localScale.x;
            transform.localScale += newSize / speed;
            await Task.Yield();
        }
    }
}
