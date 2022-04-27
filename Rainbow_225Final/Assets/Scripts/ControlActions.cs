// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/ControlActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlActions"",
    ""maps"": [
        {
            ""name"": ""Base"",
            ""id"": ""1416481e-2e0c-4d02-aedd-68038b6a2853"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""928a5c43-942b-4ff5-bc12-65a93174906d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5e8cce1f-c6e8-4e18-bb17-fa9c016caa52"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""79d762ee-6d94-4818-93b7-85a96d1cb1d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""AutoShoot"",
                    ""type"": ""Button"",
                    ""id"": ""e1797c6d-9f36-4ad5-a781-7be07c825ae0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookUp"",
                    ""type"": ""Button"",
                    ""id"": ""30a46ea7-6e5a-40b4-a91d-c116daa11fbc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookDown"",
                    ""type"": ""Button"",
                    ""id"": ""4b3d63e1-d998-450d-ae52-81eef4212714"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""05f1684c-0450-455d-90cc-c3c29aef1d86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""AutoSkill"",
                    ""type"": ""Button"",
                    ""id"": ""4d138858-49bf-40c8-9a7d-6a98b396cf21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NextWep"",
                    ""type"": ""Button"",
                    ""id"": ""0d166792-bd4c-4c9f-b0d6-bee301c12658"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PrevWep"",
                    ""type"": ""Button"",
                    ""id"": ""6aefb54f-3309-45a9-9d18-89b57c0e4940"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ActionB"",
                    ""type"": ""Button"",
                    ""id"": ""fcf2bcb3-0d4d-410f-841e-fe94226db55f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""9e084ee8-133d-4b77-ac53-01082e56bafe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Climb"",
                    ""type"": ""Button"",
                    ""id"": ""b0d9c5c1-8f51-4d63-912b-857936b95019"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""293a8b76-575f-418c-b4fa-274181274e1e"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bdc3b57-6595-4544-81cb-4c44e07bf7d0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5599545-281f-4205-a36a-f847371aa014"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acabb290-8a60-4b79-aa92-2a2dcdf0150a"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24195764-c5e6-4bdd-82aa-c2e96667b374"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03a4fedf-9e9d-40cb-a58d-e11927f2da71"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dce0bae8-983d-4e32-8169-b6de16651f55"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""686d6189-bbb5-4aca-ad94-58df3d2dddb2"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f95d9bde-457d-4234-a85a-ac378d7ae09f"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be2227d7-d765-4982-b62b-be2d13268d9a"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""1ed9bcaa-842f-4a4d-ace5-208c8c176cb1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""50b3f1cc-388e-4fc2-b515-ea41c4616791"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""eb5b060f-739b-497e-bd82-ec00d6421ca5"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""af30ff80-107f-405d-bc72-d547d57db0a5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d5da637c-aa14-44f9-8187-e5de5475e6f6"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8dc8d6b9-f54d-444e-aaca-2f96dcd833d7"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""48710041-24e6-4d68-b805-2081cc0078bc"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextWep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d744a972-6319-493a-9b1d-2264d9d186e2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextWep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7e42c06-638d-48d3-840f-e1314d11abfb"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrevWep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""774f7972-c9a2-4381-ba46-1dcf044a7dec"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrevWep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c712180-3fd0-4c9d-abe8-dc35b5c4d53b"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ActionB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57315583-e3d1-441f-ba36-5fc554aee8fb"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ActionB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12533517-6f48-447b-babf-ff757bb1c330"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c866884-72e6-49ee-8963-678b0bfac6db"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b07f8800-25a4-4f5e-bed1-4b4ded118d50"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AutoShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9a9f012-3d7e-4365-a80c-b49afaa9bd38"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AutoShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa79cd44-5e7e-4010-a6a3-2c1013b5c125"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AutoSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0731826a-23be-4eff-af5e-1e1f1bff7855"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AutoSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""f407270f-d398-4f1d-a43f-0088818706ce"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1edd1599-f2b7-4f99-bb2f-5b8d3b73b6d2"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bb109b54-0d44-444b-a2d2-2abd15e45081"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""e434e52a-2fe7-4054-85c7-e4545f50047e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8951c83a-3ca6-4bbb-9ade-3edb0b13b25e"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a16dd113-465e-4622-a5fb-343ff07e3491"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Base
        m_Base = asset.FindActionMap("Base", throwIfNotFound: true);
        m_Base_Move = m_Base.FindAction("Move", throwIfNotFound: true);
        m_Base_Jump = m_Base.FindAction("Jump", throwIfNotFound: true);
        m_Base_Shoot = m_Base.FindAction("Shoot", throwIfNotFound: true);
        m_Base_AutoShoot = m_Base.FindAction("AutoShoot", throwIfNotFound: true);
        m_Base_LookUp = m_Base.FindAction("LookUp", throwIfNotFound: true);
        m_Base_LookDown = m_Base.FindAction("LookDown", throwIfNotFound: true);
        m_Base_Skill = m_Base.FindAction("Skill", throwIfNotFound: true);
        m_Base_AutoSkill = m_Base.FindAction("AutoSkill", throwIfNotFound: true);
        m_Base_NextWep = m_Base.FindAction("NextWep", throwIfNotFound: true);
        m_Base_PrevWep = m_Base.FindAction("PrevWep", throwIfNotFound: true);
        m_Base_ActionB = m_Base.FindAction("ActionB", throwIfNotFound: true);
        m_Base_Pause = m_Base.FindAction("Pause", throwIfNotFound: true);
        m_Base_Climb = m_Base.FindAction("Climb", throwIfNotFound: true);
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

    // Base
    private readonly InputActionMap m_Base;
    private IBaseActions m_BaseActionsCallbackInterface;
    private readonly InputAction m_Base_Move;
    private readonly InputAction m_Base_Jump;
    private readonly InputAction m_Base_Shoot;
    private readonly InputAction m_Base_AutoShoot;
    private readonly InputAction m_Base_LookUp;
    private readonly InputAction m_Base_LookDown;
    private readonly InputAction m_Base_Skill;
    private readonly InputAction m_Base_AutoSkill;
    private readonly InputAction m_Base_NextWep;
    private readonly InputAction m_Base_PrevWep;
    private readonly InputAction m_Base_ActionB;
    private readonly InputAction m_Base_Pause;
    private readonly InputAction m_Base_Climb;
    public struct BaseActions
    {
        private @ControlActions m_Wrapper;
        public BaseActions(@ControlActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Base_Move;
        public InputAction @Jump => m_Wrapper.m_Base_Jump;
        public InputAction @Shoot => m_Wrapper.m_Base_Shoot;
        public InputAction @AutoShoot => m_Wrapper.m_Base_AutoShoot;
        public InputAction @LookUp => m_Wrapper.m_Base_LookUp;
        public InputAction @LookDown => m_Wrapper.m_Base_LookDown;
        public InputAction @Skill => m_Wrapper.m_Base_Skill;
        public InputAction @AutoSkill => m_Wrapper.m_Base_AutoSkill;
        public InputAction @NextWep => m_Wrapper.m_Base_NextWep;
        public InputAction @PrevWep => m_Wrapper.m_Base_PrevWep;
        public InputAction @ActionB => m_Wrapper.m_Base_ActionB;
        public InputAction @Pause => m_Wrapper.m_Base_Pause;
        public InputAction @Climb => m_Wrapper.m_Base_Climb;
        public InputActionMap Get() { return m_Wrapper.m_Base; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BaseActions set) { return set.Get(); }
        public void SetCallbacks(IBaseActions instance)
        {
            if (m_Wrapper.m_BaseActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnJump;
                @Shoot.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnShoot;
                @AutoShoot.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnAutoShoot;
                @AutoShoot.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnAutoShoot;
                @AutoShoot.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnAutoShoot;
                @LookUp.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnLookUp;
                @LookUp.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnLookUp;
                @LookUp.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnLookUp;
                @LookDown.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnLookDown;
                @LookDown.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnLookDown;
                @LookDown.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnLookDown;
                @Skill.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnSkill;
                @Skill.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnSkill;
                @Skill.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnSkill;
                @AutoSkill.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnAutoSkill;
                @AutoSkill.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnAutoSkill;
                @AutoSkill.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnAutoSkill;
                @NextWep.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnNextWep;
                @NextWep.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnNextWep;
                @NextWep.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnNextWep;
                @PrevWep.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnPrevWep;
                @PrevWep.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnPrevWep;
                @PrevWep.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnPrevWep;
                @ActionB.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnActionB;
                @ActionB.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnActionB;
                @ActionB.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnActionB;
                @Pause.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnPause;
                @Climb.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnClimb;
                @Climb.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnClimb;
                @Climb.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnClimb;
            }
            m_Wrapper.m_BaseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @AutoShoot.started += instance.OnAutoShoot;
                @AutoShoot.performed += instance.OnAutoShoot;
                @AutoShoot.canceled += instance.OnAutoShoot;
                @LookUp.started += instance.OnLookUp;
                @LookUp.performed += instance.OnLookUp;
                @LookUp.canceled += instance.OnLookUp;
                @LookDown.started += instance.OnLookDown;
                @LookDown.performed += instance.OnLookDown;
                @LookDown.canceled += instance.OnLookDown;
                @Skill.started += instance.OnSkill;
                @Skill.performed += instance.OnSkill;
                @Skill.canceled += instance.OnSkill;
                @AutoSkill.started += instance.OnAutoSkill;
                @AutoSkill.performed += instance.OnAutoSkill;
                @AutoSkill.canceled += instance.OnAutoSkill;
                @NextWep.started += instance.OnNextWep;
                @NextWep.performed += instance.OnNextWep;
                @NextWep.canceled += instance.OnNextWep;
                @PrevWep.started += instance.OnPrevWep;
                @PrevWep.performed += instance.OnPrevWep;
                @PrevWep.canceled += instance.OnPrevWep;
                @ActionB.started += instance.OnActionB;
                @ActionB.performed += instance.OnActionB;
                @ActionB.canceled += instance.OnActionB;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Climb.started += instance.OnClimb;
                @Climb.performed += instance.OnClimb;
                @Climb.canceled += instance.OnClimb;
            }
        }
    }
    public BaseActions @Base => new BaseActions(this);
    public interface IBaseActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnAutoShoot(InputAction.CallbackContext context);
        void OnLookUp(InputAction.CallbackContext context);
        void OnLookDown(InputAction.CallbackContext context);
        void OnSkill(InputAction.CallbackContext context);
        void OnAutoSkill(InputAction.CallbackContext context);
        void OnNextWep(InputAction.CallbackContext context);
        void OnPrevWep(InputAction.CallbackContext context);
        void OnActionB(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnClimb(InputAction.CallbackContext context);
    }
}
