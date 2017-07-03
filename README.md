[![GitHub release](https://img.shields.io/github/release/TimeWalkOrg/external-package-manager.svg)]()
# External Package Manager (EPM)
A simple asset package manager for Unity3d that is integrated in the editor and requires no external dependencies or runtime.

# Installing
1. Download latest release
1. Open Unity Project
1. `Assets -> Import Package -> Custom Package ...`
1. Select the downloaded EPM release package
1. You should now have EPM commands under `Assets -> External Package Manager`

# Unity Cloud

It is possible specify custom commands as part of a cloud build, and thus we could trigger an EPM import. However, importing external packages that contain script dependencies won't work properly since the packages would be imported *after* the project is compiled.
