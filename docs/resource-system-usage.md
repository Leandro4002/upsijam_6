# Resource System Usage Guide

This project now has a small shared foundation for:

- resource definitions
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

## 4. Client definitions

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

## 5. Inventory

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

## 6. Typical workflow

A simple way to use the system is:

1. Create several `ResourceDefinition` assets
2. Create several `ClientDefinition` assets
3. Put `ResourceDefinition` references on harvestable world objects
4. When the player collects something, call `Inventory.AddResource()`
5. When a client order starts, read the data from a `ClientDefinition`
6. Later, compare the cooked dish against the client's requested ratios

---

## 7. What this system does not do yet

The current foundation does **not** include:

- cooking evaluation
- spawning resources
- client AI
- UI
- saving/loading

Those systems can now be built on top of these shared classes.

---

## 8. Recommended next step

A good next feature would be one of these:

- a `MonoBehaviour` inventory holder so a scene object can own an `Inventory`
- a collectable resource component that references a `ResourceDefinition`
- an order evaluator that compares cooked dishes against a `ClientDefinition`
