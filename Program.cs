using System;

namespace Console_Satellite_Tracker {
    class Program {
        static void Main(string[] args) {
            var data = DataLoader.Load();

            Console.WriteLine("Loaded satellite information");
            foreach (var pair in data) {
                Console.WriteLine($"{pair.Value.name} [{pair.Key}]");
            }
        }
    }
}
