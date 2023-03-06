using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tarsalgo {
    internal class Program {

        static void Main(string[] args) {
            
            // 1. feladat
            List<Szemely> szemelyek = new List<Szemely>();

            foreach (string one in Properties.Resources.ajto.Split('\n')) {
                if (one.Length != 0) {
                    szemelyek.Add(new Szemely(one));
                }
            }

            Console.WriteLine("2. feladat");
            Console.WriteLine($"Az első belépő: {szemelyek.Find(szemely => !szemely.Outside).Id}");
            Console.WriteLine($"Az utolsó kilépő: {szemelyek.FindLast(szemely => szemely.Outside).Id}\n");

            // 3. feladat
            string fileName = "athaladas.txt";

            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }

            using (StreamWriter writer = File.CreateText(fileName)) {
                foreach (var ordered in szemelyek.GroupBy(sz => sz.Id)) {
                    writer.WriteLine($"{ordered.Distinct().First().Id} {ordered.Count()}");
                }
            }

            Console.WriteLine("4. feladat");
            Console.Write("A végén társalgóban voltak: ");
            foreach (var one in szemelyek.GroupBy(a => a.Id).Select(b => new { id = b.Key, db = b.Count() }).Where(c => c.db % 2 != 0).OrderBy(d => d.id)) {
                Console.Write($"{one.id} ");
            }

            Console.WriteLine("\n\n5. feladat");
            int max = szemelyek.Max(one => one.Bentlevok);

            Console.WriteLine($"Például {szemelyek.Find(szem => szem.Bentlevok == max).Time:HH:mm}-kor voltak a legtöbben a társalgóban.");

            Console.WriteLine("\n6. feladat");
            Console.Write("Adja meg a személy azonosítóját: ");
            int personId = int.Parse(Console.ReadLine());

            Console.WriteLine("\n7. feladat");
            List<Szemely> all = szemelyek.FindAll(szem => szem.Id == personId);

            for (int i = 0; i < all.Count; i += 2) {
                Console.Write($"{all[i].Time:HH:mm} -");

                int next = i + 1;

                if (next < all.Count) {
                    Console.WriteLine($" {all[next].Time:HH:mm}");
                }
            }

            Console.WriteLine("\n\n8. feladat");
            Console.WriteLine($"A(z) {personId}. személy összesen {all.Find(szem => szem.Id == personId).TimeMinute} percet volt bent, a megfigyelés végén a társalgóban.");

            Console.ReadKey();
        }

        private sealed class Szemely {

            public int TimeHour { get; private set; }
            public int TimeMinute { get; private set; }
            public int Id { get; private set; }
            public bool Outside { get; private set; }
            public DateTime Time { get; private set; }
            public int Bentlevok { get; private set; } = 0;

            public Szemely(string data) {
                string[] split = data.Split(' ');

                TimeHour = int.Parse(split[0]);
                TimeMinute = int.Parse(split[1]);
                Id = int.Parse(split[2]);
                
                if (!(Outside = split[3].Contains("ki"))) {
                    Bentlevok++;
                } else {
                    Bentlevok--;
                }

                DateTime now = DateTime.Now;
                Time = new DateTime(now.Year, now.Month, now.Day, TimeHour, TimeMinute, 0);
            }
        }
    }
}
