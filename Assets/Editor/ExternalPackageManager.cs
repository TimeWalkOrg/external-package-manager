using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;

public class ExternalPackageManager
{
    public static ExportPackageOptions exportOptions = ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse;

    protected static PackagesJson packagesJson;

    private static string externalAssetDirectory = Path.Combine("Assets", "External");
    private static string externalAssetDirectoryPath = Path.Combine(Application.dataPath, "External");
    
    [MenuItem("Assets/External Package Manager/Export All")]
    static void ExportAll()
    {
        Debug.Log("ExportAll");
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
                string packageFolderPath = Path.Combine(externalAssetDirectoryPath, packageFolder);
                ArrayList exportedPackages = new ArrayList();

                if (Directory.Exists(packageFolderPath))
                {
                    string packageBuildName = packageFolder + "-v" + packagesJson.version + ".unitypackage";
                    string packageBuildPath = Path.GetFullPath(Path.Combine(buildFolderPath, packageBuildName));
                    
                    Debug.Log("Exporting assets from " + externalAssetDirectoryPath + " to package " + packageBuildPath);
                    AssetDatabase.ExportPackage(externalAssetDirectory, packageBuildPath, exportOptions);

                    exportedPackages.Add(packageBuildPath);
                } else
                {
                    EditorUtility.DisplayDialog("External Package Manager",
                        "Unable to find package folder (" + packageFolderPath + ") for export.",
                        "OK");
                }

                if(exportedPackages.Count > 0)
                {   
                    EditorUtility.DisplayDialog("External Package Manager",
                        "Exported the following packages: " + 
                        string.Join("," + System.Environment.NewLine, 
                            (string[])exportedPackages.ToArray(Type.GetType("System.String"))),
                        "OK");
                }
                
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        
    }

    [MenuItem("Assets/External Package Manager/Import All")]
    static void ImportAll()
    {
        // Download and import all packages from package.json
        Debug.Log("ImportAll");
        parsePackagesJson();

        // TODO
    }

    private static void parsePackagesJson()
    {
        string packagesPath = Path.Combine(Application.dataPath, "packages.json");

        Debug.Log("Loading packages file: " + packagesPath);

        try
        {
            if (File.Exists(packagesPath))
            {
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

    protected class PackagesJson
    {
        public string version;
        public string buildFolder = Path.Combine("Builds", "Packages"); // relative to project root
        public List<string> exports = new List<string>();
        public List<string> dependencies = new List<string>();
    }
}