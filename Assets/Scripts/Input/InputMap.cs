//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Input/InputMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputMap : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMap"",
    ""maps"": [
        {
            ""name"": ""Keys"",
            ""id"": ""0a1cb9a6-65dc-4d05-8c60-8c7af5ba2eda"",
            ""actions"": [
                {
                    ""name"": ""CameraAxis"",
                    ""type"": ""Value"",
                    ""id"": ""e67f84c4-5ac9-4807-b120-944b43dc3b98"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveAxis"",
                    ""type"": ""Value"",
                    ""id"": ""8b150c0d-863e-495a-a4e3-cb811cbfaabb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""67fee4e2-65f5-43c4-a76e-f5f979562246"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""735ef45b-b3ee-4b0f-abb7-6b66abbf077b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""bd64554b-528a-4bd8-bbb3-e04f82037f37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Turn"",
                    ""type"": ""Value"",
                    ""id"": ""0f966f47-4a76-4d61-b397-26829d08400c"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FireAuto"",
                    ""type"": ""Button"",
                    ""id"": ""3aa5d608-5d84-4bef-85dd-1bb961fcb3a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FireTap"",
                    ""type"": ""Button"",
                    ""id"": ""5d4a9dad-8b31-4975-a669-2e458815707c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""13bd2753-bd88-4f28-84b0-f527c4a4ff27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""a5c1b9a2-fc54-443e-b278-2f6f7627e82b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""NightVision"",
                    ""type"": ""Button"",
                    ""id"": ""3958c0d6-5c71-4aa4-8fd9-ea70d93df26d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Flashlight"",
                    ""type"": ""Button"",
                    ""id"": ""472f1a35-4de5-482a-8257-75611aaa3d73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Laser"",
                    ""type"": ""Button"",
                    ""id"": ""e224c223-efea-4d55-b2bb-1dad95351a64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""968abc51-a4fa-4cd2-8166-4c3fdea91b2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inspect"",
                    ""type"": ""Button"",
                    ""id"": ""4eb0d930-1f6c-4535-9cd0-4b756c712d7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Custom"",
                    ""type"": ""Button"",
                    ""id"": ""f0981c42-8ca3-4a5d-9e1b-2d339dd7af66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipPrimary"",
                    ""type"": ""Button"",
                    ""id"": ""bde2314d-5999-432c-88b0-34ddd87162aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipSecondary"",
                    ""type"": ""Button"",
                    ""id"": ""234b889d-e010-43cb-a7f4-76aea212868a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""939f788c-dd3f-46e6-8c8b-b6b1b16a4f76"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ef0fdd3-b758-4a2c-9e12-dfcf1e0ad5a5"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""967eec24-a5dc-4136-8540-a6f4b13b2971"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""PC"",
                    ""id"": ""7884947c-51a1-44c4-b3bb-bd576bd1b75b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""68ee0bc5-0bb9-4a84-afd7-9fefb0229d89"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2b1e3587-bdfb-4b69-8a66-fcf3e6f24f5e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""25eeaa4d-34ef-4dda-8dca-3e72c5ab6554"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ffa9d271-0986-4231-9725-e0cb5d1f7ac9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7db02d2d-d737-4151-bbed-29dc09df62f6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireAuto"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6be8792-be84-4372-a563-c079cd2938a3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireTap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85e09bc6-0e73-4285-88ad-92fb431692d6"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf744a02-7987-48b9-9f0d-baeef1464715"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c79b050-20b0-4e9b-ac27-dcfcd9f4baa6"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NightVision"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bd9b190-fb6e-4c2d-9f08-cb94525d556f"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6465f0ec-460e-4584-a555-53d0418f0175"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""406093db-521c-4c9d-af6b-d06660b5577d"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Laser"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""179ecae7-02a4-4b9d-8862-f11d6e0350d5"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipPrimary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfd9a753-afbe-40eb-8adb-1c9810b52a66"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipSecondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d21cf65d-ff94-4120-9650-0ba7c93f4569"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""PC"",
                    ""id"": ""708aba3c-b78d-44cd-8042-bd8bae7c70ed"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4e8dd702-5f06-437d-9f5a-4d2424dd58d2"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""38c78594-127a-4cb2-af5d-70d3f0cfc7dd"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5ae6b64c-8715-47e4-b1c2-bdad0b24d290"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Custom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25267f31-b448-4bff-ac73-7cab5917bffb"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inspect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keys
        m_Keys = asset.FindActionMap("Keys", throwIfNotFound: true);
        m_Keys_CameraAxis = m_Keys.FindAction("CameraAxis", throwIfNotFound: true);
        m_Keys_MoveAxis = m_Keys.FindAction("MoveAxis", throwIfNotFound: true);
        m_Keys_Jump = m_Keys.FindAction("Jump", throwIfNotFound: true);
        m_Keys_Run = m_Keys.FindAction("Run", throwIfNotFound: true);
        m_Keys_Crouch = m_Keys.FindAction("Crouch", throwIfNotFound: true);
        m_Keys_Turn = m_Keys.FindAction("Turn", throwIfNotFound: true);
        m_Keys_FireAuto = m_Keys.FindAction("FireAuto", throwIfNotFound: true);
        m_Keys_FireTap = m_Keys.FindAction("FireTap", throwIfNotFound: true);
        m_Keys_Aim = m_Keys.FindAction("Aim", throwIfNotFound: true);
        m_Keys_Reload = m_Keys.FindAction("Reload", throwIfNotFound: true);
        m_Keys_NightVision = m_Keys.FindAction("NightVision", throwIfNotFound: true);
        m_Keys_Flashlight = m_Keys.FindAction("Flashlight", throwIfNotFound: true);
        m_Keys_Laser = m_Keys.FindAction("Laser", throwIfNotFound: true);
        m_Keys_Interact = m_Keys.FindAction("Interact", throwIfNotFound: true);
        m_Keys_Inspect = m_Keys.FindAction("Inspect", throwIfNotFound: true);
        m_Keys_Custom = m_Keys.FindAction("Custom", throwIfNotFound: true);
        m_Keys_EquipPrimary = m_Keys.FindAction("EquipPrimary", throwIfNotFound: true);
        m_Keys_EquipSecondary = m_Keys.FindAction("EquipSecondary", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Keys
    private readonly InputActionMap m_Keys;
    private IKeysActions m_KeysActionsCallbackInterface;
    private readonly InputAction m_Keys_CameraAxis;
    private readonly InputAction m_Keys_MoveAxis;
    private readonly InputAction m_Keys_Jump;
    private readonly InputAction m_Keys_Run;
    private readonly InputAction m_Keys_Crouch;
    private readonly InputAction m_Keys_Turn;
    private readonly InputAction m_Keys_FireAuto;
    private readonly InputAction m_Keys_FireTap;
    private readonly InputAction m_Keys_Aim;
    private readonly InputAction m_Keys_Reload;
    private readonly InputAction m_Keys_NightVision;
    private readonly InputAction m_Keys_Flashlight;
    private readonly InputAction m_Keys_Laser;
    private readonly InputAction m_Keys_Interact;
    private readonly InputAction m_Keys_Inspect;
    private readonly InputAction m_Keys_Custom;
    private readonly InputAction m_Keys_EquipPrimary;
    private readonly InputAction m_Keys_EquipSecondary;
    public struct KeysActions
    {
        private @InputMap m_Wrapper;
        public KeysActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraAxis => m_Wrapper.m_Keys_CameraAxis;
        public InputAction @MoveAxis => m_Wrapper.m_Keys_MoveAxis;
        public InputAction @Jump => m_Wrapper.m_Keys_Jump;
        public InputAction @Run => m_Wrapper.m_Keys_Run;
        public InputAction @Crouch => m_Wrapper.m_Keys_Crouch;
        public InputAction @Turn => m_Wrapper.m_Keys_Turn;
        public InputAction @FireAuto => m_Wrapper.m_Keys_FireAuto;
        public InputAction @FireTap => m_Wrapper.m_Keys_FireTap;
        public InputAction @Aim => m_Wrapper.m_Keys_Aim;
        public InputAction @Reload => m_Wrapper.m_Keys_Reload;
        public InputAction @NightVision => m_Wrapper.m_Keys_NightVision;
        public InputAction @Flashlight => m_Wrapper.m_Keys_Flashlight;
        public InputAction @Laser => m_Wrapper.m_Keys_Laser;
        public InputAction @Interact => m_Wrapper.m_Keys_Interact;
        public InputAction @Inspect => m_Wrapper.m_Keys_Inspect;
        public InputAction @Custom => m_Wrapper.m_Keys_Custom;
        public InputAction @EquipPrimary => m_Wrapper.m_Keys_EquipPrimary;
        public InputAction @EquipSecondary => m_Wrapper.m_Keys_EquipSecondary;
        public InputActionMap Get() { return m_Wrapper.m_Keys; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeysActions set) { return set.Get(); }
        public void SetCallbacks(IKeysActions instance)
        {
            if (m_Wrapper.m_KeysActionsCallbackInterface != null)
            {
                @CameraAxis.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnCameraAxis;
                @CameraAxis.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnCameraAxis;
                @CameraAxis.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnCameraAxis;
                @MoveAxis.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnMoveAxis;
                @MoveAxis.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnMoveAxis;
                @MoveAxis.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnMoveAxis;
                @Jump.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnJump;
                @Run.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnRun;
                @Crouch.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnCrouch;
                @Turn.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnTurn;
                @Turn.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnTurn;
                @Turn.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnTurn;
                @FireAuto.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnFireAuto;
                @FireAuto.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnFireAuto;
                @FireAuto.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnFireAuto;
                @FireTap.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnFireTap;
                @FireTap.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnFireTap;
                @FireTap.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnFireTap;
                @Aim.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnAim;
                @Reload.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnReload;
                @NightVision.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnNightVision;
                @NightVision.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnNightVision;
                @NightVision.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnNightVision;
                @Flashlight.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnFlashlight;
                @Flashlight.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnFlashlight;
                @Flashlight.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnFlashlight;
                @Laser.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnLaser;
                @Laser.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnLaser;
                @Laser.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnLaser;
                @Interact.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnInteract;
                @Inspect.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnInspect;
                @Inspect.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnInspect;
                @Inspect.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnInspect;
                @Custom.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnCustom;
                @Custom.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnCustom;
                @Custom.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnCustom;
                @EquipPrimary.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnEquipPrimary;
                @EquipPrimary.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnEquipPrimary;
                @EquipPrimary.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnEquipPrimary;
                @EquipSecondary.started -= m_Wrapper.m_KeysActionsCallbackInterface.OnEquipSecondary;
                @EquipSecondary.performed -= m_Wrapper.m_KeysActionsCallbackInterface.OnEquipSecondary;
                @EquipSecondary.canceled -= m_Wrapper.m_KeysActionsCallbackInterface.OnEquipSecondary;
            }
            m_Wrapper.m_KeysActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraAxis.started += instance.OnCameraAxis;
                @CameraAxis.performed += instance.OnCameraAxis;
                @CameraAxis.canceled += instance.OnCameraAxis;
                @MoveAxis.started += instance.OnMoveAxis;
                @MoveAxis.performed += instance.OnMoveAxis;
                @MoveAxis.canceled += instance.OnMoveAxis;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Turn.started += instance.OnTurn;
                @Turn.performed += instance.OnTurn;
                @Turn.canceled += instance.OnTurn;
                @FireAuto.started += instance.OnFireAuto;
                @FireAuto.performed += instance.OnFireAuto;
                @FireAuto.canceled += instance.OnFireAuto;
                @FireTap.started += instance.OnFireTap;
                @FireTap.performed += instance.OnFireTap;
                @FireTap.canceled += instance.OnFireTap;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @NightVision.started += instance.OnNightVision;
                @NightVision.performed += instance.OnNightVision;
                @NightVision.canceled += instance.OnNightVision;
                @Flashlight.started += instance.OnFlashlight;
                @Flashlight.performed += instance.OnFlashlight;
                @Flashlight.canceled += instance.OnFlashlight;
                @Laser.started += instance.OnLaser;
                @Laser.performed += instance.OnLaser;
                @Laser.canceled += instance.OnLaser;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Inspect.started += instance.OnInspect;
                @Inspect.performed += instance.OnInspect;
                @Inspect.canceled += instance.OnInspect;
                @Custom.started += instance.OnCustom;
                @Custom.performed += instance.OnCustom;
                @Custom.canceled += instance.OnCustom;
                @EquipPrimary.started += instance.OnEquipPrimary;
                @EquipPrimary.performed += instance.OnEquipPrimary;
                @EquipPrimary.canceled += instance.OnEquipPrimary;
                @EquipSecondary.started += instance.OnEquipSecondary;
                @EquipSecondary.performed += instance.OnEquipSecondary;
                @EquipSecondary.canceled += instance.OnEquipSecondary;
            }
        }
    }
    public KeysActions @Keys => new KeysActions(this);
    public interface IKeysActions
    {
        void OnCameraAxis(InputAction.CallbackContext context);
        void OnMoveAxis(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnTurn(InputAction.CallbackContext context);
        void OnFireAuto(InputAction.CallbackContext context);
        void OnFireTap(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnNightVision(InputAction.CallbackContext context);
        void OnFlashlight(InputAction.CallbackContext context);
        void OnLaser(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnInspect(InputAction.CallbackContext context);
        void OnCustom(InputAction.CallbackContext context);
        void OnEquipPrimary(InputAction.CallbackContext context);
        void OnEquipSecondary(InputAction.CallbackContext context);
    }
}
