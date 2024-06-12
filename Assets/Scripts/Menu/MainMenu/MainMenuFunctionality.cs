using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFunctionality : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> Menus = new List<GameObject>();

    public void DisableAllMenus()
    {
        foreach (GameObject menu in Menus)
        {
            menu.SetActive(false);
        }
    }
}
