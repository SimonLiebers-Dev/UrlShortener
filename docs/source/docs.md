# Dokumentation

>Dokumentation zur Projektarbeit im Fach "Software-Qualitätssicherung" an der TH Rosenheim (SoSe25) von Simon Liebers

Der Aufbau dieser Dokumentation orientiert sich am offiziellen [Arc42-Template](https://docs.arc42.org/home/)

## Kapitel 0: Voraussetzungen
Folgende Softwarevoraussetzungen müssen zum lokalen Testen der Anwendung gegegeben sein:
- Node.js in der aktuellen LTS-Version
- .NET 7
- eine laufende PostgreSQL-Instanz

## Kapitel 1: Einleitung
### Fachliche Anforderungen
Die *PokemonApp* soll es dem Nutzer ermöglichen, Basisinformationen zu allen derzeit bekannten Pokemon zu erhalten. Nach Eingabe der individuellen Nummer eines Pokemon werden sein Name, sein Bild in Form eines Sprite sowie sein Typ bzw. Typen (sofern das Pokemon zwei Typen besitzt) angezeigt. Sind die jeweiligen Daten in der angebundenen SQL-Datenbank bereits vorhanden, werden sie direkt über eine entsprechende Datenbankabfrage gewonnen. Ansonsten erfolgt eine HTTP-REST Anfrage über die öffentlich verfügbare [PokeAPI](https://pokeapi.co/). Die erhaltenen Daten werden daraufhin in der Datenbank abgespeichert. Damit soll sichergestellt werden, dass im Falle des Nichtvorhandenseins der API dennoch ein Mindestmaß an positiver User Experience gegeben ist und bereits vorhandene Daten weiterhin abgerufen werden können.

### Qualitätsziele
TODO

### Stakeholder
TODO

## Kapitel 2: Beschränkungen
TODO

## Kapitel 3: Kontext und Umfang
TODO: C4 Modelle

### Kontextdiagramm (Level 1)
TODO

### Containerdiagramm (Level 2)
TODO

### Komponentendiagramm (Level 3)
TODO

## Kapitel 4: Lösungsstrategie

### Technische Entscheidungen
TODO

## Kapitel 5: Baustein-Sicht 
TODO

## Kapitel 6: Runtime-Sicht
TODO

## Kapitel 7: Deployment-Sicht
TODO

## Kapitel 8: Querschnittskonzepte

### Sicherheit
TODO

### User Interface
TODO

### User Experience
TODO

## Kapitel 9: Architekturentscheidungen
ADRs

|Sektion    |Beschreibung|
|---        |---         |
|Titel   |ADR 1: TODO   |
|Kontext   | - |
|Entscheidung   | - |
|Status   | - |
|Konsequenzen   |  - |

## Kapitel 10: Qualität
### Nicht-funktionale Qualitätsanforderungen nach ISO 25010

#### Sicherheit
TODO

#### Effizienz und Performanz
TODO

#### Wartbarkeit
TODO

- **Wartbarkeit und Erweiterbarkeit**: TODO

- **Wirtschaftlichkeit**: TODO

#### Benutzbarkeit
- **Benutzerfreundlichkeit**: TODO

#### Kompatibilität

- **Interoperabilität und Integration**: TODO
- **Funktionale Eignung**: TODO
- **Portabilität**: TODO
- **Zuverlässigkeit**: TODO

## Kapitel 11: Qualitätssichernde Maßnahmen und Tests
TODO: Pipeline

### Unittests (Backend)
TODO

### Unittests (Frontend)
TODO

 ### Integrationstests
TODO

### Statische Codeanalyse
TODO

### Penetration-Tests
TODO

### End2End-Tests
TODO: PlayWright

### Last-Tests
TODO