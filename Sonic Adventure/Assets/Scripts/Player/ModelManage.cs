using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManage : MonoBehaviour
{
    [SerializeField] private GameObject sonicModel;
    [SerializeField] private GameObject ballModel;
    [SerializeField] private GameObject spinModel;

    public void ChangeToNormalModel()
    {
        sonicModel.SetActive(true);
        ballModel.SetActive(false);
        spinModel.SetActive(false);
    }

    public void ChangeToBallModel()
    {
        sonicModel.SetActive(false);
        ballModel.SetActive(true);
        spinModel.SetActive(false);
    }

    public void ChangeToSpinModel()
    {
        sonicModel.SetActive(false);
        ballModel.SetActive(false);
        spinModel.SetActive(true);
    }
}
