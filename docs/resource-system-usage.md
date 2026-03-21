# Resource System Usage Guide

This project now has a small shared foundation for:

- resource definitions
- resource catalogs
- client definitions
- criterion values and ratios
- a runtime inventory

This guide explains what each part is for and how to use it in Unity.

---

## 1. Core idea

The system is split into two parts:

1. **Definition assets**
   - reusable data you create in the Unity editor
   - examples: a rock resource, a chili resource, a slime client

2. **Runtime state**
   - data used while the game is running
   - example: the player's current inventory

This is useful because you can create content in the editor without tying everything directly to scene objects.

---

## 2. Criteria

The current supported criteria are:

- `Mohs`
- `Scoville`
- `PH`

These are defined in the enum in `CriterionType`.

### `CriterionValue`
Use this for resource properties.

Example meaning:
- a resource can have `Mohs = 7`
- a resource can have `PH = 2`

### `CriterionRatio`
Use this for client requests.

Example meaning:
- a client wants a dish that is `60% Scoville`
- a client wants a dish that is `40% PH`

---

## 3. Resource definitions

`ResourceDefinition` is a `ScriptableObject`.

It stores:
- display name
- optional prefab
- size
- a list of criterion values

### How to create one

In Unity:

1. Open the Project window
2. Right click in a folder under `Assets`
3. Select **Create → Upsijam → Definitions → Resource**
4. Fill in the fields in the Inspector

### Example

A resource could be:

- Display Name: `Volcanic Pepper`
- Size: `2`
- Criteria Values:
  - `Scoville = 9`
  - `PH = 3`

Use the prefab field only if you want this resource to have a world object or model linked to it.

---

## 4. Resource catalogs

`ResourceCatalog` is a `ScriptableObject`.

It stores:
- a list of `ResourceDefinition` references
- a spawn weight for each resource entry

Use it when you want a spawner to choose from a reusable set of resource definitions instead of assigning each one directly on a scene object.

### How to create one

In Unity:

1. Open the Project window
2. Right click in a folder under `Assets`
3. Select **Create -> Upsijam -> Definitions -> Resource Catalog**
4. Add entries to the list and assign a weight to each resource

### Example

A conveyor catalog could contain:

- `Volcanic Pepper` with weight `5`
- `River Stone` with weight `2`
- `Crystal Onion` with weight `1`

That means peppers appear most often, stones sometimes, and onions rarely.

---

## 5. Client definitions

`ClientDefinition` is also a `ScriptableObject`.

It stores:
- display name
- optional prefab
- quantity requested
- a list of criterion ratios

### How to create one

In Unity:

1. Open the Project window
2. Right click in a folder under `Assets`
3. Select **Create → Upsijam → Definitions → Client**
4. Fill in the fields in the Inspector

### Example

A client could be:

- Display Name: `Lava Troll`
- Quantity Requested: `5`
- Criteria Ratios:
  - `Scoville = 0.7`
  - `Mohs = 0.3`

This means the client wants 5 units of food with those target proportions.

---

## 6. Inventory

`Inventory` is a runtime C# class.

It stores a list of `InventoryEntry` objects and supports:

- `AddResource(resource, quantity)`
- `RemoveResource(resource, quantity)`
- `GetQuantity(resource)`
- `HasResource(resource, quantity)`

### Behavior summary

- adding the same resource stacks it
- removing too much returns `false`
- removing the last quantity deletes the entry
- asking for an unknown resource returns `0`

### Simple example

```csharp
Inventory inventory = new Inventory();

inventory.AddResource(resourceDefinition, 3);

bool hasEnough = inventory.HasResource(resourceDefinition, 2);
int currentAmount = inventory.GetQuantity(resourceDefinition);
bool removed = inventory.RemoveResource(resourceDefinition, 1);
```

---

## 7. Conveyor setup

The project now also supports a simple conveyor workflow built from:

- `ResourceCatalog`
- `ConveyorSpawner`
- `ConveyorBeltZone`
- `ConveyorItem`

### Unity setup order

1. Create 2 to 4 simple resource prefabs
2. Add a collider to each prefab
3. Add a `Rigidbody` to each prefab, or let the spawner add one as a fallback
4. Create a `ResourceCatalog` asset and add weighted entries
5. Build a straight belt in the scene with side rails to keep items contained
6. Add a trigger collider over the belt and attach `ConveyorBeltZone`
7. Add `ConveyorSpawner` to a scene object and assign the catalog and spawn point transform
8. Press Play and tune spawn interval, acceleration, drag, and cleanup distance

### Runtime behavior summary

- the spawner picks a weighted-random resource from the catalog
- the resource prefab is instantiated at the spawn point
- a `ConveyorItem` component tracks what resource was spawned and where cleanup should happen
- the belt zone applies acceleration to rigidbodies that stay inside the trigger
- items fall off the end naturally
- items are destroyed once they move far enough away

### Graceful failure handling

The conveyor system is defensive by default:

- missing catalog references log a warning and skip spawning
- missing spawn points log a warning and skip spawning
- empty or invalid catalogs log a warning and skip spawning
- resources without prefabs log a warning and are skipped
- prefabs without `Rigidbody` components get one added at runtime with a warning

---

## 8. Typical workflow

A simple way to use the system is:

1. Create several `ResourceDefinition` assets
2. Create one or more `ResourceCatalog` assets for spawnable sets
3. Create several `ClientDefinition` assets
4. Put `ResourceDefinition` references on harvestable world objects or conveyor prefabs
5. When the player collects something, call `Inventory.AddResource()`
6. When a client order starts, read the data from a `ClientDefinition`
7. Later, compare the cooked dish against the client's requested ratios

---

## 9. What this system does not do yet

The current foundation does **not** include:

- cooking evaluation
- client AI
- UI
- saving/loading

The project now includes basic conveyor spawning, but not higher-level production logic or robotic pickup behavior yet.

Those systems can now be built on top of these shared classes.

---

## 10. Recommended next step

A good next feature would be one of these:

- a `MonoBehaviour` inventory holder so a scene object can own an `Inventory`
- a robotic arm or pickup zone that reacts to `ConveyorItem`
- an order evaluator that compares cooked dishes against a `ClientDefinition`
