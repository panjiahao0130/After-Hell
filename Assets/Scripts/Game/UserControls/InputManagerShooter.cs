using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManagerShooter : MonoBehaviour
{
    private PlayerInput input;
    private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)> ();
    
    private void Awake()
    {
        input = GetComponent<PlayerInput> ();
                    
        InitializeActions();
    }
        
    private void InitializeActions()
    {
        actions.Add((input.actions["Fire"], OnFire));
        actions.Add((input.actions["Move"], OnMove));
        actions.Add((input.actions["Dash"], OnDash));
        actions.Add((input.actions["SwitchWeaponUp"], OnSwitchWeaponUp));
        actions.Add((input.actions["SwitchWeaponDown"], OnSwitchWeaponDown));
    }
        
    private void OnEnable()
    {
        foreach (var inputAction in actions)
            inputAction.action.performed += inputAction.command;
    }
        
    private void OnDisable()
    {
        foreach (var inputAction in actions)
            inputAction.action.performed -= inputAction.command;
    }
        //在下面方法中加入各种输入的逻辑
    public void OnFire(InputAction.CallbackContext c)
    {
                    
    }
        
    public void OnDash(InputAction.CallbackContext c)
    {
                    
    }
                
    public void OnSwitchWeaponUp(InputAction.CallbackContext c)
    {
                    
    }
    public void OnSwitchWeaponDown(InputAction.CallbackContext c)
    {
                    
    }
    public void OnMove(InputAction.CallbackContext c)
    {
                    
    }
}        

