namespace FilamentKostenRechner
{
    public class Filament
    {
        public string Name { get; set; }
        public string Marke { get; set; }
        public double PreisProKg { get; set; }

        public Filament(string name, string marke, double preisProKg)
        {
            Name = name;
            Marke = marke;
            PreisProKg = preisProKg;
        }

        public override string ToString()
        {
            return $"{Name} von {Marke} - {PreisProKg} €/kg";
        }
    }
}
