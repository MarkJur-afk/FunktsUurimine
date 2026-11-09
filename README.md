# FunktsUurimine — funktsioonide uurija

FunktsUurimine on veebirakendus, mis uurib ühe muutujaga funktsiooni f(x). Sisesta valem, näiteks `x^2-6*x+9`, ja programm leiab määramispiirkonna, tuletise, nullkohad, kriitilised punktid ja ekstreemumid ning kuvab funktsiooni ja tuletise graafiku. Kasutajaliides on eesti ja vene keeles.

![.NET](https://img.shields.io/badge/.NET-8-512BD4) ![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91) ![SQLite](https://img.shields.io/badge/SQLite-EF%20Core%208-003B57) ![Chart.js](https://img.shields.io/badge/Chart.js-graafik-FF6384) ![xUnit](https://img.shields.io/badge/xUnit-97%20testi-5A9E2F)

Rakendus ei kasuta välist matemaatikateeki: valem parsitakse omaenda parseriga avaldisepuuks ning tuletis leitakse sellelt puult sümboolselt, mitte ligikaudselt.

## Võimalused

- **Sümboolne tuletamine.** Tuletis leitakse analüütiliselt reeglite järgi (summa, korrutis, jagatis, astendamine, ahelreegel) ja kuvatakse tekstina kujul `f'(x) = ...`.
- **Uurimistulemused.** Iga funktsiooni kohta salvestatakse:
  - määramispiirkond,
  - tuletis,
  - nullkohad,
  - kriitilised punktid,
  - ekstreemumid (maksimum või miinimum koos y-väärtusega).
- **Graafik.** Detailvaates kuvatakse f(x) ja f'(x) Chart.js abil.
- **Teooria leht.** Eraldi leht selgitab määramispiirkonda, nullkohti, tuletist, kriitilisi punkte, ekstreemume ja graafiku kuju.
- **Kaks keelt.** Liidese keelt saab vahetada eesti ja vene keele vahel.
- **Hele ja tume teema.** Valik jääb brauseris meelde.
- **Täielik CRUD.** Uurimisi saab lisada, vaadata, muuta ja kustutada.
- **Sisendi kontroll.** Valem on kohustuslik ja kuni 200 märki pikk. Vigase süntaksi korral kuvatakse veateade välja juures.

## Valemi sisestamine

| Tehe | Kirjutamine | Näide |
|---|---|---|
| liitmine / lahutamine | `+` `-` | `x^2-4` |
| korrutamine | `*` | `3*x` |
| jagamine | `/` | `1/(x-2)` |
| astendamine | `^` | `x^3` |
| sulud | `( )` | `(x+1)^2` |
| funktsioonid | `sin`, `cos`, `tan`, `sqrt`, `ln`, `log`, `exp`, `abs` | `sqrt(x)`, `sin(x)` |
| konstandid | `pi`, `e` | `2*pi` |

Näited, mida proovida:

```text
x^2-6*x+9
(x^2-1)/(x^2-4)
x*sqrt(1-x^2)
sin(x)
```

> Korrutusmärk `*` on kohustuslik: kirjuta `3*x`, mitte `3x`.

## Kuidas arvutus töötab

```text
  valem (tekst)
      │
      ▼
ExpressionParser.Parse()          ← oma parser, tulemuseks avaldisepuu (Expr)
      │
      ├─► f'(x) = ast.Derivative()   → Format() → lihtsustamine
      │
      ▼
 Numbriline analüüs (FunctionAnalysisService)
  ├─ nullkohad: skaneerimine vahemikus [-20; 20] sammuga 0,02
  │    + märgivahetusel poolitusmeetod (kuni 80 iteratsiooni)
  ├─ kriitilised punktid: f'(x) = 0 sama meetodiga
  ├─ ekstreemumid: f' märk punktist x ± 0,001 (+ → − maksimum, − → + miinimum)
  └─ määramispiirkond: skaneerimine sammuga 0,05, otsitakse kohti,
       kus väärtus muutub lõplikust mittelõplikuks
```

Head nipid koodis:

- tuletis on täpne (sümboolne), numbrilisi meetodeid kasutatakse ainult juurte ja määramispiirkonna otsimiseks;
- mittelõplikud väärtused (`NaN`, `±∞`) loetakse „määramata", seetõttu töötavad ka `sqrt(x)`, `ln(x)` ja `1/x`;
- parser jälgib tehete prioriteeti: `^` on parem-assotsiatiivne, seega `2^3^2 = 512`.

> **Piirangud.**
> - Nullkohti ja kriitilisi punkte otsitakse vahemikus `[-20; 20]`, tulemused ümardatakse 4 komakohani.
> - Tuletist ei saa leida, kui astendaja pole konstant (nt `2^x`, `x^x`).
> - Puutepunktis olevad juured, kus funktsioon märki ei vaheta (nt `x^2`), leitakse ainult siis, kui ruudustiku punkt satub juurele piisavalt lähedale.
> - Teadaolev viga: unaarne miinus seotakse tugevamini kui `^`, seega `-x^2` arvutatakse kui `(-x)^2`.

## Tehnoloogiad

| Kiht | Tehnoloogia |
|---|---|
| Backend | ASP.NET Core MVC (.NET 8), C# |
| Matemaatika | oma avaldiseparser ja sümboolne tuletamine (`Services/`) |
| Andmebaas | SQLite + Entity Framework Core 8 |
| Graafik | Chart.js |
| Kujundus | oma CSS, hele ja tume teema |
| Testid | xUnit |

## Käivitamine

### Eeldused

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Sammud

```bash
# 1. Klooni repositoorium ja mine projekti kausta
git clone <repositooriumi-aadress>
cd FunktsUurimine

# 2. Taasta sõltuvused
dotnet restore

# 3. Käivita
dotnet run
```

SQLite andmebaas ja vajalikud migratsioonid luuakse käivitamisel automaatselt. Ava brauseris terminalis kuvatud aadress, vaikimisi `http://localhost:5100` (HTTPS-profiiliga `https://localhost:7100`). Visual Studios ava `FunktsUurimine.sln` ja vajuta F5.

## Marsruudid

| URL | Kirjeldus |
|---|---|
| `/` | avaleht |
| `/Home/Theory` | teooria: tuletised ja funktsiooni uurimine |
| `/Funktsioonid` | kõik salvestatud uurimised |
| `/Funktsioonid/Create` | uue funktsiooni lisamine |
| `/Funktsioonid/Details/{id}` | tulemused ja graafik |
| `/Funktsioonid/Edit/{id}` | valemi muutmine (analüüs arvutatakse uuesti) |
| `/Funktsioonid/Delete/{id}` | kustutamine (kinnitusega) |

## Andmemudel

| Väli | Näide |
|---|---|
| `Valem` | `x^2-6*x+9` |
| `Maaramispiirkond` | `R (reaalarvud)` |
| `Tuletis` | `f'(x) = ...` |
| `Nullkohad` | `x1 = 3` |
| `KriitilisedPunktid` | `x1 = 3` |
| `Ekstreemumid` | `miinimum: (3; 0)` |
| `LuodudAeg` | loomise aeg (UTC) |

Andmebaasi asukoht määratakse failis `appsettings.json`:

```json
"ConnectionStrings": { "DefaultConnection": "Data Source=funktsioonid.db" }
```

## Testid

Ühiktestid asuvad projektis `FunktsUurimine.Tests` (xUnit, net8.0). Kokku on 39 testimeetodit, mis annavad 97 testjuhtumit (`[Theory]` ridadega).

| Fail | Mida kontrollib |
|---|---|
| `ExpressionParserTests` | tehete prioriteet ja assotsiatiivsus, funktsioonid, konstandid, vigane süntaks, `Format()` |
| `ExpressionDerivativeTests` | tuletise väärtus punktis, ahelreegel, toetamata astendajad |
| `FunctionAnalysisServiceTests` | nullkohad, ekstreemumid, määramispiirkond, vigased valemid |
| `FunktsiooniUurimineValidationTests` | valemi kohustuslikkus ja pikkuse piir 200 märki |

Testide käivitamine:

```bash
dotnet test FunktsUurimine.sln
```

Üks test on tahtlikult märgitud `Skip`: see kirjeldab matemaatiliselt õiget käitumist (`-x^2` annab punktis 3 tulemuse −9), mida parser praegu ei täida. Kontrollerid ja vaated ei kuulu ühiktestide alla, sest nende kontrollimiseks oleks vaja integratsiooniteste.

Testiprojekt asub rakenduse juurkaustas, seepärast on rakenduse `FunktsUurimine.csproj` failis kaust `FunktsUurimine.Tests` kompileerimisest välistatud.
