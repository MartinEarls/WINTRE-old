Welcome to WINTRE (Windows-MITRE) Adversary Simulation Tool! Please download the latest release to run the tool. This tool was made as part of my final year project for my honours degree (Cyber Crime & IT Security). The tool won best project in its category (amongst all other Cyber Crime & IT Security projects) and is meant to provided a series of tests you can execute in your environment to test your anti-virus solution.

# Requirements
- WINTRE is built using the .NET framework, all editions of Windows 10 include the .NET framework 4.6 installed by default. If you wish to run the application on an older OS (Windows 7), 
please ensure .NET Framework v4.0 is installed.

- Visual Studio Compiler/C++ Support - Note, C++ support is built into the application assuming the VS component is installed for compilation but there is 
currently only one C++ technique, meaning if you don't have C++ support installed the application will still execute C# and command line techniques and warn you if the compiler is not
found for any given C++ technique.

- Microsoft Word is used for the reporting features, please make sure Microsoft Office is installed if you wish to utilise the reporting feature.

# Usage Instructions
Please refer to the User Manual for usage instructions. Be advised this tool may cause anti-virus alerts. https://showcase.itcarlow.ie/C00227207/index.html

# DISCLAIMER
You are responsible for what you do with this tool, it should only be used to perform authorized testing for which you have explicit permission to do so. As it stands the tool does not facilitate external C2 but may still damage your files (impact testing) or trigger unintended anti-virus alerts.

# WINTRE 2.0
Please note I'm currently in the process of re-making this tool to be suitable for modern enterprise environments and performing detection testing/purple teaming against EDR/DLP security controls. Due to this it is unlikely I'll continue to update this version of the tool as the re-make will be substantially different in terms of architecture, design and capabilities.
