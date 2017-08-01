[![GitHub release](https://img.shields.io/github/release/TimeWalkOrg/external-package-manager.svg)]()
# External Package Manager (EPM)
EPM is a simple asset package manager for Unity3d that is integrated in the editor and requires no external dependencies or runtime. EPM allows a project to specify dependencies on remotely hosted [Unity Asset Packages](https://docs.unity3d.com/Manual/AssetPackages.html) in a JSON config file (`Assets/packages.json`). This makes it easier to split a large project into multiple reusable projects with minimal setup, and allows open-source Unity projects to depend on third-party assets without making them part of the project source code.

EPM also supports bulk-exporting of packages and (eventually) package upload to S3.

EPM editor script is here: [external-package-manager/Assets/Editor/ExternalPackageManager.cs](https://github.com/TimeWalkOrg/external-package-manager/blob/master/Assets/Editor/ExternalPackageManager.cs)

# Installation and Setup
1. [Download latest release](https://github.com/TimeWalkOrg/external-package-manager/releases)
1. Open your Unity Project
1. `Assets -> Import Package -> Custom Package ...`
1. Select the downloaded EPM release package
1. You should now have EPM commands under `Assets -> External Package Manager`
1. Modify `Assets/packages.json` to specify the remote URLs of asset packages you wish to depend on, and/or the asset folder names for the packages you wish to export.

# Commands

## Import All

Download and import all external dependencies specified by `dependencies` field in `packages.json`.

## Export All

Create packages for all folders specified in `packages.json`. Folders must live inside `Assets/External`, which ensures that assets will be located in a single directory when imported. All built packages can be found in `Builds/Packages`.

EPM builds packages with [IncludeDependencies](https://docs.unity3d.com/ScriptReference/ExportPackageOptions.IncludeDependencies.html) and [Recurse](https://docs.unity3d.com/ScriptReference/ExportPackageOptions.Recurse.html) options enabled.

# `packages.json`

## Fields
* `version`
  * Version string to include in exported package name.
* `exports`
  * Array of asset folders that should be exported with `Export All` command.
* `dependencies`
  * Array of remotely hosted Unity asset packages to be downloaded and imported with `Import All` command.


## Example
    {
      "version": "1",
      "exports": [
        "foo"
      ],
      "dependencies": [
        "https://www.dropbox.com/s/n9969so8bh57jms/sphere-v1.unitypackage?dl=1",
        "https://www.dropbox.com/s/8j0mupy3ugcnnx2/cylinder-v1.unitypackage?dl=1"
      ]
    }

* Invoking `Import All` would result in all the assets contained in the `sphere` and `cylinder` packages being imported into `Assets/External/`.
* Invoking `Export All` would create `/Builds/packages/foo-v1.unitypackage`, containing all assets under `Assets/External/foo`.

# Tips

## `.gitignore`

Add the following to your `.gitignore` file to ensure package builds and external assets aren't checked into your project's source.

    # EPM
    /Builds/Packages
    /Assets/External.meta*
    /Assets/External/*

If your project depends on external packages *and* exports packages, an exception for one or the other will need to be specified in the `.gitignore`. 
    
    # EPM
    /Builds/Packages
    /Assets/External.meta*
    /Assets/External/*
    # Except this one
    !/Assets/External/foo

## Unity Cloud

It is possible to specify custom commands as part of a cloud build, and thus we could trigger an EPM import. However, importing external packages that contain script dependencies won't work properly since the packages would be imported *after* the project is compiled.

See [AssetDatabase.ImportPackage in Pre-Export Method](https://forum.unity3d.com/threads/assetdatabase-importpackage-in-pre-export-method.418468/).

