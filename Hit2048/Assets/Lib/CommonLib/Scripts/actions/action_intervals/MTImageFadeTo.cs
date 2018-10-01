using System;

using UnityEngine;
using UnityEngine.UI;

namespace MTUnity.Actions
{
    public class MTImageFadeTo: MTFiniteTimeAction
    {
        public float ToOpacity { get; private set; }


        #region Constructors

        public MTImageFadeTo (float duration, float opacity) : base (duration)
        {
            ToOpacity = opacity;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTImageFadeToState (this, target);

        }

        public override MTFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }

    public class MTImageFadeToState : MTFiniteTimeActionState
    {
		protected float FromOpacity { get; set; }

		protected float ToOpacity { get; set; }
        Image _image;
        public MTImageFadeToState (MTImageFadeTo action, GameObject target)
            : base (action, target)
        {              
            ToOpacity = action.ToOpacity;

            if(target != null)
            {
                _image = target.GetComponent<Image>();
                if(_image != null)
                {
                    FromOpacity = _image.color.a;
                }
            }

    //        var pRGBAProtocol = target;
    //        if (pRGBAProtocol != null)
    //        {
				//FromOpacity = pRGBAProtocol.getOpacity();
    //        }
        }

        public override void Update (float time)
        {
            if (_image != null)
            {
                var newColor = _image.color;
                newColor.a = FromOpacity + (ToOpacity - FromOpacity) * time;
                _image.color = newColor;
            }
    //        var pRGBAProtocol = Target;
    //        if (pRGBAProtocol != null)
    //        {
				//pRGBAProtocol.setOpacity (FromOpacity + (ToOpacity - FromOpacity) * time);
    //        }
        }
    }


}