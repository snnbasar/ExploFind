using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Image mask;

    public void FillAmount(int max, int cur)
    {
        float fillAmount = (float)cur / (float)max;
        mask.fillAmount = fillAmount;
    }
}
