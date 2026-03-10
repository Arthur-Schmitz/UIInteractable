///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 07/03/2025 18:08
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable.Testing {

    public class Tester : MonoBehaviour {

        public UIInteractable interactable;

        private void Start() {
            interactable.AddListener(UIInteractable.E_State.PointerDown_Left, Log);
        }

        private void Log<TState>(Interaction<TState> interaction, StateData eventData)
        { 
            Debug.Log(interaction.state + "\n" + eventData);
        }
    }
}