 #pragma strict

import System.Collections.Generic;
#if UNITY_EDITOR
import System.IO;
#endif
import System;

 @script ExecuteInEditMode
 class BakeSelection extends MonoBehaviour
 {
 
 var  cachedObjects:ArrayList;

 public  function BakeSelectedObjects(objs:System.Object[],caller:System.Object)
    {
        var param:SelectionBakingParam = new SelectionBakingParam();
        param.objs = objs;
		param.caller = caller;
		
        StartCoroutine("Bake", param);
      // while(Lightmapping.isRunning)
      //      yield return null;  

    }
	
	  function Bake(param:SelectionBakingParam)
    {
    #if UNITY_EDITOR
    
    var initial:LightmapData[] =  new LightmapData[LightmapSettings.lightmaps.Length];

				
				for(var i:int =0;i<LightmapSettings.lightmaps.Length;i++)
				{
					initial[i] = new LightmapData();
					initial[i].lightmapLight = LightmapSettings.lightmaps[i].lightmapLight;
					initial[i].lightmapDir = LightmapSettings.lightmaps[i].lightmapDir;
			}
			var renderers:MeshRenderer[] = GameObject.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
			var flags:StaticEditorFlags[] = new StaticEditorFlags[renderers.Length];
			var originalScales:Vector4[] = new Vector4[renderers.Length];
			var scaleInLightmap:float[] = new float[renderers.Length];
			
		var selectedRenderers:ArrayList = new ArrayList(Selection.GetFiltered(typeof(MeshRenderer),SelectionMode.Deep));
		var originalSelectedAtals:Vector4[] = new Vector4[selectedRenderers.Count];
		
				for(i=0;i<renderers.Length;i++)
				{
					originalScales[i] = renderers[i].lightmapScaleOffset;
					var so : SerializedObject = new SerializedObject(renderers[i]);
       				scaleInLightmap[i] =  so.FindProperty("m_ScaleInLightmap").floatValue;
       				flags[i] = GameObjectUtility.GetStaticEditorFlags(renderers[i].gameObject);
     
					if(!Selection.Contains(renderers[i].gameObject))
					{
					//Debug.Log(renderers[i]);
					//	renderers[i].lightmapTilingOffset = new Vector4(0,0,renderers[i].lightmapTilingOffset.z,renderers[i].lightmapTilingOffset.w);
					
       				 so.FindProperty("m_ScaleInLightmap").floatValue = 0;
        		     so.ApplyModifiedProperties();
					//Debug.Log(so.FindProperty("m_ScaleInLightmap").floatValue);
					}
					
				}
        cachedObjects = new ArrayList();
		
		var callerType:Type = param.caller.GetType();
		var method:MethodInfo = callerType.GetMethod("StartAsyncBaking");
		var objs:Object[] = new Object[0];
		//objs[0] = null;
		method.Invoke(param.caller,objs);
         //param.caller.StartAsyncBaking();
		 //param.caller.isLightmappingRunning();
		var method2:MethodInfo = callerType.GetMethod("isLightmappingRunning");
       var check:boolean = method2.Invoke(param.caller,objs);
         while (check)
         {
            yield;
            check =  method2.Invoke(param.caller,objs);
          }  
            if(param.objs!=null && param.objs.length>0)
            {
            var types:Type[] = new Type[2];
            types[0]=typeof(UnityEngine.Object[]);
            types[1]=typeof(ArrayList);
           	 var method3:MethodInfo = callerType.GetMethod("CreatLightmappedObjects",types);
		     var objs3:Object[] = new Object[2];
			 objs3[0] = param.objs;
			 objs3[1] = cachedObjects;
            
            method3.Invoke(param.caller,objs3);
        	//	param.caller.CreatLightmappedObjects(param.objs,cachedObjects);
			}  
			for(var x:int;x<selectedRenderers.Count;x++)
			{
			var mr:MeshRenderer = selectedRenderers[x] as MeshRenderer;
			
			originalSelectedAtals[x] = mr.lightmapScaleOffset;
			}
			   
           // load original lightmap
       var method4:MethodInfo = callerType.GetMethod("LoadTempLightmap");
       
       
       
        // param.caller.LoadTempLightmap();
        method4.Invoke(param.caller,objs);
        var method5:MethodInfo = callerType.GetMethod("RefreshDatabase");
       method5.Invoke(param.caller,null);
         //param.caller.RefreshDatabase();
		method4.Invoke(param.caller,objs);
        var method6:MethodInfo = callerType.GetMethod("ResetObjectPrefabState");
        var method7:MethodInfo = callerType.GetMethod("AddLightmapComponent");
         var objs4:Object[] = new Object[1];
        for(i=0;i<cachedObjects.Count;i++)
        {
			var obj = cachedObjects[i];
			
			objs4[0] = obj;
			method5.Invoke(param.caller,null);
			method6.Invoke(param.caller,objs4);
			method7.Invoke(param.caller,objs4);
			
			//param.caller.ResetObjectPrefabState(obj);
            //param.caller.AddLightmapComponent(obj);
        
        }
          for(i=0;i<renderers.Length;i++)
				{
				if(!selectedRenderers.Contains(renderers[i]))
					{
					//renderers[i].lightmapTilingOffset = originalScales[i];
					var so1 : SerializedObject = new SerializedObject(renderers[i]);
    			    so1.FindProperty("m_ScaleInLightmap").floatValue = scaleInLightmap[i];
    			    so1.ApplyModifiedProperties();
					}
					else
					{
					GameObjectUtility.SetStaticEditorFlags(renderers[i].gameObject,flags[i]);
					Debug.Log("Filtered "+renderers[i].name);
					}
				}
				
			for( x = 0;x<selectedRenderers.Count;x++)
			{
			mr = selectedRenderers[x] as MeshRenderer;
			mr.lightmapScaleOffset = originalSelectedAtals[x];
			}  
			 
				for(i =0;i<LightmapSettings.lightmaps.Length;i++)
				{
				
				if(LightmapSettings.lightmaps[i].lightmapLight == null)
				{
				Debug.Log(initial[i].lightmapLight);
				var copiedData:LightmapData[] = new LightmapData[LightmapSettings.lightmaps.Length];
				for(x =0;x<LightmapSettings.lightmaps.Length;x++)
				{
			
				copiedData[x]= new LightmapData();
				copiedData[x].lightmapLight =LightmapSettings.lightmaps[x].lightmapLight;
				copiedData[x].lightmapDir =LightmapSettings.lightmaps[x].lightmapDir;
				}
				if(initial[i].lightmapLight!=null)
				copiedData[i].lightmapLight = initial[i].lightmapLight;
				if(initial[i].lightmapDir!=null)
				copiedData[i].lightmapDir = initial[i].lightmapDir;
				
				LightmapSettings.lightmaps = copiedData;
				
				}
			//	Debug.Log(LightmapSettings.lightmaps[i].lightmapFar);
			//	Debug.Log(LightmapSettings.lightmaps[i].lightmapNear);
				}
		#endif
		
    }

}
 class SelectionBakingParam
{
    public var objs:System.Object[];
	public var caller:System.Object;

}