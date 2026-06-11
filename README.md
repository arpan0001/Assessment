# Assessment
Unity Developer Technical Assessment
By - Arpan Kandari

# PROJECT OVERVIEW 

The Inspection Training System is a  interactive training application developed in Unity that allows users to inspect a collection of safety-related objects and complete inspection objectives.
Instead of relying on a traditional monolithic design—where managers directly handle, query, and hardcode logic for specific assets—this solution is built from the ground up using a Data-Driven and Event-Driven Architecture.

The application demonstrates a maintainable and production-oriented architecture while satisfying the requirements of:

Object interaction
Objective tracking
Data-driven design
Event-driven communication
Save and load functionality
Cross-platform input support
Performance awareness

The primary objective of the system is to guide a user through a sequential training process where they locate, select, and inspect specific 3D workspace items (e.g., Safety Helmet, Toolbox, Fire Extinguisher)while seamlessly saving progress and updating the UI dynamically.


# ARCHITECTURE

The project follows a modular architecture where each system has a single responsibility.

1. Data Layer (ScriptableObjects)
Instead of hardcoding values or maintaining messy configuration files, the system uses Unity’s native ScriptableObject format to drive the simulation.

InspectionObjectData: Acts as the single source of truth for an inspectable item. It holds the unique string identifier (ObjectId), player-facing name, and educational descriptions.

ObjectiveData: Stores the master sequence of tasks. It holds an ordered list of ObjectId strings defining the exact order in which objects must be inspected to complete the training.

2. Interaction & Input System
This layer acts as the bridge between player physics and code logic. It captures hardware events and routes them into the simulated world.

InputManager: Monitors mouse clicks and mobile touch inputs. It handles cross-platform input wrapping and fires a single Physics.Raycast only when an explicit click or tap is registered. Crucially, it checks the EventSystem to ensure the user isn't clicking on a UI button before passing the raycast through to the 3D scene.

InteractableObject: A component attached to physical 3D prefabs (like the Helmet or Toolbox). It holds a reference to its corresponding InspectionObjectData. When hit by the InputManager's raycast, it broadcasts its data packet to the rest of the game via GameEvents.ObjectSelected.

3. Core Logic & Objective Tracking (ObjectiveManager)
This system acts as the state machine and rule validator for the training session.

Responsibilities: It listens for the GameEvents.ObjectInspected event. When an object is inspected, it compares the incoming ObjectId against the required ID at the current index of the ObjectiveData list.

If the ID matches, it increments the progress index, commands the SaveManager to commit the save, and checks if the player has reached the end of the list. If they have, it fires GameEvents.TrainingCompleted.

4. SaveManager
A lightweight data handler that ensures user progress isn't lost between application restarts.

Responsibilities: It utilizes a singleton pattern to persist across scenes (DontDestroyOnLoad). It serializes the player's current objective index into a utility JSON string and saves it securely via PlayerPrefs. Upon system initialization, it feeds this data back to the ObjectiveManager to seamlessly restore the state.

5. UI & Presentation Layer
This layer is completely passive, relying on reactive programming to update what the player sees. It contains no internal game state logic.

UIManager: Listens for GameEvents.ObjectSelected to populate the text fields of the inspection panel with the selected asset's name and description. When the player clicks the physical "Inspect" button, it passes that intent forward by invoking GameEvents.ObjectInspected.

ProgressUI & CompletionUI: These components listen strictly for state changes (ObjectInspected and TrainingCompleted). They completely avoid performance-heavy Update() polling, updating text strings and activating completion panels only at the precise microsecond an event is fired.

# DESIGN DECISIONS

1. ScriptableObjects over Hardcoding

What I did: Object data (Names, IDs, Descriptions) and the objective sequence are stored in Unity ScriptableObjects instead of inside C# scripts.

Why: This completely separates game data from game logic. Designers can change text, add new items, or rearrange the training order entirely within the Unity Inspector without touching a single line of code.

2. Event-Driven Architecture

What I did: All major systems (Input, UI, Objectives, and Saving) communicate exclusively by subscribing and publishing to a static GameEvents script.

Why: This creates loose coupling. The InputManager doesn't need to know the UIManager exists, and the ObjectiveManager doesn't need to know about the SaveManager. This makes the codebase incredibly easy to expand, debug, and test in isolation.

3. Singleton Pattern for Save Management

What I did: The SaveManager uses a persistent Singleton pattern.

Why: A single, centralized player profile tracker is required to manage game data safely across application lifecycles. It provides a clean, global access point for saving and loading without risking duplicate managers.

4. Raycasting & Pointer Blocking
What we did: Combined cross-platform input logic with a EventSystem.current.IsPointerOverGameObject() check before casting lines into the 3D space.

Why: This prevents the notorious "click-through" bug, ensuring that if a user clicks an "Inspect" button on the UI canvas, the system doesn't accidentally raycast through the UI and select a 3D object standing behind it.

# SCALABILITY

Because this system avoids hardcoded logic and direct dependencies, it can easily scale from a small 5-object assignment into a massive, multi-level enterprise training application.

1. Adding 100+ New Objects

The Old, Bad Way: Writing 100 new scripts or adding giant switch statements.

My Scalable Way: Create a new InspectionObjectData ScriptableObject for each item, fill in the fields, and drop it onto a generic prefab. Zero lines of code are written.

2. Multi-Level Training Courses

Instead of a single sequence, the ObjectiveData structure can be expanded to support multiple training tracks (e.g., Electrical Safety Course, Fire Hazard Course).

The ObjectiveManager can load different ObjectiveData containers depending on which level or module the user selects from the main menu.

3. Upgrading the Input System

If the project transitions from Mobile/PC to Virtual Reality (VR) or Augmented Reality (AR), the core logic remains completely untouched.You only need to swap or update the InputManager to read VR controller triggers instead of mouse clicks. Once a selection is detected, it fires the exact same

# FUTURE IMPROVEMENTS

1. Advanced Interaction & Visuals

Object Highlighting System: Implement a custom outline shader or post-processing effect to visually highlight the currently selected 3D object.

Inspection Animation Mode: Add an interactive inspection state where selected objects smoothly transition to a screen-center preview layer, allowing the player to rotate and examine the 3D mesh using mouse drag or touch swipe.

2. Audio System

Object interaction sounds
UI feedback sounds
Completion notifications

# Performance & Optimization

1. Performance considerations 

   .Event-Driven Updates
       UI updates occur only when data changes.
       Avoids unnecessary Update() polling.

   .Lightweight Save Files
       Only completed object IDs are stored.

   .HashSet Objective Tracking
       Completed inspections are tracked using HashSet.

2. Optimization Decisions

   Implemented:

   Event-driven architecture
   Scriptable Object data storage
   HashSet objective tracking
   JSON persistence
   Inspector-based dependency injection

 These decisions improve scalability and reduce runtime overhead.

3. Future Optimization Opportunities

   For larger projects:
   Addressables
     Load assets on demand rather than at startup.

   UI Virtualization
     Optimize objective lists containing hundreds of items.