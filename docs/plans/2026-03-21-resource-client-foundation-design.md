# Resource and Client Foundation Design

**Date:** 2026-03-21

## Goal

Create a small set of shared base classes for a Unity game jam project so resource collection, client orders, and inventory systems can be developed in parallel.

## Recommended Architecture

Use a hybrid model:

- `ScriptableObject` assets for reusable authored definitions
- serializable C# classes for nested value objects shown in the inspector
- runtime classes for mutable play-session state such as inventory contents
- scene `GameObject`s only for visible world objects, not for core data storage

This keeps data reusable, editor-friendly, and independent from scene setup.

## Core Data Model

### `CriterionType`
Enum listing the supported criteria:

- `Mohs`
- `Scoville`
- `PH`

This enum is expected to grow later.

### `CriterionValue`
Serializable value object representing a criterion and its integer value.

Fields:
- `CriterionType Criterion`
- `int Value`

Used by resources.

### `CriterionRatio`
Serializable value object representing a criterion and its target ratio.

Fields:
- `CriterionType Criterion`
- `float Ratio`

Used by clients.

### `ResourceDefinition`
A `ScriptableObject` asset representing a type of collectable resource.

Fields:
- display name
- optional prefab reference for world visuals
- `int Size`
- list of `CriterionValue`

Purpose:
- author a reusable resource template in the Unity inspector
- allow collection and cooking systems to reference one shared definition

### `ClientDefinition`
A `ScriptableObject` asset representing a type of monster client.

Fields:
- display name
- optional prefab reference for visuals or portrait use later
- `int QuantityRequested`
- list of `CriterionRatio`

Purpose:
- author reusable client order templates
- let order systems instantiate runtime requests from data assets

### `InventoryEntry`
Runtime serializable class that stores:
- `ResourceDefinition Resource`
- `int Quantity`

Purpose:
- represent one stack of a resource in inventory

### `Inventory`
Runtime class or Unity component that owns a list of `InventoryEntry` objects and exposes basic operations:
- `AddResource()`
- `RemoveResource()`
- `GetQuantity()`
- `HasResource()`
- read-only access to entries

Purpose:
- centralize resource tracking
- provide a stable shared API for collection, cooking, and UI systems

## Data Flow

1. A world object references a `ResourceDefinition`.
2. When collected, the inventory adds quantity for that definition.
3. A client system references a `ClientDefinition`.
4. When an order starts, runtime logic reads the quantity request and criterion ratios from the definition.
5. A future cooking/evaluation system compares the cooked dish against those requested ratios.

## Visuals Recommendation

Include optional prefab fields now on `ResourceDefinition` and `ClientDefinition`.

Do not turn the core data classes into scene objects.

Reason:
- gameplay logic should work even before models are ready
- prefabs can be added later without changing the data contract
- data assets remain reusable across scenes and systems

## Suggested Script Layout

- `Scripts/Data/`
  - `CriterionType.cs`
  - `CriterionValue.cs`
  - `CriterionRatio.cs`
- `Scripts/Definitions/`
  - `ResourceDefinition.cs`
  - `ClientDefinition.cs`
- `Scripts/Inventory/`
  - `InventoryEntry.cs`
  - `Inventory.cs`

## Scope Boundaries

Include now:
- enum and serializable value types
- resource and client definition assets
- inventory entry and inventory API
- optional prefab references

Do not include yet:
- cooking result evaluation
- spawning logic
- client behavior AI
- UI logic
- save/load systems

## Testing Strategy

Use edit-mode C# tests for inventory behavior first.

Priority behaviors:
- adding a new resource creates an entry
- adding the same resource increases quantity
- removing resources decreases quantity
- removing too much fails safely
- quantity queries return expected values

Definition asset classes mainly need serialization-safe structure and can be validated with lightweight tests later if needed.
