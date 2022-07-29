//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Systems/Input/InputMap.inputactions
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
            ""name"": ""Player"",
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
                    ""name"": ""Cover"",
                    ""type"": ""Value"",
                    ""id"": ""0f966f47-4a76-4d61-b397-26829d08400c"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
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
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""225bbfd4-b488-4af1-8a69-04a63452c000"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSafety"",
                    ""type"": ""Button"",
                    ""id"": ""66cbd9cd-1e92-4aae-9ca9-6b11e797954a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponFireAuto"",
                    ""type"": ""Button"",
                    ""id"": ""3aa5d608-5d84-4bef-85dd-1bb961fcb3a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponFireTap"",
                    ""type"": ""Button"",
                    ""id"": ""5d4a9dad-8b31-4975-a669-2e458815707c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponAim"",
                    ""type"": ""Button"",
                    ""id"": ""13bd2753-bd88-4f28-84b0-f527c4a4ff27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponReload"",
                    ""type"": ""Button"",
                    ""id"": ""a5c1b9a2-fc54-443e-b278-2f6f7627e82b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponHandguard"",
                    ""type"": ""Button"",
                    ""id"": ""472f1a35-4de5-482a-8257-75611aaa3d73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.005)"",
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
                    ""action"": ""WeaponFireAuto"",
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
                    ""action"": ""WeaponFireTap"",
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
                    ""action"": ""WeaponReload"",
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
                    ""action"": ""WeaponHandguard"",
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
                    ""action"": ""WeaponAim"",
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
                    ""name"": ""PC"",
                    ""id"": ""708aba3c-b78d-44cd-8042-bd8bae7c70ed"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cover"",
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
                    ""action"": ""Cover"",
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
                    ""action"": ""Cover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9b802726-8e5d-4718-9b6d-0a01fc3c47b0"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23846b3d-7482-4928-9a3a-f8d3ad9a71c7"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSafety"",
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
                }
            ]
        },
        {
            ""name"": ""Customize"",
            ""id"": ""3e48de3c-e3c9-45b3-8807-a857b620a534"",
            ""actions"": [
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Button"",
                    ""id"": ""a0acdb13-d9c1-48b9-af54-b79a3ff3ebb6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ResetRotation"",
                    ""type"": ""Button"",
                    ""id"": ""43956639-aa36-40cd-b2fb-0d14c8c5472a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""caa39e92-bdd3-460f-a68e-877d8e66d480"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""411ce746-58de-4fbd-824d-c8db34fc6e79"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a97bf592-2b0f-4278-932d-68ae5c4ce4be"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3a9946e-493f-4212-83a4-b8987302ef0c"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_CameraAxis = m_Player.FindAction("CameraAxis", throwIfNotFound: true);
        m_Player_MoveAxis = m_Player.FindAction("MoveAxis", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_Cover = m_Player.FindAction("Cover", throwIfNotFound: true);
        m_Player_NightVision = m_Player.FindAction("NightVision", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_WeaponSafety = m_Player.FindAction("WeaponSafety", throwIfNotFound: true);
        m_Player_WeaponFireAuto = m_Player.FindAction("WeaponFireAuto", throwIfNotFound: true);
        m_Player_WeaponFireTap = m_Player.FindAction("WeaponFireTap", throwIfNotFound: true);
        m_Player_WeaponAim = m_Player.FindAction("WeaponAim", throwIfNotFound: true);
        m_Player_WeaponReload = m_Player.FindAction("WeaponReload", throwIfNotFound: true);
        m_Player_WeaponHandguard = m_Player.FindAction("WeaponHandguard", throwIfNotFound: true);
        m_Player_EquipPrimary = m_Player.FindAction("EquipPrimary", throwIfNotFound: true);
        m_Player_EquipSecondary = m_Player.FindAction("EquipSecondary", throwIfNotFound: true);
        // Customize
        m_Customize = asset.FindActionMap("Customize", throwIfNotFound: true);
        m_Customize_Rotation = m_Customize.FindAction("Rotation", throwIfNotFound: true);
        m_Customize_ResetRotation = m_Customize.FindAction("ResetRotation", throwIfNotFound: true);
        m_Customize_Scroll = m_Customize.FindAction("Scroll", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_CameraAxis;
    private readonly InputAction m_Player_MoveAxis;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_Cover;
    private readonly InputAction m_Player_NightVision;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_WeaponSafety;
    private readonly InputAction m_Player_WeaponFireAuto;
    private readonly InputAction m_Player_WeaponFireTap;
    private readonly InputAction m_Player_WeaponAim;
    private readonly InputAction m_Player_WeaponReload;
    private readonly InputAction m_Player_WeaponHandguard;
    private readonly InputAction m_Player_EquipPrimary;
    private readonly InputAction m_Player_EquipSecondary;
    public struct PlayerActions
    {
        private @InputMap m_Wrapper;
        public PlayerActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraAxis => m_Wrapper.m_Player_CameraAxis;
        public InputAction @MoveAxis => m_Wrapper.m_Player_MoveAxis;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @Cover => m_Wrapper.m_Player_Cover;
        public InputAction @NightVision => m_Wrapper.m_Player_NightVision;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @WeaponSafety => m_Wrapper.m_Player_WeaponSafety;
        public InputAction @WeaponFireAuto => m_Wrapper.m_Player_WeaponFireAuto;
        public InputAction @WeaponFireTap => m_Wrapper.m_Player_WeaponFireTap;
        public InputAction @WeaponAim => m_Wrapper.m_Player_WeaponAim;
        public InputAction @WeaponReload => m_Wrapper.m_Player_WeaponReload;
        public InputAction @WeaponHandguard => m_Wrapper.m_Player_WeaponHandguard;
        public InputAction @EquipPrimary => m_Wrapper.m_Player_EquipPrimary;
        public InputAction @EquipSecondary => m_Wrapper.m_Player_EquipSecondary;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @CameraAxis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraAxis;
                @CameraAxis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraAxis;
                @CameraAxis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraAxis;
                @MoveAxis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveAxis;
                @MoveAxis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveAxis;
                @MoveAxis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveAxis;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Run.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Crouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Cover.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCover;
                @Cover.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCover;
                @Cover.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCover;
                @NightVision.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNightVision;
                @NightVision.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNightVision;
                @NightVision.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNightVision;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @WeaponSafety.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSafety;
                @WeaponSafety.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSafety;
                @WeaponSafety.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSafety;
                @WeaponFireAuto.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponFireAuto;
                @WeaponFireAuto.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponFireAuto;
                @WeaponFireAuto.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponFireAuto;
                @WeaponFireTap.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponFireTap;
                @WeaponFireTap.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponFireTap;
                @WeaponFireTap.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponFireTap;
                @WeaponAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponAim;
                @WeaponAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponAim;
                @WeaponAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponAim;
                @WeaponReload.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponReload;
                @WeaponReload.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponReload;
                @WeaponReload.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponReload;
                @WeaponHandguard.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponHandguard;
                @WeaponHandguard.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponHandguard;
                @WeaponHandguard.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponHandguard;
                @EquipPrimary.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipPrimary;
                @EquipPrimary.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipPrimary;
                @EquipPrimary.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipPrimary;
                @EquipSecondary.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipSecondary;
                @EquipSecondary.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipSecondary;
                @EquipSecondary.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipSecondary;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
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
                @Cover.started += instance.OnCover;
                @Cover.performed += instance.OnCover;
                @Cover.canceled += instance.OnCover;
                @NightVision.started += instance.OnNightVision;
                @NightVision.performed += instance.OnNightVision;
                @NightVision.canceled += instance.OnNightVision;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @WeaponSafety.started += instance.OnWeaponSafety;
                @WeaponSafety.performed += instance.OnWeaponSafety;
                @WeaponSafety.canceled += instance.OnWeaponSafety;
                @WeaponFireAuto.started += instance.OnWeaponFireAuto;
                @WeaponFireAuto.performed += instance.OnWeaponFireAuto;
                @WeaponFireAuto.canceled += instance.OnWeaponFireAuto;
                @WeaponFireTap.started += instance.OnWeaponFireTap;
                @WeaponFireTap.performed += instance.OnWeaponFireTap;
                @WeaponFireTap.canceled += instance.OnWeaponFireTap;
                @WeaponAim.started += instance.OnWeaponAim;
                @WeaponAim.performed += instance.OnWeaponAim;
                @WeaponAim.canceled += instance.OnWeaponAim;
                @WeaponReload.started += instance.OnWeaponReload;
                @WeaponReload.performed += instance.OnWeaponReload;
                @WeaponReload.canceled += instance.OnWeaponReload;
                @WeaponHandguard.started += instance.OnWeaponHandguard;
                @WeaponHandguard.performed += instance.OnWeaponHandguard;
                @WeaponHandguard.canceled += instance.OnWeaponHandguard;
                @EquipPrimary.started += instance.OnEquipPrimary;
                @EquipPrimary.performed += instance.OnEquipPrimary;
                @EquipPrimary.canceled += instance.OnEquipPrimary;
                @EquipSecondary.started += instance.OnEquipSecondary;
                @EquipSecondary.performed += instance.OnEquipSecondary;
                @EquipSecondary.canceled += instance.OnEquipSecondary;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Customize
    private readonly InputActionMap m_Customize;
    private ICustomizeActions m_CustomizeActionsCallbackInterface;
    private readonly InputAction m_Customize_Rotation;
    private readonly InputAction m_Customize_ResetRotation;
    private readonly InputAction m_Customize_Scroll;
    public struct CustomizeActions
    {
        private @InputMap m_Wrapper;
        public CustomizeActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotation => m_Wrapper.m_Customize_Rotation;
        public InputAction @ResetRotation => m_Wrapper.m_Customize_ResetRotation;
        public InputAction @Scroll => m_Wrapper.m_Customize_Scroll;
        public InputActionMap Get() { return m_Wrapper.m_Customize; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CustomizeActions set) { return set.Get(); }
        public void SetCallbacks(ICustomizeActions instance)
        {
            if (m_Wrapper.m_CustomizeActionsCallbackInterface != null)
            {
                @Rotation.started -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnRotation;
                @ResetRotation.started -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnResetRotation;
                @ResetRotation.performed -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnResetRotation;
                @ResetRotation.canceled -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnResetRotation;
                @Scroll.started -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_CustomizeActionsCallbackInterface.OnScroll;
            }
            m_Wrapper.m_CustomizeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
                @ResetRotation.started += instance.OnResetRotation;
                @ResetRotation.performed += instance.OnResetRotation;
                @ResetRotation.canceled += instance.OnResetRotation;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
            }
        }
    }
    public CustomizeActions @Customize => new CustomizeActions(this);
    public interface IPlayerActions
    {
        void OnCameraAxis(InputAction.CallbackContext context);
        void OnMoveAxis(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnCover(InputAction.CallbackContext context);
        void OnNightVision(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnWeaponSafety(InputAction.CallbackContext context);
        void OnWeaponFireAuto(InputAction.CallbackContext context);
        void OnWeaponFireTap(InputAction.CallbackContext context);
        void OnWeaponAim(InputAction.CallbackContext context);
        void OnWeaponReload(InputAction.CallbackContext context);
        void OnWeaponHandguard(InputAction.CallbackContext context);
        void OnEquipPrimary(InputAction.CallbackContext context);
        void OnEquipSecondary(InputAction.CallbackContext context);
    }
    public interface ICustomizeActions
    {
        void OnRotation(InputAction.CallbackContext context);
        void OnResetRotation(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
}
