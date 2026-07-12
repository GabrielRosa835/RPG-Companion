### The Event Engine
A StateMachine that can run multiple threads simultaneously, where each state 
defines when the processing should stop or continue to the next state (event).

### The Synchronous Communication
A CQRS and MediatR like approach, where Intent-objects defines payloads and 
result types, and an external handler contains the code to process the payload
and return the desired result. No need for MediatR, though.

### The Asynchronous Communication
SignalR connects and sends each client a light-weight (Guid + URL) object 
indicating what has happened, and a specific endpoint is offered to whose wants 
more data from the operation.

### The Persistence Layer / API

---

The players are going to be deciding between events

Player: \
Action[] -> can come from different sources \
ClientId -> SignalR \
Role -> Plugin dependant \
There must be a single SessionMaster

Action: \
A named intent for some choice \
Defines stopping points for the player to decide before, during (maybe) or 
after an event has been triggered \
They define how the use can interact with the system \
\* Some actions require more data than a simple selection -> payload definition

Session: \
Player[] -> All current playing people with their metadata

=> If the player has an action to intercept an event, it and the DM is prompted
to take or not the action. \
=> Interception points: Before, Started, Finished, After \
=> Otherwise, the engine keeps on going.

---

Endpoints: Rules with an extra CallerContext object that brings in pre-processed 
metadata of the request




Raise -> Pipeline

Context:
    Setup
    Execute:
        Work
    Teardown
    ...

Halt -> Stops the pipeline as a whole (including nesting)
Exit -> Stops the loop naturally without continuing after Teardown
Break -> Simply stops the loop and executes the Teardown
Next -> Queues the next event
