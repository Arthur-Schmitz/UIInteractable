///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 10/03/2025 11:19
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable {

    // ALEX 22/01/26 : Placehodler to remove
    public class UIAudio : MonoBehaviour 
    {
        private static UIAudio _instance;
        
        private       void InstancePlaySound(string sound) 
        {
            /*IMPLEMENT HERE ON FMOD IMPLEMENTATION*/
            Debug.Log(nameof(UIAudio) + $": play {sound}");
        }
        public static void PlaySound(string sound)
        {
            if(_instance == null)
            {
                throw new NullReferenceException($"{nameof(UIAudio)}: cannot play {sound}, there is no {nameof(UIAudio)} instance on this scene");
            }

            _instance.InstancePlaySound(sound);
        }

        protected virtual void Awake()
        {
            if(_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this;
        }

        protected virtual void OnDestroy()
        {
            if(this == _instance)
            {
                _instance = null;
            }
        }
    }
}