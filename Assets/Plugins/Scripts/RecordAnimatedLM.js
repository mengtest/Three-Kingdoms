 #pragma strict
 
import System.Collections.Generic;
#if UNITY_EDITOR
import System.IO;
import System;
import System.Reflection;
#endif
 
@script ExecuteInEditMode
 class RecordAnimatedLM extends MonoBehaviour
 {
 var baking = false;
 public var Sample:float = 0;
 
   public function DestroyMe(caller:System.Object)
    {
		var callerType:System.Type = caller.GetType();
		var method:System.Reflection.MethodInfo = callerType.GetMethod("stopLightmapping");
		var objs:System.Object[] = new System.Object[0];
		//objs[0] = null;
		method.Invoke(caller,objs);
        //caller.stopLightmapping();
        baking = false;
        //AnimationUtility.StopAnimationMode();
        StopCoroutine("RecordAnimation");
        DestroyImmediate(this);
        
    }
    public function RecordAnimation(param:System.Object)
    {
        baking = true;
      
        var paramType:System.Type = param.GetType();
        var field1:System.Reflection.FieldInfo = paramType.GetField("animatedLightmaps");
        var ANField:System.Object = field1.GetValue(param);
        var ANType:System.Type = ANField.GetType();
        var field2:System.Reflection.FieldInfo = ANType.GetField("animationFrames");
        var field3:System.Reflection.FieldInfo = ANType.GetField("baking");
        
        field2.SetValue(ANField,null);
        field3.SetValue(ANField,true);
        field1.SetValue(param,ANField);
       // param.animatedLightmaps.animationFrames =null;
	   //	param.animatedLightmaps.baking = true;
#if UNITY_EDITOR

        //Debug.Log(clip.length);
        // animatedLight.animation.clip = clip;
        var callerfield:System.Reflection.FieldInfo = paramType.GetField("caller");
        var caller:System.Object = callerfield.GetValue(param);
        var callerType:System.Type = caller.GetType();
        
        var method:System.Reflection.MethodInfo = callerType.GetMethod("SaveCurrentLightmappingforAnimation");
		var objs:System.Object[] = new System.Object[1];
		objs[0] = ANField;
		if(caller!=null && method!=null)
		method.Invoke(caller,objs);
		//callerfield.SetValue(param,caller);
		
       // param.caller.SaveCurrentLightmappingforAnimation(param.animatedLightmaps); 
        //AnimationUtility.StartAnimationMode(null);
         var field4:System.Reflection.FieldInfo = ANType.GetField("sampleValue");
         var field5:System.Reflection.FieldInfo = ANType.GetField("clip");
         var field6:System.Reflection.FieldInfo = ANType.GetField("animatedLight");
         var val:float = field4.GetValue(ANField);
         var clip:AnimationClip = field5.GetValue(ANField) as AnimationClip;
         var animatedLight:GameObject = field6.GetValue(ANField) as GameObject;
         
        while ( val< clip.length)
        {

            //sampleValue = Mathf.Round(sampleValue);
           
           clip.SampleAnimation(animatedLight,val);
          //  animatedLight.animation[clip.name].enabled = true;
          //  animatedLight.animation.Sample();
          //  animatedLight.animation[clip.name].enabled = false;
          Sample = val;
          
        var method2:System.Reflection.MethodInfo = callerType.GetMethod("SkipBakingFrame");
		var objs2:System.Object[] = new System.Object[2];
		objs2[0] = ANField;
		objs2[1] = Sample;
		var result:boolean = method2.Invoke(caller,objs2);
          if( !result)
            Lightmapping.BakeAsync();
            
            Debug.Log("Currently recording at :"+val.ToString()+" sec, Please wait...");
          
            while (Lightmapping.isRunning)
            {
                yield;
            }
        var lockAtlasMethod:System.Reflection.MethodInfo = callerType.GetMethod("lockAtlas");
		var atlasobjs:System.Object[] = new System.Object[1];
		atlasobjs[0] = true;
		if(caller!=null && lockAtlasMethod!=null)
		lockAtlasMethod.Invoke(caller,atlasobjs);
		
		var method4:System.Reflection.MethodInfo = callerType.GetMethod("RefreshAssetDatabase");
		var objs4:System.Object[] = new System.Object[0];
		method4.Invoke(caller,objs4);
		
        var method3:System.Reflection.MethodInfo = callerType.GetMethod("CopyLightmapsToDirectoryforAnimation");
		var objs3:System.Object[] = new System.Object[2];
		objs3[0] = ANField;
		objs3[1] = Sample;
	
		method3.Invoke(caller,objs3);
		
           // param.caller.CopyLightmapsToDirectoryforAnimation(param.animatedLightmaps,param.animatedLightmaps.sampleValue);
       
       var field7:System.Reflection.FieldInfo = callerType.GetField("RecordRate");
        var rate:float = field7.GetValue(caller); 
        val = val+rate;       
       //field4.SetValue(ANField,val+rate);
           // param.animatedLightmaps.sampleValue += param.caller.RecordRate;
           // var multi:int = param.animatedLightmaps.sampleValue.ToString().Split(new char[]{'.'})[0].length;
            val = (Mathf.Round(val * 100) / 100);
            field4.SetValue(ANField,val);
           // param.animatedLightmaps.sampleValue= Mathf.Round(param.animatedLightmaps.sampleValue * 100) / 100;
           Debug.Log(val);
           field1.SetValue(param,ANField);
           val = field4.GetValue(ANField);
             Sample =val;
            
        }
        
        //AnimationUtility.StopAnimationMode();
            baking = false;
            var field8:System.Reflection.FieldInfo = ANType.GetField("baking");
			field8.SetValue(ANField,false);
            DestroyImmediate(this);
#endif
    }
 
 
 }
 
 class RecordAnimatedLMParam
 {
    public var animatedLightmaps:System.Object;
	public var caller:System.Object;
 }