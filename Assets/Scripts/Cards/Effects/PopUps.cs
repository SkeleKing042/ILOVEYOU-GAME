using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUps : MonoBehaviour
{
    [SerializeField] GameObject[] m_popUpPrefabs;
    private List<GameObject> m_currentPopups;

    //TODO: finish this lol
    //when active constantly make sure that the context button is assigned to this
    //

    private void Awake()
    {
        //assign player controller here (or have an initialization function called by player manager)
    }

    private void Update()
    {
        //perhaps this can assign the context function
        //also will check if all popups are destroyed and calls EndPopups()
    }

    public void StartPopUps(int count)
    {
        //creates popups with count
    }

    public void EndPopups()
    {
        //clears any outstanding popups and disables self
        //also removes context function
    }

    public void WindowClosed()
    {
        //this will be the context funciton called to remove windows
    }
}
