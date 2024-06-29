
using System.Collections.Generic;
using UnityEngine;



public class EnableOnPhone : MonoBehaviour
{
#if UNITY_ANDROID
    [SerializeField] GameObject AddButton;
    [SerializeField] List<GameObject> Menus = new List<GameObject>();
    private MenuManager _menuManager;
   
    private void Start()
    {
        AddButton.SetActive(true);
        _menuManager = GetComponent<MenuManager>();

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            foreach (GameObject menu in Menus)
            {
                if (menu.activeSelf)
                {
                    _menuManager.CloseMenu(menu);
                }
            }
        }
    }
    

#endif
}
