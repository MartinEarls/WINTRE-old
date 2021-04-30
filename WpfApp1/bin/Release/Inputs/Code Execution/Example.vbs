Dim WSHShell, currentDirectory
Set WSHShell = CreateObject("WScript.Shell")  
currentDirectory = WSHShell.CurrentDirectory
WSHShell.Run currentDirectory & "\\Inputs\\CODEEX~1\\Example.exe"
Set WSHShell = Nothing    
WScript.Quit