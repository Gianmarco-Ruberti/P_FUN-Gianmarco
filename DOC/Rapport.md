# Rapport
## table des matière
1. [introduction](#introduction)
    - 1.1 [objectifs produit](#objectifs-produit)
    - 1.2 [pédagogique](#pédagogique)
    - 1.3 [Description du domaine](#description-du-domaine)
2. [analyse fonctionnelle](#lanalyse-fonctionnelle)
3. [planification initiale](#la-planification-initiale)
    - 3.1[Macro-Planning](#macro-planning-du-projet)
4. [Rapport de tests](#rapport-de-tests)
5. [Utilisation de L'IA](#utilisation-de-lia-dans-ce-projet)
6. [Bilan de déroulement](#bilan-du-déroulement-de-projet)
7. [Bilan produit](#bilan-produit)
8. [Conclusion](#conclusion)
## introduction
### objectifs produit
Le projet Plot Those Lines (PTL) vise à concevoir une application dédiée à la visualisation et à l'analyse de la météo spatiale. L'application permet d'importer, de stocker localement et d'afficher simultanément jusqu'à 5 séries de données complexes sur un axe temporel commun. Grâce à une interface graphique flexible et un mode de fonctionnement 100 % hors-ligne.
### pédagogique
Ce projet permet de pratiquer et de valider les notions théoriques et techniques vues en cours
### Description du domaine
Le domaine d'application retenu pour ce projet est la météo spatiale, et plus particulièrement la surveillance des variations du champ magnétique interplanétaire (IMF) mesuré au point de Lagrange L1 entre le Soleil et la Terre (via le satellite DSCOVR de la NOAA).

La météo spatiale étudie l'impact de l'activité solaire sur l'environnement spatial terrestre. Lors d'éruptions solaires ou d'éjections de masse coronale, un vent solaire chargé en particules magnétisées frappe la la Terre. Ces événements peuvent perturber les satellites de télécommunication, dégrader les signaux GPS, créer des surtensions sur les réseaux électriques haute tension et générer des aurores polaires.

#### Cohérence d'affichage
Puisque les séries B_x_gse, B_y_gse, B_x_gsm, B_y_gsm et B_t partagent la même unité physique (le nanotesla, nT), elles peuvent être superposées directement sur un axe Y unique sans déformer l'affichage.
## L’analyse fonctionnelle
vous pouvez vois les US avec les lien suivant :
- US-1 : https://github.com/Gianmarco-Ruberti/P_FUN-Gianmarco/issues/1#issue-5232498212
- US-2 : https://github.com/Gianmarco-Ruberti/P_FUN-Gianmarco/issues/2#issue-5232526328
- US-3 : https://github.com/Gianmarco-Ruberti/P_FUN-Gianmarco/issues/3#issue-5232601596
- US-4 : https://github.com/Gianmarco-Ruberti/P_FUN-Gianmarco/issues/4#issue-5232617330

## La planification initiale

La réalisation du projet s'étale sur **24 périodes** réparties du début du trimestre au **30 octobre 2026** (date de livraison finale).

### Macro-Planning du projet

| Phase | Périodes estimées | Activités principales | Livrable associé |
| :--- | :---: | :--- | :--- |
| **1. Analyse & Prise en main** | **4 périodes** | Choix du domaine, rédaction des US, validation du Kanban | **Release v1 (04.09.2026)** |
| **2. Modèle & Parsing JSON** | **4 périodes** | Création des classes C# (`DataPoint`, `Series`), lecture de `solar_mag.json` avec LINQ et stockage local. | Commits branche `main` |
| **3. Interface & ScottPlot** | **9 périodes** | Apprentissage de ScottPlot, création de l'interface WPF, affichage des 5 courbes et gestion des Checkboxes. | Commits branche `main` |
| **4. Fonctionnalités & Tests** | **4 périodes** | Ajout d'une mesure (US-05), création des 2 méthodes d'extension C#, écriture de 3 tests unitaires et corrections. | Rapport de tests |
| **5. Bilan & Documentation** | **3 périodes** | Rédaction de la section sur l'IA, mise au propre du journal de travail et préparation de la release finale. | **Release Finale (30.10.2026)** |

## Rapport de tests
L'obectif de ces test est de validé le fonctionnement de la méthode `LoadFromFile` lors du chargement des donnéees depuis un fichier JSON
les 3 test valide les points suivant :
1. les données valides issues du satéllite **SOLAR1** sont extraites et converties
2. les données d'autres source sont ignorées
3. le système donne les bonne exceptions en cas d'absence de données de **SOLAR1**

| ID Test | nom du test | Scénario de Test | Résultat attendu | Statut |
| :--------------- | :------------ | :---------------: | :---------------: | :------:|
| TU-01   | `LoadFromFile_ValidDataTests` | Chargement d'un fichier JSON contenant des données valide pour `SOLAR1` | Le fichier est lu sans erreur, 1 élément est retourné avec des valeurs `y_gse`, `bz_gse` et `bt` conformes. | **SUCCÈS** |
| TU-02   | `LoadFromFile_FilteringTests` | Chargement d'un fichier JSON contenant une donnée valide pour `SOLAR1` et une donnée de `ACE` | la donnée de `ACE` est ignorée et la donnée de `SOLAR1` est conservée | **SUCCÈS**
| TU-03   | `LoadFromFile_ExceptionTests` |  Chargement d'un fichier JSON contenant uniquement une donnée de `ACE` |  `InvalidDataException` est levée en indiquant l'absence de données valides. | **SUCCÈS**

### TU-01 : Validation du Cas Nominal (LoadFromFile_ValidDataTests.cs)
- Description : S'assure que les données extrait du JSON ont les propriétés requises (`ByGse`, `BzGse`, `Bt`, `TimeTag`) lorsque la source est SOLAR1.
- Couverture : Méthode LoadFromFile et méthode auxiliaire GetDoubleProperty.

### TU-02 : Validation du Filtrage (LoadFromFile_FilteringTests.cs)
- Description : Vérifie le respect des règles concernant la priorité et l'exclusion des satellites autre que **SOLAR1**.
- Couverture : Condition de filtrage source == "SOLAR1".

### TU-03 : Validation de la Gestion des Erreurs (LoadFromFile_ExceptionTests.cs)
- Description : S'assure que l'application ne traite pas un fichier invalide ou sans les données demander.
- Couverture : Bloc de validation final hasValidData et levée d'exception InvalidDataException.
## Utilisation de L'IA dans ce projet
## Bilan du déroulement de projet
## Bilan produit
## Conclusion