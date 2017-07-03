using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class ExternalPackageManager
{
    private static string externalAssetDirectoryPath = Path.Combine(Application.dataPath, "External");
    public static ExportPackageOptions exportOptions = ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse;

    protected static PackagesJson packagesJson;

    protected class PackagesJson
    {
        public int version;
        public string buildFolder = Path.Combine("Builds", "Packages"); // relative to project root
        public List<string> exports = new List<string>();
        public List<string> dependencies = new List<string>();
    }
    
    [MenuItem("Assets/External Package Manager/Export")]
    static void Export()
    {
        Debug.Log("Export");
        parsePackagesJson();
        try
        {
            // Make sure build folder exists at project root (inferred from assets folder)
            string buildFolderPath = 
                Path.Combine(Application.dataPath, Path.Combine("..", packagesJson.buildFolder));
            if (!Directory.Exists(buildFolderPath))
            {
                Directory.CreateDirectory(buildFolderPath);
            }

            // Export a package per export directory defined in packages.json
            foreach (string packageFolder in packagesJson.exports)
            {
                string fileName = packageFolder + "-v" + packagesJson.version + ".unitypackage";
                string packageFolderPath = Path.Combine(externalAssetDirectoryPath, packageFolder);
                if (Directory.Exists(packageFolderPath))
                {
                    AssetDatabase.ExportPackage(packageFolderPath, 
                        Path.Combine(buildFolderPath, fileName), exportOptions);
                } else
                {
                    EditorUtility.DisplayDialog("External Package Manager",
                        "Unable to find package folder (" + packageFolderPath + ") for export.",
                        "OK");
                }
                
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        
    }

    [MenuItem("Assets/External Package Manager/Install")]
    static void Install()
    {
        // Download and import all packages from package.json
        Debug.Log("Install");
        parsePackagesJson();
    }

    [MenuItem("Assets/External Package Manager/Delete")]
    static void Delete()
    {
        // Delete all packages in ./Assets/External
        Debug.Log("Delete");
        parsePackagesJson();
    }


    private static void parsePackagesJson()
    {
        string packagesPath = Path.Combine(Application.dataPath, "packages.json");

        Debug.Log("Loading packages file: " + packagesPath);

        try
        {
            if (File.Exists(packagesPath))
            {
                // TODO try/catch
                string dataAsJson = File.ReadAllText(packagesPath);
                packagesJson = JsonUtility.FromJson<PackagesJson>(dataAsJson);
            }
            else
            {
                EditorUtility.DisplayDialog("External Package Manager",
                    "Unable to find " + packagesPath,
                    "OK");
            }
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}