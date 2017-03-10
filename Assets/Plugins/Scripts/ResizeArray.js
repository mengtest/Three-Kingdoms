 #pragma strict
 
class ResizeArray{

static function Add(key:int,obj:Object)
{
var LMArray : LightmapData[]  = LightmapSettings.lightmaps;
var newarr = new Array (LMArray);
newarr[key] = obj;

 var builtinArray : LightmapData[] = new LightmapData[newarr.length];
 for(var i:int;i<newarr.length;i++)
 {
var item:LightmapData = newarr[i] as LightmapData;
 builtinArray[i] = item;
}
LightmapSettings.lightmaps = builtinArray;
}
}