using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool IsMenuOpen {  get; private set; }
    public bool IsGameOn { get; set; }
    private void OnEnable()
    {
        IsMenuOpen = false;
    }
    public void OpenMenu(GameObject menu)
    {
        if (!IsMenuOpen && !IsGameOn) 
        {
            IsMenuOpen =true;
            menu.SetActive(true);
        }
    }

    public void CloseMenu(GameObject menu) 
    {
        if (IsMenuOpen)
        {
            IsMenuOpen=false;
            menu.SetActive(false);
        }
    }



}
