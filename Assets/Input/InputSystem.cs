// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputSystem
{
    public class @Controls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""PCPlayer"",
            ""id"": ""a121fc29-9c44-4bbc-afcb-635c477a7fd7"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a9bf5818-02b6-4eaf-a9ef-aea21302ca09"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""6f345d7e-c877-4471-8eea-806c44fbb601"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e5801a8b-4c10-4c0c-8970-c1d04e08ca44"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ca65ec8c-f806-4b99-886a-439e2899e049"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1442012e-df7e-4451-94d7-7b873e0f7f43"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0fe76593-650a-4825-ad03-cc8428941464"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""XBOXPlayer"",
            ""id"": ""f6f8ad28-fe78-44d8-aa08-e9aa198f0e3f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""b8efd88f-7d15-4ba4-ad33-19248c409e03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""14ee752e-f468-471d-8456-e40a5a5ba21a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d5407878-11ba-4fbd-9caf-f7c5c6dbdd02"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9d069540-384e-4f87-a4d4-ec28626a9df8"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""51a2c2e8-feac-405f-8fa5-c2c046f652f6"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4352fba2-8706-409a-b5de-37f28e4f64b6"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // PCPlayer
            m_PCPlayer = asset.FindActionMap("PCPlayer", throwIfNotFound: true);
            m_PCPlayer_Move = m_PCPlayer.FindAction("Move", throwIfNotFound: true);
            // XBOXPlayer
            m_XBOXPlayer = asset.FindActionMap("XBOXPlayer", throwIfNotFound: true);
            m_XBOXPlayer_Move = m_XBOXPlayer.FindAction("Move", throwIfNotFound: true);
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

        // PCPlayer
        private readonly InputActionMap m_PCPlayer;
        private IPCPlayerActions m_PCPlayerActionsCallbackInterface;
        private readonly InputAction m_PCPlayer_Move;
        public struct PCPlayerActions
        {
            private @Controls m_Wrapper;
            public PCPlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_PCPlayer_Move;
            public InputActionMap Get() { return m_Wrapper.m_PCPlayer; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PCPlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPCPlayerActions instance)
            {
                if (m_Wrapper.m_PCPlayerActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_PCPlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PCPlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PCPlayerActionsCallbackInterface.OnMove;
                }
                m_Wrapper.m_PCPlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                }
            }
        }
        public PCPlayerActions @PCPlayer => new PCPlayerActions(this);

        // XBOXPlayer
        private readonly InputActionMap m_XBOXPlayer;
        private IXBOXPlayerActions m_XBOXPlayerActionsCallbackInterface;
        private readonly InputAction m_XBOXPlayer_Move;
        public struct XBOXPlayerActions
        {
            private @Controls m_Wrapper;
            public XBOXPlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_XBOXPlayer_Move;
            public InputActionMap Get() { return m_Wrapper.m_XBOXPlayer; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(XBOXPlayerActions set) { return set.Get(); }
            public void SetCallbacks(IXBOXPlayerActions instance)
            {
                if (m_Wrapper.m_XBOXPlayerActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_XBOXPlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_XBOXPlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_XBOXPlayerActionsCallbackInterface.OnMove;
                }
                m_Wrapper.m_XBOXPlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                }
            }
        }
        public XBOXPlayerActions @XBOXPlayer => new XBOXPlayerActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IPCPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
        }
        public interface IXBOXPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
        }
    }
}
