**Models**
>- Hold the application's core data and state, such as player `health` or gun `ammo`.
>- Serialize, deserialize, and/or convert between types.
>- Load/save data(locally or on the web).
>- Notify Controllers of progress of operations.
>- Store the Game State for Game's `Finite State Machine`.
>- Never access Views.


**Controllers**
>- Do not stroe core data.
>- Can sometimes filter notifications from undesired Views.
>- Update and use the Model's data.
>- Manages Unity's scene workflow.

**Views**
>- Can get data from Models in order to represent up-to-date game state to the user.For example, a View method `player.Run()` can internally use `model.speed` to manifest the player abilities.
>- Should never mutate Models.
>- Strictly implements the functionalities of its class. For example: 
- A `PlayerView` should not implement input detection or modify the Game State.
- A View should act as a block box that has an interface, and notifies of important events.
- Does not store core data(like speed, health, lives,...)