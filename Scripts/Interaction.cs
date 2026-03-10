///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 06/03/2025 15:35
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable
{
    [Serializable]
    public abstract class Interaction
    {

    }

    [Serializable]
    public class Interaction<TState> : Interaction, IEquatable<TState>
    {
#if UNITY_EDITOR
        public class Editor
        {
            public const string PROPERTY_STATE = nameof(state);
            public const string PROPERTY_SIGNS = nameof(signs);

        }
#endif

        public delegate void Listener(Interaction<TState> interaction, StateData data);

        [SerializeField] public TState     state;
        [SerializeField] public List<Sign> signs;

        public event Listener OnTriggered;

        public Interaction(TState state)
        {
            this.state  = state;
            signs       = new List<Sign>();
            OnTriggered = null;
        }

        public Interaction(Interaction<TState> original)
        {
            state       = original.state;
            signs       = original.signs;
            OnTriggered = original.OnTriggered;
        }

        public void Render (StateData data) => Sign.Render(signs.ToArray(), data);
        public void Trigger(StateData data) => OnTriggered?.Invoke(this, data);
        public void Execute(StateData data)
        {
            Render (data);
            Trigger(data);
        }
        
        public static void RemoveAllListeners(Interaction<TState>   interaction ) => interaction.OnTriggered = null;
        public static void RemoveAllListeners(Interaction<TState>[] interactions) => Array.ForEach(interactions, RemoveAllListeners);

        public void OnValidate()
        {
            Sign.OnValidate(signs.ToArray());
        }
        private static void OnValidate(       Interaction<TState>   interaction ) => interaction.OnValidate();
        public  static void OnValidate(params Interaction<TState>[] interactions) => Array.ForEach(interactions, OnValidate);

        public bool Equals(TState other) => state.Equals(other);
        public static Interaction<TState> Find(TState state, params  Interaction<TState>[] interactions) => Array.Find(interactions, interaction => interaction.Equals(state));
    }
}