using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public class Filament
{
    public string Name { get; set; }
    public string Farbe { get; set; }
    public string Material { get; set; }
    public double Gewicht { get; set; } // in Gramm
    public double PreisProKg { get; set; } // in Euro
}

class Program
{
    static List<Filament> filamentListe = new List<Filament>();
    static string dateiPfad = "filamente.json";

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        DatenLaden();

        bool laufend = true;
        while (laufend)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n🎛️ Filament-Manager");
            Console.ResetColor();
            Console.WriteLine("1 – Neues Filament hinzufügen");
            Console.WriteLine("2 – Filamente anzeigen");
            Console.WriteLine("3 – Druckkosten berechnen");
            Console.WriteLine("4 – Beenden");
            Console.Write("➡️ Auswahl: ");
            string eingabe = Console.ReadLine();

            switch (eingabe)
            {
                case "1": FilamentHinzufuegen(); break;
                case "2": FilamenteAnzeigen(); break;
                case "3": DruckkostenBerechnen(); break;
                case "4": laufend = false; Console.WriteLine("📦 Programm wird beendet..."); break;
                default: Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("⚠️ Ungültige Eingabe"); Console.ResetColor(); break;
            }

            Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
            Console.ReadKey();
        }
    }

    static void FilamentHinzufuegen()
    {
        Console.WriteLine("\n📦 Neues Filament hinzufügen");

        Console.Write("📝 Name: ");
        string name = Console.ReadLine();

        Console.Write("🎨 Farbe: ");
        string farbe = Console.ReadLine();

        Console.Write("🧪 Material: ");
        string material = Console.ReadLine();

        Console.Write("⚖️ Gewicht in Gramm: ");
        double gewicht = double.Parse(Console.ReadLine());

        Console.Write("💶 Preis pro Kilogramm (€): ");
        double preisProKg = double.Parse(Console.ReadLine());

        Filament neuesFilament = new Filament
        {
            Name = name,
            Farbe = farbe,
            Material = material,
            Gewicht = gewicht,
            PreisProKg = preisProKg
        };

        filamentListe.Add(neuesFilament);
        DatenSpeichern();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Filament gespeichert!");
        Console.ResetColor();
    }

    static void FilamenteAnzeigen()
    {
        if (filamentListe.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("📭 Keine Filamente vorhanden.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("\n📋 Verfügbare Filamente:");
        int index = 1;
        foreach (var f in filamentListe)
        {
            Console.WriteLine($"{index++}. {f.Name} | {f.Farbe} | {f.Material} | {f.Gewicht}g | {f.PreisProKg:F2} €/kg");
        }
    }

    static void DruckkostenBerechnen()
    {
        if (filamentListe.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ Keine Filamente vorhanden.");
            Console.ResetColor();
            return;
        }

        FilamenteAnzeigen();
        Console.Write("\n➡️ Nummer des gewünschten Filaments: ");
        if (!int.TryParse(Console.ReadLine(), out int auswahl) || auswahl < 1 || auswahl > filamentListe.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Ungültige Auswahl.");
            Console.ResetColor();
            return;
        }

        var f = filamentListe[auswahl - 1];

        Console.Write("🧾 Verbrauch in Gramm (laut Slicer): ");
        if (!double.TryParse(Console.ReadLine(), out double verbrauch) || verbrauch <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Ungültiger Verbrauch.");
            Console.ResetColor();
            return;
        }

        if (verbrauch > f.Gewicht)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Nicht genug Material vorhanden.");
            Console.ResetColor();
            return;
        }

        double kosten = (verbrauch / 1000.0) * f.PreisProKg;
        f.Gewicht -= verbrauch;
        DatenSpeichern();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n💰 Druckkosten: {kosten:F2} € mit {f.Name} ({f.Material})");
        Console.WriteLine($"📉 Verbleibendes Gewicht: {f.Gewicht:F2}g");
        Console.ResetColor();
    }

    static void DatenSpeichern()
    {
        var json = JsonSerializer.Serialize(filamentListe, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dateiPfad, json);
    }

    static void DatenLaden()
    {
        if (File.Exists(dateiPfad))
        {
            var json = File.ReadAllText(dateiPfad);
            filamentListe = JsonSerializer.Deserialize<List<Filament>>(json);
        }
    }
}
