

```markdown
# Zoo Manager App

Diese C#-basierte Zoo Manager App verwaltet Zoos und Tiere in einer SQL Server-Datenbank. Die App ermöglicht es, Tiere verschiedenen Zoos zuzuordnen und stellt die Daten über die Datenbanktabellen **Animal**, **Zoo** und **ZooAnimal** dar. Die Daten werden in einem relationalen Datenbankmodell gespeichert, das durch Fremdschlüsselbeziehungen zwischen den Tabellen verbunden ist.

## Datenbankstruktur

### Tabelle: Animal
Die **Animal**-Tabelle speichert die Daten der Tiere, die in den Zoos verwaltet werden.

| Spalte | Datentyp | Beschreibung |
|--------|----------|--------------|
| Id     | INT      | Eindeutige Identifikationsnummer des Tieres (Primärschlüssel, Identity) |
| Name   | VARCHAR(50) | Name des Tieres |

```sql
CREATE TABLE [dbo].[Animal] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

### Tabelle: Zoo
Die **Zoo**-Tabelle speichert Informationen zu den Zoos, in denen Tiere gehalten werden.

| Spalte   | Datentyp    | Beschreibung |
|----------|-------------|--------------|
| Id       | INT         | Eindeutige Identifikationsnummer des Zoos (Primärschlüssel, Identity) |
| Location | VARCHAR(50) | Standort des Zoos |

```sql
CREATE TABLE [dbo].[Zoo] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Location] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

### Tabelle: ZooAnimal
Die **ZooAnimal**-Tabelle stellt die Zuordnung zwischen einem Zoo und den darin gehaltenen Tieren dar.

| Spalte   | Datentyp | Beschreibung |
|----------|----------|--------------|
| Id       | INT      | Eindeutige Identifikationsnummer der Zuordnung (Primärschlüssel, Identity) |
| ZooId    | INT      | Verweis auf die **Id** der Zoo-Tabelle (Fremdschlüssel) |
| AnimalId | INT      | Verweis auf die **Id** der Animal-Tabelle (Fremdschlüssel) |

Die Tabelle enthält zwei Fremdschlüssel:
- **ZooFK**: Verknüpft die **ZooId** mit der **Id** in der Zoo-Tabelle.
- **AnimalFK**: Verknüpft die **AnimalId** mit der **Id** in der Animal-Tabelle.

```sql
CREATE TABLE [dbo].[ZooAnimal] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [ZooId]    INT NOT NULL,
    [AnimalId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AnimalFK] FOREIGN KEY ([AnimalId]) REFERENCES [dbo].[Animal] ([Id]),
    CONSTRAINT [ZooFK] FOREIGN KEY ([ZooId]) REFERENCES [dbo].[Zoo] ([Id])
);
```

## Join-Statement

Um eine Liste aller Tiere mit ihren zugehörigen Zoos anzuzeigen, kannst du folgendes Join-Statement verwenden:

```sql
SELECT 
    A.Name AS AnimalName, 
    Z.Location AS ZooLocation
FROM 
    ZooAnimal ZA
JOIN 
    Animal A ON ZA.AnimalId = A.Id
JOIN 
    Zoo Z ON ZA.ZooId = Z.Id;
```

Dieses Statement verknüpft die **Animal**-, **Zoo**- und **ZooAnimal**-Tabellen, um eine Übersicht über alle Tiere und die jeweiligen Zoos, in denen sie sich befinden, zu liefern.

## Verwendung

- Füge die oben beschriebenen Tabellen in deine SQL-Datenbank ein.
- Verwende die Zoo Manager App, um Daten zu Zoos und Tieren zu verwalten und anzuzeigen.
- Mit dem Join-Statement kannst du eine Liste der Tiere und ihrer Zoos abrufen.

## Anforderungen

- C# (WPF oder WinForms) für die App-Entwicklung
- SQL Server für die Datenbank
- Visual Studio für die Projektverwaltung
```

```
- Datenquellen = shift + alt + d

```