///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 06/03/2025 14:55
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.ArthurSchmitz.UIInteractable {

    public interface IRenderer
    {
        public void Render(StateData eventData);
    }

    public abstract class Renderer<TParameter, TTarget> : IRenderer
    {
#if UNITY_EDITOR
        public class Editor
        {
            public const string PROPERTY_PARAMETER                 = nameof(parameter);
            public const string PROPERTY_PARAMETER_NOTINTERACTABLE = nameof(parameter_notInteractable);
            public const string PROPERTY_TARGETS                   = nameof(targets);
        }
#endif

        [SerializeField] public TParameter parameter                ;
        [SerializeField] public TParameter parameter_notInteractable;

        [SerializeField] public TTarget[]  targets  ;

        protected        abstract void Render(TParameter parameter,             TTarget  target);
        protected                 void Render(TParameter parameter, IEnumerable<TTarget> targets) { foreach (TTarget target in targets) Render(parameter, target); }
        
        void IRenderer.Render(StateData eventData) => Render(eventData.available ? parameter : parameter_notInteractable, targets); 
    }

    public abstract class Renderer<TParameter, TSettings, TTarget> : Renderer<TParameter, TTarget>, IRenderer where TSettings : SO_RendererSettings<TParameter>
    {
#if UNITY_EDITOR
        public new class Editor : Renderer<TParameter, TTarget>.Editor
        {
            public const string PROPERTY_SETTING = nameof(settings);
        }
#endif
        [SerializeField] public TSettings settings;

        void IRenderer.Render(StateData eventData)
        {
            if(settings == null)
            {
                Render(eventData.available ? parameter : parameter_notInteractable, targets);
                return;
            }

            Render(eventData.available ? settings.Parameter : settings.ParameterNotAvailable, targets);
        }
    }

    public abstract class StaticRenderer<TParameter, TSettings> : IRenderer where TSettings : SO_RendererSettings<TParameter>
    {
        [SerializeField] public TParameter parameter;
        [SerializeField] public TParameter parameter_notInteractable;

        [SerializeField] public TSettings  settings;

        protected abstract void Render(TParameter parameter);

        void IRenderer.Render(StateData eventData)
        {
            if (settings == null)
            {
                Render(eventData.available ? parameter : parameter_notInteractable);
                return;
            }

            Render(eventData.available ? settings.Parameter : settings.ParameterNotAvailable);
        }
    }

    [Serializable] public class SetActive     : Renderer<bool                  ,                GameObject     > { protected override void Render(bool                   parameter, GameObject      target) => target.SetActive      (parameter); }
    [Serializable] public class Enable        : Renderer<bool                  ,                Behaviour      > { protected override void Render(bool                   parameter, Behaviour       target) => target.enabled       = parameter ; }
    [Serializable] public class Position      : Renderer<Vector3               , RS_Vector3   , Transform      > { protected override void Render(Vector3                parameter, Transform       target) => target.localPosition = parameter ; }
    [Serializable] public class Scale         : Renderer<Vector3               , RS_Vector3   , Transform      > { protected override void Render(Vector3                parameter, Transform       target) => target.localScale    = parameter ; }
    [Serializable] public class Tint          : Renderer<Color                 , RS_Tint      , Image          > { protected override void Render(Color                  parameter, Image           target) => target.color         = parameter ; }
    [Serializable] public class SetSprite     : Renderer<Sprite                , RS_Sprite    , Image          > { protected override void Render(Sprite                 parameter, Image           target) => target.sprite        = parameter ; }
    [Serializable] public class StyleSheet    : Renderer<TMP_StyleSheet        , RS_StyleSheet, TextMeshProUGUI> { protected override void Render(TMP_StyleSheet         parameter, TextMeshProUGUI target) => target.styleSheet    = parameter ; }
    [Serializable] public class Alpha         : Renderer<float                 , RS_Alpha     , CanvasGroup    > { protected override void Render(float                  parameter, CanvasGroup     target) => target.alpha         = parameter ; }
    [Serializable] public class SetState      : Renderer<UIInteractable.E_State,                UIInteractable > { protected override void Render(UIInteractable.E_State parameter, UIInteractable  target) => target.SetState       (parameter); }
    [Serializable] public class SetEvent      : Renderer<UIInteractable.E_Event,                UIInteractable > { protected override void Render(UIInteractable.E_Event parameter, UIInteractable  target) => target.SetState       (parameter); }
    [Serializable] public class Sound         : Renderer<string                ,                UIInteractable > { protected override void Render(string                 parameter, UIInteractable target) => Debug.Log("Sound implementation goes here"); }
}