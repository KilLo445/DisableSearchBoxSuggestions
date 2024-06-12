using System;
using System.Diagnostics;

namespace DisableSearchBoxSuggestions
{
    internal class Program
    {
        public static string version = "1.0";
        public static string githubLink = "https://github.com/KilLo445/DSBS";

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

        static void Main(string[] args)
        {
            Console.Title = "Disable Search Box Suggestions";

            Console.WriteLine(asciiArt);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine();
            Console.WriteLine("[1] Disable Search Box Suggestions");
            Console.WriteLine("[2] Enable Search Box Suggestions");
            Console.WriteLine("[3] Open DSBS GitHub");
            Console.WriteLine();
            Console.Write("Enter choice: ");
            string userInput = Console.ReadLine();
            Console.WriteLine();
            if (userInput == "1") { }
            if (userInput == "2") { }
            if (userInput == "3") { Console.WriteLine("Opening GitHub link..."); Process.Start(githubLink); }
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
