using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifUI : MonoBehaviour
{
    public static NotifUI instance;
    public enum NotifType { EnergyLow, }

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Notify(NotifType type)
    {
        switch (type)
        {
            case NotifType.EnergyLow:
                break;
            default:
                break;
        }
    }
}
