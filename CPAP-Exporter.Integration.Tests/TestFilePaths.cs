using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.Integration.Tests
{
    public class TestFilePaths
    {
        public const string AS11_ROOT_PATH = "MachineData\\ResMed\\AirSense\\11\\APAP";
        public const string AS11_NIGHT_PATH = "MachineData\\ResMed\\AirSense\\11\\APAP\\DATALOG\\20241017";

        public const string AC10_ROOT_PATH = "MachineData\\ResMed\\AirCurve\\10\\VAuto";
        public const string AC10_NIGHT_PATH = "MachineData\\ResMed\\AirCurve\\10\\VAuto\\20250202";

        public const string AirBreak_AS10_ASV_ROOT_PATH = "MachineData\\ResMed\\AirBreak\\AS10\\ASVAuto";
        public const string AirBreak_AS10_ASV_NIGHT_PATH = "MachineData\\ResMed\\AirBreak\\AS10\\ASVAuto\\20250211";

        public static string GetEffectivePath(string path) => Path.Combine(Environment.CurrentDirectory, path);

    }
}
