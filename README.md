# 2º Projeto de LP2 (2018)

## Our solution

### Division of Tasks

We both structured the program before we began.

Rui: 
* Add base `Inventory`, `IInteractable` and `Pickable` (later `InventoryItem`) classes.
* Add partial quest system.
* Fix bugs.
* Model 90% of the 3D assets (the rest was available for free in the Unity Asset Store).
* Create theme music along with some foleys and edited the rest of the sounds.
* Mount the scenarios.
* Level Design.

Nuno:
* Add objects detection.
* Create basic inventory system.
* Create dialogue system.
* Fix bugs.
* Improve questing system to the final one.
* Create main menu.
* Restructure code.

Both worked on most of the classes as the project kept improving.

### Architecture

Our game is based on multiple available choices, so we make lots of use for `arrays` and `Lists` (from the inventory size to the multiple dialogue options that the NPCs have).

We tried to follow all SOLID design principles to make our software design more understandable, flexible and maintainable.

Design patterns used:
* Observer - We add listeners to `Event` types for checking quest completion upon interaction and dialogue interactions.
* Iterator - C# built-in, we use it to iterate through `IEnumerable` types.
* Template - Used on `NPC` and `QuestGiver` classes (subclass overrides existing methods of the template class).
* Gameloop & Update - Unity built-in. 

The other menu options only show other pages or exit the program.

### UML Diagram

![Diagram(UML)](https://gitlab.com/Robot_Game/making-history/uploads/89ced105b26f70a95cbe923493288357/UML_MakingHistory.png)

## Conclusions

Using SOLID principles as the core of our programming wasn't any easier but in the end we think it has paid off as the classes are very organized and non-dependant on each others.

Despite being a genre that we don't play much, it surely was a challenge and we had lots of fun (and headaches) while creating this game.
Because in the end, if we don't have fun while doing it, then it might not be worth it.

Our knowledge was surely extended and we both worked equally which was a key factor to have the gameplay and polish that we have at this moment.

## References

* <a name="ref1">\[1\]</a> Whitaker, R. B. (2016). The C# Player`s Guide (3rd Edition). Starbound Software

## Metadata

* Authors: Nuno Carriço and Rui Martins