Feature: StreamNameRules
  In order to correlate entity id with the stream name
  As a library user
  I expect stream names to be generated based on the id provided
  
  Scenario: Save events to a stream based on explicit interface
    Given a new Id explicitly implementing ISerializeToStreamName
    When I try to save the new events for that id
    Then the explicitly named event stream should exist and contain saved data

  Scenario: Save events to a stream based on ToString 
    Given a new Id that doesn't implement ISerializeToStreamName
    When I try to save the new events for that id
    Then the ToString based event stream should exist and contain saved data
