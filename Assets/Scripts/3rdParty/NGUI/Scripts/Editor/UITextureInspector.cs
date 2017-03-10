//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UITextures.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UITexture), true)]
public class UITextureInspector : UIBasicSpriteEditor
{
	UITexture mTex;

	protected override void OnEnable ()
	{
		base.OnEnable();
		mTex = target as UITexture;
	}

	protected override bool ShouldDrawProperties ()
	{
		if (target == null) return false;
		if (mTex.material != null || mTex.mainTexture == null)
		{
			Material mat = EditorGUILayout.ObjectField("Material", mTex.material, typeof(Material), false) as Material;
			
			if (mTex.material != mat)
			{
				NGUIEditorTools.RegisterUndo("Material Selection", mTex);
				mTex.material = mat;
			}
		}
		
		if (mTex.material == null || mTex.hasDynamicMaterial)
		{
			Shader shader = EditorGUILayout.ObjectField("Shader", mTex.shader, typeof(Shader), false) as Shader;
			
			if (mTex.shader != shader)
			{
				NGUIEditorTools.RegisterUndo("Shader Selection", mTex);
				mTex.shader = shader;
			}

			NGUIEditorTools.DrawProperty("Texture", serializedObject, "mTexture");
			//Texture tex = EditorGUILayout.ObjectField("Texture", mTex.mainTexture, typeof(Texture), false) as Texture;
			
			/*if (mTex.mainTexture != tex)
			{
				NGUIEditorTools.RegisterUndo("Texture Selection", mTex);
				mTex.mainTexture = tex;
			}*/
			
			bool enable = EditorGUILayout.Toggle("Enable", mTex.isEnabled);	
			
			if (mTex.isEnabled != enable)
			{
				NGUIEditorTools.RegisterUndo("Texture Selection", mTex);	
				mTex.isEnabled = enable;
			}
		}
		
		if (mTex.mainTexture != null)
		{
			Rect rect = EditorGUILayout.RectField("UV Rectangle", mTex.uvRect);
			
			if (rect != mTex.uvRect)
			{
				NGUIEditorTools.RegisterUndo("UV Rectangle Change", mTex);
				mTex.uvRect = rect;
			}
		}
		return (mWidget.material != null);
	}

	/// <summary>
	/// Allow the texture to be previewed.
	/// </summary>

	public override bool HasPreviewGUI ()
	{
		return (Selection.activeGameObject == null || Selection.gameObjects.Length == 1) &&
			(mTex != null) && (mTex.mainTexture as Texture2D != null);
	}

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		Texture2D tex = mTex.mainTexture as Texture2D;

		if (tex != null)
		{
			Rect tc = mTex.uvRect;
			tc.xMin *= tex.width;
			tc.xMax *= tex.width;
			tc.yMin *= tex.height;
			tc.yMax *= tex.height;
			NGUIEditorTools.DrawSprite(tex, rect, mTex.color, tc, mTex.border);
		}
	}
}
