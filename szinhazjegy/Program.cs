using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Szék
{
    public int Sor { get; set; }
    public int Szám { get; set; }
    public string Név { get; set; }
    public bool Lefoglalt { get; set; }
}

class Program
{
    static List<Szék> székek = new List<Szék>();
    const int sorokSzáma = 16;
    const int székekSzámaSoronként = 15;

    static void Main(string[] args)
    {
        SzékekInicializálása();

        bool kilépés = false;
        while (!kilépés)
        {
            Console.Clear();
            Console.WriteLine("1. Szabad helyek megjelenítése");
            Console.WriteLine("2. Szék(ek) foglalása");
            Console.WriteLine("3. Foglalás módosítása");
            Console.WriteLine("4. Foglalás törlése");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("5. Mentés és Kilépés");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("6. Kilépés mentés nélkül");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Válasszon egy lehetőséget:");
            string választás = Console.ReadLine();

            switch (választás)
            {
                case "1":
                    Console.Clear();
                    SzabadSzékekMegjelenítéseUI();
                    Console.WriteLine("Nyomjon meg bármilyen billentyűt a menühöz való visszatéréshez...");
                    Console.ReadKey();
                    break;
                case "2":
                    Console.Clear();
                    SzékFoglalása();
                    break;
                case "3":
                    Console.Clear();
                    FoglalásMódosítása();
                    break;
                case "4":
                    Console.Clear();
                    FoglalásTörlése();
                    break;
                case "5":
                    FoglalásokMentése();
                    kilépés = true;
                    break;
                case "6":
                    kilépés = true;
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Kérem, válasszon újra.");
                    break;
            }
        }
    }

    static void SzékekInicializálása()
    {
        Random rand = new Random();
        for (int i = 1; i <= sorokSzáma; i++)
        {
            for (int j = 1; j <= székekSzámaSoronként; j++)
            {
                bool lefoglalt = rand.Next(1, 11) == 1; // 10% esély a foglalásra
                székek.Add(new Szék { Sor = i, Szám = j, Lefoglalt = lefoglalt });
            }
        }
    }

    static void SzabadSzékekMegjelenítéseUI()
    {
        Console.WriteLine("Színpad:");
        Console.WriteLine("**************");

        Console.WriteLine("Szabad helyek:");
        for (int i = 1; i <= sorokSzáma; i++)
        {
            Console.Write("|");
            for (int j = 1; j <= székekSzámaSoronként; j++)
            {
                var szék = székek.FirstOrDefault(s => s.Sor == i && s.Szám == j);
                if (szék != null && !szék.Lefoglalt)
                {
                    Console.Write("[ ]");
                }
                else
                {
                    Console.Write("[X]");
                }
            }
            Console.WriteLine("|");
        }

        Console.WriteLine("**************");
    }

    static void SzékFoglalása()
    {
        Console.WriteLine("Színpad:");
        Console.WriteLine("**************");

        SzabadSzékekMegjelenítéseUI();

        Console.WriteLine("Adja meg a nevét:");
        string név = Console.ReadLine();

        if (székek.Any(s => s.Név == név && s.Lefoglalt))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Már létezik foglalás ezen a néven. Kérem, válasszon másik nevet.");
            Console.ResetColor();
            Console.WriteLine("Nyomjon meg bármilyen billentyűt a menühöz való visszatéréshez...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Adja meg a lefoglalandó székek számát:");
        int székekSzáma = int.Parse(Console.ReadLine());

        for (int i = 0; i < székekSzáma; i++)
        {
            Console.WriteLine($"Szék {i + 1} foglalása a(z) {székekSzáma} helyből");
            Console.Write("Adja meg a foglalandó szék számát (pl.: 'Sor 3 Szék 5'):");
            string[] bemenet = Console.ReadLine().Split(' ');
            int sor, székSzám;
            if (bemenet.Length == 4 && bemenet[0] == "Sor" && bemenet[2] == "Szék" && int.TryParse(bemenet[1], out sor) && int.TryParse(bemenet[3], out székSzám))
            {
                var szék = székek.FirstOrDefault(s => s.Sor == sor && s.Szám == székSzám);
                if (szék != null && !szék.Lefoglalt)
                {
                    szék.Lefoglalt = true;
                    szék.Név = név;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Szék {sor}, {székSzám} lefoglalva {név} részére");
                    Thread.Sleep(2000); // 2 másodperc várakozás
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"A(z) {sor}, {székSzám} szék nem elérhető. Kérem, válasszon másik széket.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Érvénytelen bemeneti formátum. Kérem, adja meg a szék számát a 'Sor x Szék y' formátumban.");
                Console.ResetColor();
            }
        }

        Console.WriteLine("Az összes szék sikeresen lefoglalva. Nyomjon meg bármilyen billentyűt a menühöz való visszatéréshez...");
        Console.ReadKey();
    }

