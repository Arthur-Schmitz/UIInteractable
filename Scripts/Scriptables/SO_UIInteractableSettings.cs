///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 19/03/2025 11:53
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable
{
    [CreateAssetMenu(fileName ="UIS_", menuName = nameof(ArthurSchmitz.UIInteractable)+"/Settings/UIInteractable")]
    public class SO_UIInteractableSettings : ScriptableObject 
    {
        public class RuntimeHandler
        {
            private readonly Dictionary<UIInteractable.E_Event, UIInteractable.E_State[]> chainedStates;
            private readonly Dictionary<UIInteractable.E_State, UIInteractable.E_Event[]> chainedEvents;

            
            public RuntimeHandler(SO_UIInteractableSettings settings)
            {
                chainedStates = StateHandler<UIInteractable.E_Event>.Chain<UIInteractable.E_State>.CreateChain(settings.chainedStates);
                chainedEvents = StateHandler<UIInteractable.E_State>.Chain<UIInteractable.E_Event>.CreateChain(settings.chainedEvents);
            }

            public void ChainStates(UIInteractable.E_Event @event, Action<UIInteractable.E_State> execute) => StateHandler<UIInteractable.E_Event>.Chain<UIInteractable.E_State>.CallChain(@event, execute, chainedStates);
            public void ChainEvents(UIInteractable.E_State state , Action<UIInteractable.E_Event> execute) => StateHandler<UIInteractable.E_State>.Chain<UIInteractable.E_Event>.CallChain(state , execute, chainedEvents);
        }

        [field: SerializeField] public UIInteractable.E_State DefaultState { get; private set; }

        [SerializeField] private StateHandler<UIInteractable.E_Event>.Chain<UIInteractable.E_State>[] chainedStates;
        [SerializeField] private StateHandler<UIInteractable.E_State>.Chain<UIInteractable.E_Event>[] chainedEvents;

        private RuntimeHandler _runtime = null;
        public RuntimeHandler Runtime => _runtime ??= new RuntimeHandler(this);
    }
}