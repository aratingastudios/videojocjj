using UnityEngine;
using UnityEditor;
using System;

//Sets our settings for all new Models and Textures upon first import
public class CustomImportSettings : AssetPostprocessor 
{
	//public const float globalScaleModifier = 0.0028f;
	
	void OnPreprocessAudio()
	{
		AudioImporter audioImporter = assetImporter as AudioImporter;
		audioImporter.threeD = false;
		audioImporter.format = AudioImporterFormat.Compressed;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnPreprocessTexture() 
	{
		TextureImporter importer = assetImporter as TextureImporter;
		
		if(importer.assetPath.Contains("Textures"))
		{
			importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			importer.wrapMode = TextureWrapMode.Clamp;
		} 
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	void OnPreprocessModel() 
	{
		ModelImporter importer = assetImporter as ModelImporter;

		//importer.globalScale  = globalScaleModifier;
		importer.globalScale  = 1.0f;
		importer.swapUVChannels = false;
		importer.generateSecondaryUV = true;
		importer.addCollider = true;
    }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	*/
}