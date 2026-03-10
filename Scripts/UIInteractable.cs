///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 06/03/2025 12:44
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.ArthurSchmitz.UIInteractable
{
    public class UIInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        public const string PATH_PROJECTSETTINGS_FOLDER = "Assets/UIInteractable/Resources/";
        public const string PATH_PROJECTSETTINGS_NAME   = "UIInteractableProjectSettings";
        public const string PATH_PROJECTSETTINGS_ASSET  = PATH_PROJECTSETTINGS_FOLDER + PATH_PROJECTSETTINGS_NAME+ ".asset";
        public enum E_State
        {
            None         = 000,

            Select       = 100,

            Deselect     = 200,
            
            Submit       = 300,
            
            PointerEnter = 400,
            
            PointerExit  = 500,
            
            PointerDown_Left   = 600,
            PointerDown_Right  = 610,
            PointerDown_Middle = 620,
            
            PointerUp_Left    = 700,
            PointerUp_Right   = 710,
            PointerUp_Middle  = 720,

            PointerClick_Left   = 800,
            PointerClick_Right  = 810,
            PointerClick_Middle = 820,

            Move         = 900
        }

        public enum E_Event
        {
            None              = 000,

            InteractableTrue  = 100,
            InteractableFalse = 101,
            
            ToggledTrue       = 200,
            ToggledFalse      = 201,

            SelectedTrue      = 300,
            SelectedFalse     = 301,

            AvailableTrue     = 400,
            AvailableFalse    = 401,
        }

        public class PROPERTIES
        {
            public const string SETTINGS   = nameof(settings);
            public const string INIT_AWAKE = nameof(initializeOnAwake);
            public const string AVAILABLE  = nameof(_available);
            public const string TOGGLED    = nameof(_toggled);
            public const string SELECTED   = nameof(_selected);
            public const string STATES     = nameof(states);
            public const string EVENTS     = nameof(events);
        }

        [SerializeField] private SO_UIInteractableSettings settings                 ;
        [SerializeField] private bool                      initializeOnAwake = true ;

        [SerializeField] private bool _available          = true ;
        [SerializeField] private bool _toggled            = false;
        [SerializeField] private bool _selected           = false;

        [SerializeField] private StateHandler<E_State> states = new();
        [SerializeField] private StateHandler<E_Event> events = new();

        private static readonly Func<UIInteractable, StateHandler>[] HANDLERS =
        {
            (UIInteractable interactable) => interactable.states,
            (UIInteractable interactable) => interactable.events,
        };

        public bool Interactable
        { 
            get => enabled; 
            set
            {
                if (!value) SetState(E_State.PointerExit);

                enabled = value;
            }
        }

        public bool Available
        {
            get => _available;
            set => SetState((_available = value) ? E_Event.AvailableTrue : E_Event.AvailableFalse);
        }

        public bool Toggled
        {
            get => _toggled;
            set => SetState((_toggled = value) ? E_Event.ToggledTrue : E_Event.ToggledFalse);
        }

        public bool Selected
        {
            get => _selected;
            set => SetState((_selected = value) ? E_Event.SelectedTrue : E_Event.SelectedFalse);
        }

        public StateData StateData => new(name, Available);

        public void Actualize         () => StateHandler.Actualize         (StateData, this, HANDLERS);
        public void SetDefaultState   () => StateHandler.SetDefaultState   (StateData, this, HANDLERS);
        public void RemoveAllListeners() => StateHandler.RemoveAllListeners(           this, HANDLERS);

        public void SetState(E_State state)
        {
            if (!Interactable) return;
            states.SetState(state, StateData);
            settings.Runtime.ChainEvents(state, SetState);
        }

        public void AddListener       (E_State state, Interaction<E_State>.Listener listener) => states.AddListener(state, listener);
        public void RemoveListener    (E_State state, Interaction<E_State>.Listener listener) => states.RemoveListener(state, listener);
        public void RemoveAllListeners(E_State state                                        ) => states.RemoveAllListeners(state);

        public void SetState(E_Event @event)
        {
            events.SetState(@event, StateData);
            states.Actualize(StateData);
            settings.Runtime.ChainStates(@event, SetState);
        }

        public void AddListener       (E_Event @event, Interaction<E_Event>.Listener listener) => events.AddListener       (@event, listener);
        public void RemoveListener    (E_Event @event, Interaction<E_Event>.Listener listener) => events.RemoveListener    (@event, listener);
        public void RemoveAllListeners(E_Event @event                                        ) => events.RemoveAllListeners(@event);

        [ContextMenu(nameof(ToggleAvailable))]
        public void ToggleAvailable() => Available = !Available;

        public void Toggle(bool isToggled) => Toggled = isToggled;
        [ContextMenu(nameof(Toggle))]
        public void Toggle(              ) => Toggled = !Toggled;

        [ContextMenu(nameof(Select))]
        public void Select()
        {
            Selected = true;
        }

        [ContextMenu(nameof(Unselect))]
        public void Unselect() => Selected = false;

        public void Initialize()
        {
            if (settings.DefaultState != E_State.None) states.defaultState = settings.DefaultState;

            StateHandler.Awake(StateData, this, HANDLERS);
            SetState(Interactable ? E_Event.InteractableTrue : E_Event.InteractableFalse);
        }

        // Pointer click is called when Pointer_Up
        public void OnPointerClick(PointerEventData eventData)
        {
            SetState(eventData.button switch
            {
                PointerEventData.InputButton.Left   => E_State.PointerClick_Left,
                PointerEventData.InputButton.Right  => E_State.PointerClick_Right,
                PointerEventData.InputButton.Middle => E_State.PointerClick_Middle,
                _                                   => E_State.PointerClick_Left
            });
        }

        public void OnPointerEnter(PointerEventData eventData) => SetState(E_State.PointerEnter);
        public void OnPointerExit(PointerEventData eventData) => SetState(E_State.PointerExit);

        public void OnPointerDown(PointerEventData eventData)
        {
            SetState(eventData.button switch
            {
                PointerEventData.InputButton.Left   => E_State.PointerDown_Left,
                PointerEventData.InputButton.Right  => E_State.PointerDown_Right,
                PointerEventData.InputButton.Middle => E_State.PointerDown_Middle,
                _                                   => E_State.PointerDown_Left
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetState(eventData.button switch
            {
                PointerEventData.InputButton.Left   => E_State.PointerUp_Left,
                PointerEventData.InputButton.Right  => E_State.PointerUp_Right,
                PointerEventData.InputButton.Middle => E_State.PointerUp_Middle,
                _                                   => E_State.PointerUp_Left
            });
        }

        public void OnSubmit(BaseEventData eventData) => SetState(E_State.Submit);

        public void OnSelect(BaseEventData eventData) => SetState(E_State.Select);

        public void OnDeselect(BaseEventData eventData) => SetState(E_State.Deselect);

        public void OnMove(AxisEventData eventData) => SetState(E_State.Move);

        protected void OnValidate()
        {
            states.OnValidate();
            events.OnValidate();
        }

        protected void Awake()
        {
            if (initializeOnAwake) Initialize();
        }

        protected void OnEnable()
        {
            SetDefaultState();
            SetState(E_Event.InteractableTrue);
        }

        protected void OnDisable()
        {
            SetState(E_Event.InteractableFalse);
        }
    }
}