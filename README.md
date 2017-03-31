# Compact Framework
A set of APIs for Windows Mobile and Windows Embedded devices for .NET Compact Framework 3.5 and later.

Originally released as a set of separate products for the .NET Compact Framework these libraries have been updated over time and converted from v2.0 to v3.5 of the runtime. They have been tested on a variety of platforms from Windows Mobile (including Windows Embedded Handheld 6.5) and other variants of Windows Embedded Compact (or CE as it is still more commonly known). Some features require OS specific functionality - for example Connection Manager was originally Windows Mobile specific but was later added to the core CE 6.0 and later versions. Check with the documentation to see which classes depend on particular OS components. 

The source projects are currently Visual Studio 2008 .NETCF 3.5 projects. Much of the code will compile equally on .NETCF 2.0. We're looking at adding Visual Studio 2013 projects to support Compact 2013 but you can build these yourself currently with the source. The existing binaries should work without modification on Compact 2013.

The libraries were designed and developed by In The Hand Ltd. They have been made available to support ongoing development on Windows Embedded Compact and support for legacy Windows Mobile / Windows Embedded Handheld platforms.
