using UnityEditor;
using UnityEngine;

public class AssetBundleBuild
{

	[MenuItem("Assets/Build All Bundles WebGL")]	
	static public void BuildAllBundlesWebGL() 
	{
		BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.UncompressedAssetBundle;
		//BuildPipeline.BuildAssetBundles(DirectoryUtility.ExternalAssets(), assetBundleOptions, BuildTarget.StandaloneOSXIntel);
		BuildPipeline.BuildAssetBundles(DirectoryUtility.ExternalAssets(), assetBundleOptions, BuildTarget.WebGL);

	}

	[MenuItem("Assets/Build All Bundles Linux")]	
	static public void BuildAllBundlesLinux() 
	{
		BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.UncompressedAssetBundle;
		BuildPipeline.BuildAssetBundles(DirectoryUtility.ExternalAssets(), assetBundleOptions, BuildTarget.StandaloneLinuxUniversal);

	}

	[MenuItem("Assets/Build All Bundles OSX")]	
	static public void BuildAllBundlesOSX() 
	{
		BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.UncompressedAssetBundle;
		BuildPipeline.BuildAssetBundles(DirectoryUtility.ExternalAssets(), assetBundleOptions, BuildTarget.StandaloneOSXIntel);

	}

	[MenuItem("Assets/Build All Bundles Android")]	
	static public void BuildAllBundlesAndroid() 
	{
		BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.UncompressedAssetBundle;
		//BuildPipeline.BuildAssetBundles(DirectoryUtility.ExternalAssets(), assetBundleOptions, BuildTarget.StandaloneOSXIntel);
		BuildPipeline.BuildAssetBundles(DirectoryUtility.ExternalAssets(), assetBundleOptions, BuildTarget.Android);

	}
	
}
