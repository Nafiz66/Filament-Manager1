using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Globalization;

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
            Console.WriteLine("\n🎛️ Filament-Manager");
            Console.WriteLine("1 – Neues Filament hinzufügen");
            Console.WriteLine("2 – Filamente anzeigen");
            Console.WriteLine("3 – Druckkosten berechnen");
            Console.WriteLine("4 – Beenden");
            Console.Write("➡️ Auswahl: ");
            string eingabe = Console.ReadLine();

            switch (eingabe)
            {
                case "1":
                    FilamentHinzufuegen();
                    break;
                case "2":
                    FilamenteAnzeigen();
                    break;
                case "3":
                    DruckkostenBerechnen();
                    break;
                case "4":
                    laufend = false;
                    Console.WriteLine("📦 Programm wird beendet...");
                    break;
                default:
                    Console.WriteLine("⚠️ Ungültige Eingabe");
                    break;
            }
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
        double gewicht = double.Parse(Console.ReadLine().Replace(',', '.'),
            CultureInfo.InvariantCulture);

        Console.Write("💶 Preis pro Kilogramm (€): ");
        double preisProKg = double.Parse(Console.ReadLine().Replace(',', '.'),
            CultureInfo.InvariantCulture);

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
        Console.WriteLine("✅ Filament gespeichert!");
    }

    static void FilamenteAnzeigen()
    {
        if (filamentListe.Count == 0)
        {
            Console.WriteLine("📭 Keine Filamente vorhanden.");
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
            Console.WriteLine("⚠️ Keine Filamente vorhanden.");
            return;
        }

        FilamenteAnzeigen();
        Console.Write("\n➡️ Nummer des gewünschten Filaments: ");
        if (!int.TryParse(Console.ReadLine(), out int auswahl) || auswahl < 1 || auswahl > filamentListe.Count)
        {
            Console.WriteLine("❌ Ungültige Auswahl.");
            return;
        }

        var f = filamentListe[auswahl - 1];

        Console.Write("🧾 Verbrauch in Gramm (z. B. 23,5): ");
        string verbrauchInput = Console.ReadLine().Replace(',', '.');

        if (!double.TryParse(verbrauchInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double verbrauch))
        {
            Console.WriteLine("❌ Ungültiger Verbrauchswert.");
            return;
        }

        double kosten = (verbrauch / 1000.0) * f.PreisProKg;
        double rest = f.Gewicht - verbrauch;

        Console.WriteLine($"\n💰 Druckkosten: {kosten:F2} €");
        Console.WriteLine($"📦 Restgewicht von {f.Name}: {rest:F1}g");
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
