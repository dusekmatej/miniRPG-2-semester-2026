# AI Logbook - Záznam využití AI v projektu

Tato sekce obsahuje popis konkrétních úloh, u kterých bylo využito AI (např. GitHub Copilot, ChatGPT), a ukázky promptů, které byly použity k vyřešení technických problémů.

## 1. Optimalizace vykreslovací smyčky (Performance)
**Problém:** Windows Forms mají tendenci se při vykreslování většího množství objektů zasekávat a hra běžela pod 40 FPS.
**Využití AI:** Pomoc s nastavením `DoubleBuffered` a optimalizací cyklu v `MainForm`.

**Prompt:**
> "Mám problém s tím, že mi hra WinForms neběží plynule na 60 FPS. Mám MainForm s timerem nastaveným na 16ms, ale reálně to dosahuje jen k 40 FPS. Jaké jsou nejlepší praktiky pro plynulé vykreslování v C# Windows Forms bez změny herní logiky?"

## 2. Procedurální generování (Perlin Noise)
**Problém:** Implementace vlastního algoritmu pro pseudonáhodné generování mapy.
**Využití AI:** Vysvětlení matematického pozadí Perlin Noise a pomoc s optimalizací interpolace.

**Prompt:**
> "Potřebuji implementovat Perlin Noise třídu v C# pro generování 2D mapy v mém RPG. Můžeš mi vysvětlit, jak fungují gradientní vektory a jak nejefektivněji napsat Fade funkci pro plynulé přechody mezi biomy?"

## 3. UI Systém - Experience Bar
**Problém:** Potřeba dynamicky zobrazovat XP bar a hlášky o level upu s animací.
**Využití AI:** Návrh struktury pro `UiLayer` a časování zmizení elementů.

**Prompt:**
> "Implementuj do třídy UiLayer následující: Kdykoliv získám XP, chci nahoře vidět 'Statistic Bar' (podobně jako boss-bar v Minecraftu), který ukáže název skillu a hodnotu XP. Bar musí po 4 sekundách zmizet. Při level-upu ukaž uprostřed obrazovky velký nápis 'LEVEL UP!' s jednoduchou animací."

## 4. Save/Load Systém (JSON Serializace)
**Problém:** Robustní ukládání herního stavu a ošetření poškozených souborů.
**Využití AI:** Pomoc s nastavením `JsonSerializerOptions` a strukturou try-catch bloku pro detekci chyb v souborech.

**Prompt:**
> "Jak v C# nejlépe ošetřit situaci, kdy načítám JSON soubor přes System.Text.Json, ale soubor je poškozený nebo má neplatnou strukturu? Potřebuji, aby mi SaveManager vyhodil smysluplnou chybu a nespadla celá hra."
 
