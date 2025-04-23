using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Filament> filamentListe = new List<Filament>();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n📦 Filament-Manager");
            Console.WriteLine("1. Filament hinzufügen");
            Console.WriteLine("2. Filamentliste anzeigen");
            Console.WriteLine("0. Beenden");
            Console.Write("Wähle eine Option: ");
            string eingabe = Console.ReadLine();

            switch (eingabe)
            {
                case "1":
                    Filament neues = new Filament();
                    Console.Write("Name: ");
                    neues.Name = Console.ReadLine();
                    Console.Write("Farbe: ");
                    neues.Farbe = Console.ReadLine();
                    Console.Write("Material: ");
                    neues.Material = Console.ReadLine();
                    Console.Write("Gewicht (g): ");
                    neues.Gewicht = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Preis pro kg (€): ");
                    neues.PreisProKg = Convert.ToDouble(Console.ReadLine());

                    filamentListe.Add(neues);
                    Console.WriteLine("✅ Filament gespeichert!");
                    break;

                case "2":
                    Console.WriteLine("\n📋 Aktuelle Filamente:");
                    foreach (var f in filamentListe)
                    {
                        Console.WriteLine($"- {f.Name} | {f.Farbe} | {f.Material} | {f.Gewicht}g | {f.PreisProKg} €/kg");
                    }
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("❌ Ungültige Eingabe");
                    break;
            }
        }
    }
}
