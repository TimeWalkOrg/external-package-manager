# External Package Manager
A simple asset package manager for Unity3d that is integrated in the editor and requires no external dependencies or runtime.

# Unity Cloud
Importing external packages as part of a Unity Cloud build will only work for packages that don't contain script dependencies, since the packages would be imported *after* the project is compiled.
