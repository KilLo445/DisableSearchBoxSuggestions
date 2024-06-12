using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace DisableSearchBoxSuggestions
{
    internal class Program
    {
        public static string version = "1.0";
        public static string githubLink = "https://github.com/KilLo445/DisableSearchBoxSuggestions";

        public static string asciiArt = $@"                  
    ,---,      .--.--.       ,---,.   .--.--.    
  .'  .' `\   /  /    '.   ,'  .'  \ /  /    '.
,---.'     \ |  :  /`. / ,---.' .' ||  :  /`. /  
|   |  .`\  |;  |  |--`  |   |  |: |;  |  |--`   
:   : |  '  ||  :  ;_    :   :  :  /|  :  ;_     
|   ' '  ;  : \  \    `. :   |    ;  \  \    `.  
'   | ;  .  |  `----.   \|   :     \  `----.   \ 
|   | :  |  '  __ \  \  ||   |   . |  __ \  \  | 
'   : | /  ;  /  /`--'  /'   :  '; | /  /`--'  / 
|   | '` ,/  '--'.     / |   |  | ; '--'.     /  
;   :  .'      `--'---'  |   :   /    `--'---'   
|   ,.'                  |   | ,'                
'---'                    `----'                  

        Disable Search Box Suggestions
                    v{version}

                  by KilLo
             github.com/KilLo445";

        public static bool ExplorerRunning = true;

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void Main(string[] args)
        {
            Console.Title = "Disable Search Box Suggestions by KilLo";

            Console.WriteLine(asciiArt);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine();
            Console.WriteLine("[1] Disable Search Box Suggestions");
            Console.WriteLine("[2] Enable Search Box Suggestions");
            Console.WriteLine("[3] About DSBS");
            Console.WriteLine("[4] Open DSBS GitHub");
            Console.WriteLine();
            Console.Write("Enter choice: ");
            string userInput = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            if (userInput == "1") { DSBS(); PressAnyKeyToExit(); }
            if (userInput == "2") { ESBS(); PressAnyKeyToExit(); }
            if (userInput == "3") { AboutDSBS(); PressAnyKeyToExit(); }
            if (userInput == "4") { Console.WriteLine("Opening GitHub link..."); Process.Start(githubLink); }
            if (userInput != "1" || userInput != "2" || userInput != "3" || userInput != "4") { Console.WriteLine("Unknown command..."); PressAnyKeyToExit(); }
            PressAnyKeyToExit();
        }

        public static void DSBS()
        {
            Console.WriteLine("Checking for permission...");
            Thread.Sleep(100);
            if (IsAdministrator()) { Console.WriteLine("Permission granted!"); }
            else { Console.WriteLine("No permission, please run as administrator and try again!"); PressAnyKeyToExit(); }
            Thread.Sleep(100);
            Console.WriteLine();
            Console.WriteLine("Checking for registry key HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Explorer...");
            Thread.Sleep(100);
            RegistryKey keyExp = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer", true);
            if (keyExp == null)
            {
                Console.WriteLine("Key does not exist! Creating it...");
                Thread.Sleep(100);
                try
                {
                    RegistryKey keyWin = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer");
                    Console.WriteLine("Key created! Closing registry...");
                    keyWin.Close();
                }
                catch (Exception e) { ExceptionMSG(e); }
            }
            else { Console.WriteLine("Key found!"); }
            try
            {
                Thread.Sleep(100);
                Console.WriteLine("Creating DWORD value DisableSearchBoxSuggestions...");
                keyExp.SetValue("DisableSearchBoxSuggestions", "1", RegistryValueKind.DWord);
                Thread.Sleep(100);
                Console.WriteLine("Value created!");
            }
            catch (Exception e) { ExceptionMSG(e); }
            Thread.Sleep(100);
            Console.WriteLine("Closing registry...");
            keyExp.Close();
            Thread.Sleep(100);
            RestartExplorer();
            return;
        }

        public static void ESBS()
        {
            Console.WriteLine("Checking for permission...");
            Thread.Sleep(100);
            if (IsAdministrator()) { Console.WriteLine("Permission granted!"); }
            else { Console.WriteLine("No permission, please run as administrator and try again!"); PressAnyKeyToExit(); }
            Console.WriteLine();
            Thread.Sleep(100);
            Console.WriteLine("Checking for registry key HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Explorer...");
            RegistryKey keyExp = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer", true);
            if (keyExp == null) { Console.WriteLine("Search box suggestions are not enabled."); }
            else { Console.WriteLine("Key found!"); }
            try
            {
                Thread.Sleep(100);
                if (Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Explorer", "DisableSearchBoxSuggestions", null) == null) { Console.WriteLine("Search box suggestions are not enabled."); }
                else
                {
                    Console.WriteLine("Deleting DWORD value DisableSearchBoxSuggestions...");
                    keyExp.DeleteValue("DisableSearchBoxSuggestions");
                    Thread.Sleep(100);
                    Console.WriteLine("Value deleted!");
                }
            }
            catch (Exception e) { ExceptionMSG(e); }
            Thread.Sleep(100);
            Console.WriteLine("Closing registry...");
            keyExp.Close();
            Thread.Sleep(100);
            RestartExplorer();
            return;
        }

        public static void RestartExplorer()
        {
            Console.WriteLine("Restarting explorer...");
            if (Process.GetProcessesByName("explorer").Length > 0)
            {
                ExplorerRunning = true;
                try
                {
                    Process[] explorer = Process.GetProcessesByName("explorer");
                    foreach (Process process in explorer) { process.Kill(); ExplorerRunning = false; }
                }
                catch (Exception e) { ExceptionMSG(e); }
            }
            Thread.Sleep(500);
            if (Process.GetProcessesByName("explorer").Length < 0)
            {
                try
                {
                    Process.Start("explorer");
                }
                catch (Exception e) { ExceptionMSG(e); }
            }
            while (!ExplorerRunning)
            {
                Console.WriteLine("Waiting for explorer...");
                if (Process.GetProcessesByName("explorer").Length > 0) { ExplorerRunning = true; }
                else { ExplorerRunning = false; }
                Thread.Sleep(500);
            }
            Thread.Sleep(100);
            if (ExplorerRunning) { Console.WriteLine("Explorer running!"); }
            return;
        }

        public static void AboutDSBS() { Console.WriteLine("DisableSearchBoxSuggestions, or DSBS is a simple tool created by KilLo (github.com/KilLo445) that disables the annoying web-searches and Copilot in the Windows 11 Start Menu, allowing you to only search for files and run commands."); }

        public static void ExceptionMSG(Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("An error occured...");
            Console.WriteLine($"{ex}");
            PressAnyKeyToExit();
        }

        public static void PressAnyKeyToExit()
        {
            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
