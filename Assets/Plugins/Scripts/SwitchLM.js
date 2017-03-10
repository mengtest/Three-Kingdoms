 #pragma strict
public class SwitchLM{

static function switchmaps(far:int,near:int,index:int,SwitchTexturesFar:Texture2D[],SwitchTexturesNear:Texture2D[])
{
var LMArray : LightmapData[]  = LightmapSettings.lightmaps;
var newarr = new Array (LMArray);
var caster:LightmapData;
if (far != -1 && far<newarr.length && far<SwitchTexturesFar.length)
           {
           caster = newarr[index] as LightmapData;
            caster.lightmapLight = SwitchTexturesFar[far];
           }
if (near != -1&& near<newarr.length && near<SwitchTexturesFar.length)
           {
            caster = newarr[index] as LightmapData;
            caster.lightmapDir = SwitchTexturesNear[near];           
}
//var builtinArray : LightmapData[] =newarr.ToBuiltin(LightmapData); 
var builtinArray : LightmapData[] = new LightmapData[newarr.length];
 for(var i:int;i<newarr.length;i++)
 {
var item:LightmapData = newarr[i] as LightmapData;
 builtinArray[i] = item;
}
LightmapSettings.lightmaps = builtinArray;
}
static function switchmaps(index:int,Far:Texture2D,Near:Texture2D)
{

var LMArray : LightmapData[]  = LightmapSettings.lightmaps;
var newarr= new Array (LMArray);
var caster:LightmapData;
if (Far != null  && index<newarr.length)
          {
           caster = newarr[index] as LightmapData;
            caster.lightmapLight = Far;
           }
if (Near !=null  && index<newarr.length)
           { 
            caster = newarr[index] as LightmapData;
           caster.lightmapDir = Near;           
           }

//var builtinArray : LightmapData[] =newarr.ToBuiltin(LightmapData); 
var builtinArray : LightmapData[] = new LightmapData[newarr.length];
 for(var i:int;i<newarr.length;i++)
 {
var item:LightmapData = newarr[i] as LightmapData;
 builtinArray[i] = item;
}
LightmapSettings.lightmaps = builtinArray;
}
}