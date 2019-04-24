# BullOak.Repositories.EventStore

The EventStore integration project for BullOak

- [BullOak.Repositories.EventStore](#bulloakrepositorieseventstore)
  - [Stream names](#stream-names)
    - [Ids that implement `ISerializeToStreamName`](#ids-that-implement-iserializetostreamname)
    - [Ids that don't implement `ISerializeToStreamName`](#ids-that-dont-implement-iserializetostreamname)

## Stream names

Stream names are generated based on the following rules:

### Ids that implement `ISerializeToStreamName`

If the id of the aggreagte that is to be persisted using BullOak EventStore implements `ISerializeToStreamName` interface then the name of the stream used for persisting the events will be based on the result of the `ISerializeToStreamName.Serialize()` call.
Interface implementation can be found here: `BullOak.Repositories.EventStore/src/BullOak.Repositories.EventStore/ISerializeToStreamName.cs`

### Ids that don't implement `ISerializeToStreamName`

For the ids that don't implement the forementioned interface - the stream name is going to be generated based on the ToString() method

