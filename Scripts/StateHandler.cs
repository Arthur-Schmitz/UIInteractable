///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 18/03/2025 12:13
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable {

    public abstract class StateHandler
    {
        public virtual void Actualize         (StateData data                                                               ) { } 
        public static  void Actualize         (StateData data,                 params               StateHandler [] handlers) => Array.ForEach(handlers, handler => handler.Actualize(data));
        public static  void Actualize<TParent>(StateData data, TParent parent, params Func<TParent, StateHandler>[] handlers) => Actualize(data, Array.ConvertAll(handlers, handler => handler(parent)));

        public virtual void SetDefaultState         (StateData data                                                               ) { }
        public static  void SetDefaultState         (StateData data,                 params               StateHandler [] handlers) => Array.ForEach(handlers, handler => handler.SetDefaultState(data));
        public static  void SetDefaultState<TParent>(StateData data, TParent parent, params Func<TParent, StateHandler>[] handlers) => SetDefaultState(data, Array.ConvertAll(handlers, handler => handler(parent)));

        public    virtual void RemoveAllListeners         (                                                             ) { }
        protected static  void RemoveAllListeners         (                                     StateHandler    handler ) => handler.RemoveAllListeners();
        public    static  void RemoveAllListeners         (                params               StateHandler [] handlers) => Array.ForEach(handlers, RemoveAllListeners);
        public    static  void RemoveAllListeners<TParent>(TParent parent, params Func<TParent, StateHandler>[] handlers) => RemoveAllListeners(Array.ConvertAll(handlers, handler => handler(parent)));

        public    virtual void OnValidate         (                                                             ) { }
        protected static  void OnValidate         (                                     StateHandler    handler ) => handler.OnValidate();
        public    static  void OnValidate         (                params               StateHandler [] handlers) => Array.ForEach(handlers, OnValidate);
        public    static  void OnValidate<TParent>(TParent parent, params Func<TParent, StateHandler>[] handlers) => OnValidate(Array.ConvertAll(handlers, handler => handler(parent)));

        public    virtual void Awake         (StateData data                                                               ) { }
        public    static  void Awake         (StateData data,                 params               StateHandler [] handlers) => Array.ForEach(handlers, handler => handler.Awake(data));
        public    static  void Awake<TParent>(StateData data, TParent parent, params Func<TParent, StateHandler>[] handlers) => Awake(data, Array.ConvertAll(handlers, handler => handler(parent)));
    }

    [Serializable]
    public class StateHandler<TState> : StateHandler
    {
        [Serializable]
        public struct Chain<TChainedState>
        {
            public TState          state;
            public TChainedState[] chainedStates;

            public static Dictionary<TState, TChainedState[]> CreateChain(Chain<TChainedState>[] chains)
            {
                Dictionary<TState, TChainedState[]> yield = new();
                foreach (Chain<TChainedState> chain in chains)
                {
                    if (yield.TryGetValue(chain.state, out TChainedState[] current))
                    {
                        List<TChainedState> _current = new(current);
                        _current.AddRange(chain.chainedStates);
                        yield[chain.state] = _current.ToArray();
                    }
                    else yield.Add(chain.state, chain.chainedStates);
                }
                return yield;
            }

            public static void CallChain(TState state, Action<TChainedState> execute, Dictionary<TState, TChainedState[]> chainedStates)
            {
                if(chainedStates.TryGetValue(state, out TChainedState[] _chainedStates))
                {
                    Array.ForEach(_chainedStates, execute);
                }
            }
        }

        [SerializeField] private             TState    _currentState;
        [SerializeField] public              TState     defaultState;
        [SerializeField] private Interaction<TState>[]  interactions = new Interaction<TState>[0];

        public TState CurrentState => _currentState;

        private Dictionary<TState, Interaction<TState>> interactionsByState = new();

        private void Render(TState state, StateData eventData)
        {
            if (interactionsByState != null)
            {
                interactionsByState[state].Render(eventData);
            }
            else
            {
                Interaction<TState>.Find(state, interactions)?.Render(eventData);
            }
        }

        public void Execute(TState state, StateData eventData)
        {
            if (interactionsByState != null)
            {
                interactionsByState[state].Execute(eventData);
            }
            else
            {
                Interaction<TState>.Find(state, interactions)?.Execute(eventData);
            }
        }

        public override void Actualize(StateData data)
        {
            base.Actualize(data);

            Render(_currentState, data);
        }

        public void SetState(TState state, StateData data)
        {
            if (state.Equals(default)) return;

            Execute(_currentState = state, data);
        }

        public void SetSilentState(TState state, StateData data)
        {
            if (state.Equals(default)) return;

            Render(_currentState = state, data);
        }

        public override void SetDefaultState(StateData data)
        {
            base.SetDefaultState(data);
            SetSilentState(defaultState, data);
        }

        public void AddListener   (TState state, Interaction<TState>.Listener listener) => interactionsByState[state].OnTriggered += listener;
        public void RemoveListener(TState state, Interaction<TState>.Listener listener) => interactionsByState[state].OnTriggered -= listener;

        public          void RemoveAllListeners(TState state) => Interaction<TState>.RemoveAllListeners(interactionsByState[state]);
        public override void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            Interaction<TState>.RemoveAllListeners(interactionsByState.Values.ToArray());
        }

        public override void OnValidate()
        {
            base.OnValidate();
            Interaction<TState>.OnValidate(interactions);
        }

        public override void Awake(StateData stateData)
        {
            base.Awake(stateData);

            interactionsByState = new();
            foreach (Interaction<TState> interaction in interactions)
            {
                if (!interactionsByState.TryGetValue(interaction.state, out Interaction<TState> _interaction))
                {
                    interactionsByState.Add(interaction.state, new(interaction));
                }
                else _interaction.signs.AddRange(interaction.signs);
            }

            foreach (TState state in Enum.GetValues(typeof(TState)))
            {
                if (!interactionsByState.ContainsKey(state))
                    interactionsByState.Add(state, new(state));
            }

            SetDefaultState(stateData);
        }
    }
}