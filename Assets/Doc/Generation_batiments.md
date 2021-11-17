# Plan Generation de Batiments

### Generation de l'empreinte

- Utilisation d'un L-system pour definir l'empreinte du batiment.
    Il faut s'assurer que les dimensions des façades soient des multiples des dimensions d'un mur
- Pour la generation, il faut garder de la cohérence, il faut definir des nombre min/max pour chaque 
    type de face (fenêtre, mur, porte, balcon...)
- La règle principale étant que le batiment doit être "habitable"
- Prevoir plusieurs types d'assets et donc plusieurs type de batiments
    - immeuble (haut, linéaire, bcp de fenêtres)
    - habitation (peu d'étages, couleurs)
    - Chateau
- Theme -> medieval?
        ->

### Assets

- https://www.kenney.nl/assets/fantasy-town-kit
- https://www.kenney.nl/assets/castle-kit
- https://www.kenney.nl/assets/medieval-town-base


### Etapes
1 -> Generation de la footprint (Avec retour au point de Départ et angle de 90 degrés)
2 -> Generation de footprint avec polyligne (Pas de retour au point de depart géré par le L-system)
3 -> Etablir des règles de remplissage
4 -> Generer un batiment entier
5 -> Generer plusieurs types de batiments
6 -> ...



### En cours

Clement -> Je suis en train d'implémenter les batiments avec étages de taille différentes




