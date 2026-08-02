///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 06/03/2025 13:13
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.ArthurSchmitz.UIInteractable 
{
    [Serializable]
    public struct Sign
    {
        public enum E_Type
        {
            None                   = 000,
            GameObjectSetActive    = 100,
            BehaviourEnable        = 200,
            TransformScale         = 300,
            TransformPosition      = 350,
            ImageTint              = 400,
            ImageSprite            = 410,
            Sound                  = 500,
            TextStyleSheet         = 600,
            CanvasGroupAlpha       = 700,
            UIInteractableSetState = 800,
            // ADD MORE TYPES HERE
        }

        private delegate IRenderer Property(Sign sign);

        private static readonly Dictionary<E_Type, Property> PROPERTIES_BY_MODE = new()
        {
            { E_Type.GameObjectSetActive   , (Sign sign) => sign.setActive  },
            { E_Type.BehaviourEnable       , (Sign sign) => sign.enable     },
            { E_Type.TransformScale        , (Sign sign) => sign.scale      },
            { E_Type.TransformPosition     , (Sign sign) => sign.position   },
            { E_Type.ImageTint             , (Sign sign) => sign.tint       },
            { E_Type.ImageSprite           , (Sign sign) => sign.sprite     },
            { E_Type.Sound                 , (Sign sign) => sign.sound      },
            { E_Type.TextStyleSheet        , (Sign sign) => sign.styleSheet },
            { E_Type.CanvasGroupAlpha      , (Sign sign) => sign.alpha      },
            { E_Type.UIInteractableSetState, (Sign sign) => sign.setState   },
        };

#if UNITY_EDITOR
        public class Editor
        {
            public const string PROPERTY_TYPE = nameof(type);

            public static readonly Dictionary<E_Type, string> VALUES_BY_SIGN = new()
            {
                { E_Type.GameObjectSetActive   , nameof(setActive) },
                { E_Type.BehaviourEnable       , nameof(enable)    },
                { E_Type.TransformScale        , nameof(scale)     },
                { E_Type.TransformPosition     , nameof(position)  },
                { E_Type.ImageTint             , nameof(tint)      },
                { E_Type.ImageSprite           , nameof(sprite)    },
                { E_Type.Sound                 , nameof(sound)     },
                { E_Type.TextStyleSheet        , nameof(styleSheet)},
                { E_Type.CanvasGroupAlpha      , nameof(alpha)     },
                { E_Type.UIInteractableSetState, nameof(setState)  },
            };
        }
#endif

        [SerializeField] public E_Type        type      ;
        [SerializeField] public SetActive     setActive ;
        [SerializeField] public Enable        enable    ;
        [SerializeField] public Position      position  ;
        [SerializeField] public Scale         scale     ;
        [SerializeField] public Tint          tint      ;
        [SerializeField] public SetSprite     sprite    ;
        [SerializeField] public Sound         sound     ;
        [SerializeField] public StyleSheet    styleSheet;
        [SerializeField] public Alpha         alpha     ;
        [SerializeField] public SetState      setState  ;

        private void Render(E_Type mode, StateData eventData) 
        {
            if(PROPERTIES_BY_MODE.TryGetValue(mode, out Property property))
            {
                property(this).Render(eventData);
            }   
        }
        public        void Render(              StateData eventData) => Render(type, eventData);
        public static void Render(Sign   sign , StateData eventData) => sign.Render(eventData);
        public static void Render(Sign[] signs, StateData eventData) => Array.ForEach(signs, sign => Render(sign, eventData));

        public        void OnValidate(            ) { }
        public static void OnValidate(Sign   sign ) => sign.OnValidate();
        public static void OnValidate(Sign[] signs) => Array.ForEach(signs, OnValidate);
    }
}