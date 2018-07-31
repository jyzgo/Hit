using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MTUnity.Actions
{

    public struct VectorWithInterval
    {
       public float duration;
       public Vector3 vec;
    }
    public class MTScaleSeq: MTFiniteTimeAction
    {


        public static MTScaleSeq Init(params VectorWithInterval[] arr)
        {
            Debug.Assert(arr.Length   > 0);
            float duration = 0f;
            for(int i = 0; i < arr.Length;i++)
            {
                duration += arr[i].duration;
            }
            return new MTScaleSeq(duration,arr);
        }

        public VectorWithInterval[] SeqArr { get; private set; }

        #region Constructors

        MTScaleSeq(float totalDuration,params VectorWithInterval[] arr):base(totalDuration)
        {
            SeqArr = arr;
        }



        #endregion Constructors

        public override MTFiniteTimeAction Reverse()
        {
            throw new System.NotImplementedException ();
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTScaleSeqState (this, target);
        }
    }

    public class MTScaleSeqState : MTFiniteTimeActionState
    {
        protected float DeltaX;
        protected float DeltaY;
		protected float DeltaZ;

        protected float EndScaleX;
        protected float EndScaleY;
		protected float EndScaleZ;

        protected float StartScaleX;
        protected float StartScaleY;
		protected float StartScaleZ;

        protected VectorWithInterval[] VectorWithInteravalArr;

        public MTScaleSeqState (MTScaleSeq action, GameObject target)
            : base (action, target)
        { 
			if(target == null)
			{
				return;
			}
			StartScaleX = target.transform.localScale.x;
			StartScaleY = target.transform.localScale.y;
			StartScaleZ = target.transform.localScale.z;


        }

        public override void Update (float time)
        {
            if (Target != null)
            {
               	var ScaleX = StartScaleX + DeltaX * time;
                var ScaleY = StartScaleY + DeltaY * time;
				var ScaleZ = StartScaleZ + DeltaZ * time;
				Target.transform.localScale = new Vector3 (ScaleX, ScaleY, ScaleZ);
            }
        }

        protected internal override void Stop()
        {
            Target.transform.localScale = VectorWithInteravalArr[VectorWithInteravalArr.Length - 1].vec;
        }

       

        
    }
}
