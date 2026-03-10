///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 07/03/2025 16:27
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Serialization;

namespace Com.ArthurSchmitz.UIInteractable {
    public class SO_RendererSettings<TParameter> : ScriptableObject
    {
        [field: SerializeField] 
        public TParameter Parameter                { get; private set; }
        
        [SerializeField] private bool hasNotAvailableParameter;

        [field: SerializeField, 
               FormerlySerializedAs("<ParameterNotInteractable>k__BackingField")] 
        public TParameter ParameterNotAvailable { get; private set; }

        protected virtual void OnValidate()
        {
            ParameterNotAvailable = hasNotAvailableParameter ? ParameterNotAvailable : Parameter;
        }
    }
}
