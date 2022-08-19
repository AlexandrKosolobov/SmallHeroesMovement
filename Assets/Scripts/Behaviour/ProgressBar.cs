using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image _progresBar;
    [SerializeField] Gradient _gradient;

    public void UpdateProgressValue(float _progresPerc)
    {
        _progresBar.fillAmount = _progresPerc;
        _progresBar.color = _gradient.Evaluate(_progresPerc);
    }
}
