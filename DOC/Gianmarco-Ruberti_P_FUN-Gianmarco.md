# Gianmarco-Ruberti/P_FUN-Gianmarco

*Exporté le 28.10.2026 — 4 User Stories*
*Source : https://github.com/Gianmarco-Ruberti/P_FUN-Gianmarco*

---

## #1 — Stockage local des séries temporelles

En tant qu' utilisateur,
je veux que l'ensemble des séries importées soit sauvegardé localement sur mon poste,
afin de pouvoir relancer le logiciel et travailler sur mes données en mode hors-ligne.

- [ ]  GIVEN des données importées avec succès, WHEN l'application est fermée puis rouverte, THEN les 5 séries temporelles sont automatiquement rechargées.
- [ ] GIVEN une utilisation sans connexion réseau, WHEN l'application démarre, THEN toutes les séries enregistrées localement restent accessibles.

---

## #3 — Affichage graphique simultané des 5 séries (ScottPlot)

En tant qu' utilisateur,
je veux afficher une représentation graphique de 5 séries du champ magnétique sur un axe temporel commun,
afin de comparer visuellement leurs variations en nanoteslas (nT) sur la même période.

<img alt="Image" src="Gianmarco-Ruberti_P_FUN-Gianmarco_images/us3-1.png" />

- [ ] GIVEN 5 séries enregistrées, WHEN l'utilisateur ouvre la vue principale, THEN le graphique affiche les 5 courbes avec des couleurs distinctes.
- [ ] GIVEN le graphique affiché, WHEN l'utilisateur observe l'axe horizontal (X), THEN l'axe représente le temps (timestamps synchronisés).
- [ ] GIVEN le graphique affiché, WHEN l'utilisateur observe l'axe vertical (Y), THEN l'échelle est exprimée en nanoteslas (nT) pour toutes les séries.

---

## #4 — Sélection et filtrage dynamique des séries

En tant qu' utilisateur,
je veux pouvoir cocher ou décocher chaque série via des cases à cocher (Checkboxes),
afin de masquer les courbes inutiles et d'analyser en détail seulement 2 ou 3 séries spécifiques.

<img alt="Image" src="Gianmarco-Ruberti_P_FUN-Gianmarco_images/us4-1.png" />


- [ ] GIVEN un graphique avec 5 séries actives, WHEN l'utilisateur décoche Bz_gsm, THEN la courbe correspondante disparaît immédiatement du graphique.
- [ ] GIVEN une série masquée, WHEN l'utilisateur coche à nouveau la case, THEN la courbe réapparaît exactement à sa place sans recharger l'ensemble des données.

---

## #5 — Ajout manuel d'une mesure à une série temporelle

[BONUS/si le temps le permet]

En tant qu' utilisateur,
je veux pouvoir ajouter manuellement une nouvelle valeur datée à une série existante,
afin de compléter mes données locales si une mesure manque.

<img alt="Image" src="Gianmarco-Ruberti_P_FUN-Gianmarco_images/us5-1.png" />

- [ ] GIVEN une série sélectionnée, WHEN l'utilisateur saisit une date et une valeur valide puis valide, THEN le nouveau point est inséré dans le stockage local.
- [ ] GIVEN un nouveau point ajouté, WHEN le graphique se rafraîchit, THEN le point apparaît à la bonne position chronologique sur la courbe.
- [ ] GIVEN une saisie avec une valeur non numérique, WHEN l'utilisateur valide, THEN un message d'avertissement bloque la saisie.

---