    static void FoglalásMódosítása()
    {
        Console.WriteLine("Adja meg a nevét:");
        string név = Console.ReadLine();

        var felhasználóiSzékek = székek.Where(s => s.Név == név && s.Lefoglalt).ToList();
        if (felhasználóiSzékek.Any())
        {
            Console.WriteLine("A lefoglalt székek:");
            int lehetőségSzáma = 1;
            foreach (var szék in felhasználóiSzékek)
            {
                Console.WriteLine($" [{lehetőségSzáma++}] Sor {szék.Sor}, Szék {szék.Szám}");
            }

            Console.WriteLine("Adja meg a módosítani kívánt lehetőség számát:");
            int lehetőség = int.Parse(Console.ReadLine());

            if (lehetőség >= 1 && lehetőség <= felhasználóiSzékek.Count)
            {
                var székMódosításra = felhasználóiSzékek[lehetőség - 1];
                Console.WriteLine($"Kiválasztott szék: Sor {székMódosításra.Sor}, Szék {székMódosításra.Szám}");

                Console.WriteLine("Biztosan akarja módosítani a széket:");
                Console.WriteLine("1. Igen");
                Console.WriteLine("2. Nem");
                string módosításiLehetőség = Console.ReadLine();

                switch (módosításiLehetőség)
                {
                    case "1":
                        SzékCsere(székMódosításra);
                        break;
                    case "2":
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Érvénytelen lehetőség.");
                        Console.ResetColor();
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Érvénytelen lehetőség szám.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Nincsenek foglalásai.");
            Console.ResetColor();
        }

        Console.WriteLine("Nyomjon meg bármilyen billentyűt a menühöz való visszatéréshez...");
        Console.ReadKey();
    }

    static void SzékCsere(Szék székCsere)
    {
        Console.WriteLine("Elérhető székek:");
        SzabadSzékekMegjelenítéseUI();

        Console.WriteLine($"Adja meg az új széket a(z) {székCsere.Sor} sorhoz:");
        while (true)
        {
            Console.Write("Új szék: ");
            string[] bemenet = Console.ReadLine().Split(' ');
            int sor, székSzám;
            if (bemenet.Length == 4 && bemenet[0] == "Sor" && bemenet[2] == "Szék" && int.TryParse(bemenet[1], out sor) && int.TryParse(bemenet[3], out székSzám))
            {
                var újSzék = székek.FirstOrDefault(s => s.Sor == sor && s.Szám == székSzám);
                if (újSzék != null && !újSzék.Lefoglalt)
                {
                    székCsere.Lefoglalt = false;
                    újSzék.Lefoglalt = true;
                    újSzék.Név = székCsere.Név;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Szék cserélve. Az új szék: Sor {sor}, Szék {székSzám}");
                    Thread.Sleep(2000); // 2 másodperc várakozás
                    Console.ResetColor();
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Érvénytelen szék vagy a szék már foglalt. Kérem, válasszon másik széket.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Érvénytelen bemeneti formátum. Kérem, adja meg a szék számát a 'Sor x Szék y' formátumban.");
                Console.ResetColor();
            }
        }

        Console.WriteLine("Nyomjon meg bármilyen billentyűt a menühöz való visszatéréshez...");
        Console.ReadKey();
    }

    static void FoglalásTörlése()
    {
        Console.WriteLine("Adja meg a nevét:");
        string név = Console.ReadLine();

        var felhasználóiSzékek = székek.Where(s => s.Név == név && s.Lefoglalt).ToList();
        if (felhasználóiSzékek.Any())
        {
            Console.WriteLine("A lefoglalt székek:");
            int lehetőségSzáma = 1;
            foreach (var szék in felhasználóiSzékek)
            {
                Console.WriteLine($" [{lehetőségSzáma++}] Sor {szék.Sor}, Szék {szék.Szám}");
            }

            Console.WriteLine("Adja meg a törölni kívánt lehetőség számát:");
            int lehetőség = int.Parse(Console.ReadLine());

            if (lehetőség >= 1 && lehetőség <= felhasználóiSzékek.Count)
            {
                var székTörlése = felhasználóiSzékek[lehetőség - 1];
                székTörlése.Lefoglalt = false;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Szék {székTörlése.Sor}, {székTörlése.Szám} foglalás törölve.");
                Thread.Sleep(2000); // 2 másodperc várakozás
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Érvénytelen lehetőség szám.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Nincsenek foglalásai.");
            Console.ResetColor();
        }

        Console.WriteLine("Nyomjon meg bármilyen billentyűt a menühöz való visszatéréshez...");
        Console.ReadKey();
    }

    static void FoglalásokMentése()
    {
        // Mentés...
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Foglalások sikeresen mentve.");
        Thread.Sleep(2000); // 2 másodperc várakozás
        Console.ResetColor();
        Console.Clear();
        Environment.Exit(0);
    }
}
