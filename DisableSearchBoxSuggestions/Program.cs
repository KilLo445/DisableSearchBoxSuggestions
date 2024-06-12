using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace DisableSearchBoxSuggestions
{
    internal class Program
    {
        public static string version = "1.1";
        public static string githubLink = "https://github.com/KilLo445/DisableSearchBoxSuggestions";
        public static string changeLink = "https://github.com/KilLo445/DisableSearchBoxSuggestions/blob/main/Changelog.md";

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
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg == "-h" || arg == "-help") { Help(); PressAnyKeyToExit(); }
                if (arg == "-1" || arg == "-d" || arg == "-dsbs") { DSBS(); PressAnyKeyToExit(); }
                if (arg == "-2" || arg == "-e" || arg == "-esbs") { ESBS(); PressAnyKeyToExit(); }
                if (arg == "-3" || arg == "-a" || arg == "-about") { AboutDSBS(); PressAnyKeyToExit(); }
                if (arg == "-4" || arg == "-g" || arg == "-github") { Console.WriteLine("Opening GitHub..."); Process.Start(githubLink); PressAnyKeyToExit(); }
                if (arg == "-5" || arg == "-c" || arg == "-changes" || arg == "-changelog") { Console.WriteLine("Opening Changelog..."); Process.Start(changeLink); PressAnyKeyToExit(); }
                if (arg == "-v" || arg == "-ver" || arg == "-version") { Console.WriteLine($"You have v{version} installed"); PressAnyKeyToExit(); }
            }
            Console.WriteLine("What would you like to do?");
            Console.WriteLine();
            Console.WriteLine("[1] Disable Search Box Suggestions");
            Console.WriteLine("[2] Enable Search Box Suggestions");
            Console.WriteLine("[3] About DSBS");
            Console.WriteLine("[4] Open DSBS GitHub");
            Console.WriteLine("[5] Open DSBS Changelog");
            Console.WriteLine();
            Console.Write("Enter choice: ");
            string userInput = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            if (userInput == "1") { DSBS(); PressAnyKeyToExit(); }
            if (userInput == "2") { ESBS(); PressAnyKeyToExit(); }
            if (userInput == "3") { AboutDSBS(); PressAnyKeyToExit(); }
            if (userInput == "4") { Console.WriteLine("Opening GitHub..."); Process.Start(githubLink); PressAnyKeyToExit(); }
            if (userInput == "5") { Console.WriteLine("Opening Changelog..."); Process.Start(changeLink); PressAnyKeyToExit(); }
            if (userInput != "1" || userInput != "2" || userInput != "3" || userInput != "4" || userInput != "5") { Console.WriteLine("Unknown command..."); PressAnyKeyToExit(); }
            PressAnyKeyToExit();
        }

        public static void Help()
        {
            Console.WriteLine("-help - Opens this");
            Console.WriteLine("-github - Opens GitHub page");
            Console.WriteLine("-dsbs - Disable Search Box Suggestions");
            Console.WriteLine("-esbs - Enable Search Box Suggestions");
            Console.WriteLine("-about - About DSBS");
            Console.WriteLine("-changelog - Open Changelog");
            Console.WriteLine("-version - Shows installed version");
            Console.WriteLine();
            Console.WriteLine("These also work with the first letter of each arg, -d, -e, etc, or with the numeric option.");
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
                    Thread.Sleep(100);
                    Console.WriteLine("Restarting...");
                    Thread.Sleep(200);
                    Process dsbs = new Process();
                    dsbs.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory()) + "\\" + "DisableSearchBoxSuggestions.exe";
                    dsbs.StartInfo.Arguments = " -dsbs";
                    dsbs.Start();
                    Environment.Exit(0);
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
