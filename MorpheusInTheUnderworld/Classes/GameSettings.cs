using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes
{
    
    public static class GameSettings
    {

        private static string currentPath = Directory.GetCurrentDirectory();
        private static string configurationPath = currentPath + "\\configuration.txt";

        // Properties to be written
        public static int MasterVolume { get; set; }
        public static int MusicVolume { get; set; }
        public static int EffectsVolume { get; set; }

        // After you modify the Properties to be written, issue an Write() Method.
        // To save the configuration, and in game startup perform a Read() Call.

        // This method will write to a configuration.txt file.
        public static void Write()
        {


                string[] lines = { "master_volume:" + MasterVolume,
                               "music_volume:" + MusicVolume,
                               "effects_volume:" + EffectsVolume};
                File.WriteAllLines(configurationPath, lines);
            
        }

        // this method will read from a configuration.txt file
        public static void Read()
        {
            // Read only if file exists!!
            if (File.Exists(configurationPath))
            {
                string[] lines = File.ReadAllLines(configurationPath);

                // Parsing configurations using Write Method order of writing its configuration
                // example, master_volume[0] goes first, then music[1] and effects[2] in that order.

                MasterVolume = Convert.ToInt32(lines[0].Split(':')[1]);
                MusicVolume = Convert.ToInt32(lines[1].Split(':')[1]);
                EffectsVolume = Convert.ToInt32(lines[2].Split(':')[1]);
            }
        }
       
    }
}
