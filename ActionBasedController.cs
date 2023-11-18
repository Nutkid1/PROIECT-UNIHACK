using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// <see cref="XRBaseController"/> <see cref="MonoBehaviour"/> 
    /// </summary>
    [AddComponentMenu("XR/XR Controller (Action-based)")]
    public class ActionBasedController : XRBaseController
    {
        [SerializeField]
        InputActionProperty m_PositionAction;
        /// <summary>
        
        /// </summary>
        public InputActionProperty positionAction
        {
            get => m_PositionAction;
            set => SetInputActionProperty(ref m_PositionAction, value);
        }

        [SerializeField]
        InputActionProperty m_RotationAction;
        /// <summary>
       
        /// </summary>
        public InputActionProperty rotationAction
        {
            get => m_RotationAction;
            set => SetInputActionProperty(ref m_RotationAction, value);
        }

        [SerializeField]
        InputActionProperty m_SelectAction;
        /// <summary>
        
        /// </summary>
        public InputActionProperty selectAction
        {
            get => m_SelectAction;
            set => SetInputActionProperty(ref m_SelectAction, value);
        }

        [SerializeField]
        InputActionProperty m_ActivateAction;
        /// <summary>
        
        /// </summary>
        public InputActionProperty activateAction
        {
            get => m_ActivateAction;
            set => SetInputActionProperty(ref m_ActivateAction, value);
        }

        [SerializeField]
        InputActionProperty m_UIPressAction;
        /// <summary>
        
        /// </summary>
        public InputActionProperty uiPressAction
        {
            get => m_UIPressAction;
            set => SetInputActionProperty(ref m_UIPressAction, value);
        }

        [SerializeField]
        InputActionProperty m_HapticDeviceAction;
        /// <summary>
        
        /// </summary>
        public InputActionProperty hapticDeviceAction
        {
            get => m_HapticDeviceAction;
            set => SetInputActionProperty(ref m_HapticDeviceAction, value);
        }

        [SerializeField]
        InputActionProperty m_RotateAnchorAction;
        /// <summary>
        
        /// </summary>
        public InputActionProperty rotateAnchorAction
        {
            get => m_RotateAnchorAction;
            set => SetInputActionProperty(ref m_RotateAnchorAction, value);
        }

        [SerializeField]
        InputActionProperty m_TranslateAnchorAction;
        /// <summary>
      
        /// Must be a <see cref="Vector2Control"/> Control. Will use the Y-axis as the translation input.
        /// </summary>
        public InputActionProperty translateAnchorAction
        {
            get => m_TranslateAnchorAction;
            set => SetInputActionProperty(ref m_TranslateAnchorAction, value);
        }

        [SerializeField]
        float m_ButtonPressPoint = 0.5f;

        /// <summary>
     
        /// If a button has a value equal to or greater than this value, it is considered pressed.
        /// </summary>
        [Obsolete("Deprecated, this property will be removed when Input System dependency version is bumped to 1.1.0.")]
        public float buttonPressPoint
        {
            get => m_ButtonPressPoint;
            set => m_ButtonPressPoint = value;
        }

        /// <inheritdoc />
        protected override void OnEnable()
        {
            base.OnEnable();
            EnableAllDirectActions();
        }

        /// <inheritdoc />
        protected override void OnDisable()
        {
            base.OnDisable();
            DisableAllDirectActions();
        }

        /// <inheritdoc />
        protected override void UpdateTrackingInput(XRControllerState controllerState)
        {
            if (controllerState == null)
                return;

            if (m_PositionAction.action != null)
            {
                var pos = m_PositionAction.action.ReadValue<Vector3>();
                controllerState.position = pos;
                controllerState.poseDataFlags |= PoseDataFlags.Position;
            }

            if (m_RotationAction.action != null)
            {
                var rot = m_RotationAction.action.ReadValue<Quaternion>();
                controllerState.rotation = rot;
                controllerState.poseDataFlags |= PoseDataFlags.Rotation;
            }
        }

        /// <inheritdoc />
        protected override void UpdateInput(XRControllerState controllerState)
        {
            if (controllerState == null)
                return;

            controllerState.ResetFrameDependentStates();

            ComputeInteractionActionStates(IsPressed(m_SelectAction), ref controllerState.selectInteractionState);
            ComputeInteractionActionStates(IsPressed(m_ActivateAction), ref controllerState.activateInteractionState);
            ComputeInteractionActionStates(IsPressed(m_UIPressAction), ref controllerState.uiPressInteractionState);

            bool IsPressed(InputActionProperty property)
            {
                var action = property.action;
                if (action == null)
                    return false;

                return action.triggered || action.phase == InputActionPhase.Performed || action.ReadValue<float>() >= m_ButtonPressPoint;
            }
        }

        /// <inheritdoc />
        public override bool SendHapticImpulse(float amplitude, float duration)
        {
            if (m_HapticDeviceAction.action?.activeControl?.device is XRControllerWithRumble rumbleController)
            {
                rumbleController.SendImpulse(amplitude, duration);
                return true;
            }

            return false;
        }

        void EnableAllDirectActions()
        {
            m_PositionAction.EnableDirectAction();
            m_RotationAction.EnableDirectAction();
            m_SelectAction.EnableDirectAction();
            m_ActivateAction.EnableDirectAction();
            m_UIPressAction.EnableDirectAction();
            m_HapticDeviceAction.EnableDirectAction();
            m_RotateAnchorAction.EnableDirectAction();
            m_TranslateAnchorAction.EnableDirectAction();
        }

        void DisableAllDirectActions()
        {
            m_PositionAction.DisableDirectAction();
            m_RotationAction.DisableDirectAction();
            m_SelectAction.DisableDirectAction();
            m_ActivateAction.DisableDirectAction();
            m_UIPressAction.DisableDirectAction();
            m_HapticDeviceAction.DisableDirectAction();
            m_RotateAnchorAction.DisableDirectAction();
            m_TranslateAnchorAction.DisableDirectAction();
        }

        void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
        {
            if (Application.isPlaying)
                property.DisableDirectAction();

            property = value;

            if (Application.isPlaying && isActiveAndEnabled)
                property.EnableDirectAction();
        }

        static void ComputeInteractionActionStates(bool pressed, ref InteractionState interactionState)
        {    
            if (pressed)
            {
                if (!interactionState.active)
                {
                    interactionState.activatedThisFrame = true;
                    interactionState.active = true;
                }
            }
            else
            {
                if (interactionState.active)
                {
                    interactionState.deactivatedThisFrame = true;
                    interactionState.active = false;
                }
            }
        }
    }
}
