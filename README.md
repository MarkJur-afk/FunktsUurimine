# FunktsUurimine — parandatud näidisprojekt

ASP.NET Core MVC + EF Core + SQLite rakendus matemaatiliste funktsioonide uurimiseks.

## Mis on parandatud võrreldes algse projektiga

- eemaldatud dubleeritud `AddControllersWithViews()`;
- eemaldatud kasutamata Razor Pages `.cshtml.cs` failid;
- valemi kontroll toimub enne salvestamist;
- `DataTable.Compute` asendatud turvalisema rekursiivse parseriga;
- toetatud `+ - * / ^`, `sin`, `cos`, `tan`, `sqrt`, `ln`, `log`, `exp`, `abs`, `pi`, `e`;
- tuletis arvutatakse AST-põhise sümboolse tuletamisega;
- lisatud nullkohtade leidmine;
- kriitilised punktid leitakse tuletise nullkohtadest;
- maksimum/minimum määratakse tuletise märgi muutuse järgi;
- määramispiirkonda ei väideta automaatselt `R`, kui funktsioonil esineb katkestusi;
- CRUD kasutab `AsNoTracking()` lugemisel;
- `Delete` kontrollib, kas objekt eksisteerib;
- andmebaasi migratsioon rakendatakse käivitamisel;
- graafiku valem süstitakse JavaScripti JSON-serialiseerimisega, mitte otse stringi sisse;
- UI on lihtsam ja eemaldatud ebavajalikud failid.

## Käivitamine

1. Paigalda .NET 8 SDK.
2. Ava terminal projekti kaustas.
3. Käivita:

```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet run
```

Kui `dotnet ef` puudub:

```bash
dotnet tool install --global dotnet-ef
```

Seejärel ava terminalis näidatud localhost-aadress.

## Näited

```text
x^2 - 4*x + 3
x^3 - 3*x
sin(x)
1/(x-2)
sqrt(x)
```

## Märkus

Numbriline nullkohtade ja määramispiirkonna tuvastamine toimub piiratud vahemikus `[-20, 20]`. See on teadlik piirang, mitte väide, et programm suudab analüüsida iga võimalikku matemaatilist funktsiooni kogu reaalteljel.
