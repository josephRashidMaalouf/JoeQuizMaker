# Joe Quiz Maker

En quizapp byggd i **.NET MAUI** där användaren kan skapa, redigera, hantera och spela quiz.  
Projektet utvecklades i samband med ett examinationsmoment i kursen **Utveckling mot databas och databasadministration** under utbildningen till **.NET-utvecklare** på **IT-Högskolan i Göteborg**.

## Funktioner

- Skapa nya quiz
- Visa en lista över befintliga quiz
- Redigera quiz
- Hantera frågor
- Spela quiz
- Välja vilket quiz som ska spelas

## Projektstruktur

Projektet är uppdelat i flera delar:

- **JoeQuizApp** – huvudapplikationen byggd med .NET MAUI
- **Common** – gemensamma modeller
- **Labb3DatabaserTemplate** – data access och repositories för quiz, frågor och kategorier

## Tekniker

- **C#**
- **.NET 8**
- **.NET MAUI**
- **MVVM**
- **CommunityToolkit.Mvvm**

## Arkitektur

Appen använder en struktur inspirerad av **MVVM** där:

- **Pages** hanterar användargränssnittet
- **ViewModels** innehåller logik och dataflöde
- **Common/Models** innehåller delade modeller
- **Labb3DatabaserTemplate/Services** ansvarar för datahantering

Detta gör projektet mer lättläst, modulärt och enklare att vidareutveckla.

## Kom igång

### Förutsättningar

För att köra projektet behöver du exempelvis:

- Visual Studio 2022 eller senare
- .NET 8 SDK
- .NET MAUI workload installerad

### Starta projektet

1. Klona repot:
   ```bash
   git clone https://github.com/josephRashidMaalouf/JoeQuizMaker.git
   ```

2. Öppna lösningen i Visual Studio:
   ```bash
   Labb3DatabaserTemplate.sln
   ```

3. Välj önskad målplattform och kör projektet.

## Plattformar

Projektet är konfigurerat för:

- Android
- iOS
- Mac Catalyst
- Windows

## Syfte

Syftet med projektet var att bygga en fungerande quizapplikation med koppling till datalagring, samt att tillämpa kunskaper inom:

- apputveckling med .NET MAUI
- databasnära utveckling
- projektstrukturering
- hantering av quizdata och frågor

## Vidareutveckling

Möjliga förbättringar framåt:

- bättre design och UI/UX
- poängräkning och resultatvy
- timer per fråga
- stöd för fler frågetyper
- kategorifiltrering
- lokal/offline lagring
- bättre validering och felhantering


## Författare

Skapad av **Joseph Rashid Maalouf**
