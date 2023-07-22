using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance  { get; private set; }
    public Controls Controls { get; private set; }
    public bool ShootingInputEnabled { get; set; } = true;
    public UnityAction OnEnableEvent, OnDisableEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }    
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);        
            Controls = new Controls();
        }        
    }

    private void OnEnable() 
    {
        Controls.Enable();
    }

    private void OnDisable() 
    {
        if (Controls != null) //handle case where this is called when destroying a duplicate, so Controls are never instantiated
        {
            Controls.Disable();
        }
    }

    public void SetShootingEnabled(bool enabled)
    {
        if (enabled)
        {
            EnableShooting();
        }
        else
        {
            DisableShooting();
        }
    }

    private void EnableShooting()
    {
        OnEnableEvent?.Invoke();
        ShootingInputEnabled = true;
        Controls.Player.Cancel.Enable();    
        Controls.Player.Fire.Enable();  
    }

    private void DisableShooting()
    {
        OnDisableEvent?.Invoke();
        ShootingInputEnabled = false;
        Controls.Player.Cancel.Disable();    
        Controls.Player.Fire.Disable();    
    }
}
