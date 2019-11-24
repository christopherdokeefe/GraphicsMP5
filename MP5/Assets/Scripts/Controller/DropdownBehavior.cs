using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehavior : MonoBehaviour
{
    public Dropdown Menu;
    public GameObject QuadNxM;
    public GameObject Cylinder;
    public GameObject Controller;

    // Start is called before the first frame update
    void Start()
    {
        Menu.onValueChanged.AddListener(delegate { menuValueChanged(); });
    }

    public void menuValueChanged()
    {
        Controller.GetComponent<MainController>().DeselectVertex();
        if (Menu.value == 0)
        {
            QuadNxM.SetActive(true);
            Cylinder.SetActive(false);
        }
        else if (Menu.value == 1)
        {
            QuadNxM.SetActive(false);
            Cylinder.SetActive(true);
        }
    }
}
