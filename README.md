[![GitHub release](https://img.shields.io/github/release/TimeWalkOrg/external-package-manager.svg)]()
# External Package Manager (EPM)
A simple asset package manager for Unity3d that is integrated in the editor and requires no external dependencies or runtime. EPM allows a project to specify dependencies on remotely hosted [Unity Asset Packages](https://docs.unity3d.com/Manual/AssetPackages.html) in a JSON config file (`Assets/packages.json`). This makes it easier to split a large project into multiple reusable projects with minimal setup, and allows open-source Unity projects to depend on third-party assets without making them part of the project source code.

EPM also supports bulk-exporting packages and package upload to S3.

# Installing
1. [Download latest release](https://github.com/TimeWalkOrg/external-package-manager/releases)
1. Open Unity Project
1. `Assets -> Import Package -> Custom Package ...`
1. Select the downloaded EPM release package
1. You should now have EPM commands under `Assets -> External Package Manager`

# Commands

## Import All

Download and import all external dependencies specified by "Dependencies" field in `packages.json`.

## Export All

Create packages for all top-level asset folders in `Assets/External` and upload to S3 (coming soon). Requiring exported asset folders to be located in the same directory ensures that imported assets will be located in a single a single directory when imported. 

# `packages.json`
## Fields
* version
** Version string to include in exported package name.
* exports
** Array of asset folders 
* dependencies

# Tips

## `.gitignore`

Add the following to your `.gitignore` file to ensure external assets aren't checked into your project's source.

    # Ignore External assets
    /Assets/External*
    /Assets/External/*

If your project depends on external packages *and* exports packages, an exception for one or the other will need to be specified in the `.gitignore`. 
    # Ignore External assets
    /Assets/External*
    /Assets/External/*
    # Except this one
    !/Assets/External/foo
    


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


# Unity Cloud

It is possible specify custom commands as part of a cloud build, and thus we could trigger an EPM import. However, importing external packages that contain script dependencies won't work properly since the packages would be imported *after* the project is compiled.

See [AssetDatabase.ImportPackage in Pre-Export Method](https://forum.unity3d.com/threads/assetdatabase-importpackage-in-pre-export-method.418468/).

