# FunktsUurimine — parandatud näidisprojekt

ASP.NET Core MVC + EF Core + SQLite rakendus matemaatiliste funktsioonide uurimiseks.

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
