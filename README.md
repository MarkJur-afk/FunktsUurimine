# Funktsioonide uurimine

ASP.NET Core MVC rakendus matemaatiliste funktsioonide analüüsimiseks. Rakendus leiab tuletise, nullkohad, kriitilised punktid ja ekstreemumid ning kuvab graafiku. Kasutajaliides on eesti ja vene keeles.

## Käivitamine

1. Paigalda [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
2. Ava terminal projekti kaustas ja käivita:

```bash
dotnet restore
dotnet run
```

3. Ava terminalis kuvatud `http://localhost:xxxx` aadress.

SQLite andmebaas ja vajalikud migratsioonid luuakse käivitamisel automaatselt.

## Proovi neid valemeid

```text
x^2-6*x+9
(x^2-1)/(x^2-4)
x*sqrt(1-x^2)
sin(x)
```

Nullkohtade ja määramispiirkonna arvutus on numbriline ning toimub vahemikus `[-20, 20]`.
