using UnityEngine;
using DG.Tweening;

public class ThermometerScript : MonoBehaviour
{
    public float thermometerSpeed;
    public float thermometerDownSpeed;

    public void IncreaseThermometer()
    {
        EditorDebugger.Log("INCREASING THERMO");
        transform.DOKill(false);
        transform.DOScaleY(1, thermometerSpeed);
    }

    public void DecreaseThermometer()
    {
        transform.DOKill(false);
        transform.DOScaleY(0, thermometerDownSpeed);
    }

    public void ResetThermometer()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
    }
}