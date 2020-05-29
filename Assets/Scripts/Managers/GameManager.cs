using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int headStation;
    public int cervix;
    public int angleIndex;

    public ModelLocator modelLocator;
    public ModelSelector modelSelector;

    // Start is called before the first frame update
    void Start()
    {
        modelSelector = GetComponent<ModelSelector>();
        modelLocator = GetComponent<ModelLocator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetModelInfo(int _headStation, int _cervix, int _angle)
    {
        headStation = _headStation;
        cervix = _cervix;
        angleIndex = _angle;

        modelSelector.SelectOne(cervix);
        modelLocator.SelectTransform(headStation);
        modelLocator.SelectAngle(angleIndex);
    }

    public void SelectHeadStation(int index)
    {
        headStation = index;
    }
    public void SelectCervix(int index)
    {
        cervix = index;
    }
    public void SelectAngle(int index)
    {
        angleIndex = index;
    }

}
