using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;

namespace AssetBundleFramework
{
    public interface ISetList
    {
        void setList<T>(List<T> gameObjectList);
    }

    public class AssetbundleLoader
    {
        public static string ROOT_PATH = "";

        private const string MANIFEST_SUFFIX = ".manifest";
        private static AssetBundleManifest _manifest;

        static AssetbundleLoader()
        {
#if UNITY_EDITOR
            ROOT_PATH = Application.streamingAssetsPath;
#elif UNITY_STANDALONE
		ROOT_PATH = Application.streamingAssetsPath;
#elif UNITY_IPHONE
		ROOT_PATH = string.Format("{0}/{1}",Application.dataPath,"Raw");
#elif UNITY_ANDROID
		ROOT_PATH = Application.streamingAssetsPath;
#endif
        }

        private static Dictionary<string, AssetBundle> _assetbundleDic = new Dictionary<string, AssetBundle>();

        public static AssetBundle LoadAssetBundleDependcy(string path)
        {
            if (_manifest == null)
            {
                LoadManifest();
            }
            if (_manifest != null)
            {
                string[] dependencies = _manifest.GetAllDependencies(path);
                if (dependencies.Length > 0)
                {
                    //load all dependencies
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        //Debug.Log(" load dependencies " + dependencies[i]);
                        LoadAssetBundle(dependencies[i]);
                    }

                }
                //load self
                return LoadAssetBundle(path);
            }
            return null;
        }

        static AssetBundle LoadAssetBundle(string path)
        {
            //all characters in assetbundle are lower characters
            path = path.ToLower();
            AssetBundle bundle = null;
            //cache bundles to ignore load the same bundle
            _assetbundleDic.TryGetValue(path, out bundle);
            if (bundle != null)
            {
                return bundle;
            }

            else
            {
                string bundlePath = string.Format("{0}/{1}", ROOT_PATH, path);
                bundle = AssetBundle.LoadFromFile(bundlePath);
            }
            _assetbundleDic[path] = bundle;
            return bundle;
        }

        public static T LoadRes<T>(string path) where T : Object
        {
            AssetBundle bundle = LoadAssetBundleDependcy(path);
            if (bundle != null)
            {
                int assetNameStart = path.LastIndexOf("/") + 1;
                int assetNameEnd = path.LastIndexOf(".");
                string assetName = path.Substring(assetNameStart, assetNameEnd - assetNameStart);
                T obj = bundle.LoadAsset(assetName) as T;
                return obj;
            }
            return null;
        }

        public static IEnumerator LoadResOfType<T>(string path, ISetList comp) where T : Object
        {
            AssetBundle bundle = LoadAssetBundleDependcy(path);
            List<T> TList = new List<T>();
            if (bundle != null)
            {
                int assetNameStart = path.LastIndexOf("/") + 1;
                int assetNameEnd = path.LastIndexOf(".") == -1 ? path.Length : path.LastIndexOf(".");
                string assetName = path.Substring(assetNameStart, assetNameEnd - assetNameStart);
                Debug.Log(assetName);
                Object[] objList = bundle.LoadAllAssets(typeof(T));

                for (int i = 0; i < objList.Length; ++i)
                {
                    TList.Add(objList[i] as T);
                }
            }
            yield return null;
            comp.setList<T>(TList);

        }

        static void LoadManifest()
        {
            string assetbundleFile = string.Format("{0}/{1}", ROOT_PATH, "AssetBundles");

            AssetBundle bundle = AssetBundle.LoadFromFile(assetbundleFile);
            UnityEngine.Object obj = bundle.LoadAsset("AssetBundleManifest");
            bundle.Unload(false);
            _manifest = obj as AssetBundleManifest;
            Debug.Log("Load Manifest Finished");
        }
    }
}