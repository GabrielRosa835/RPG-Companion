# Storage and Persistence

The RPG-Companion emphasizes infrastructure agnosticism through the Plugin Development Kit (PDK).

## Core Implementation
- **Store:** The underlying database is currently MongoDB, but it is entirely wrapped by the PDK so that plugins do not depend on it directly.
- **Entities:** The PDK accepts any object type for storage. However, types explicitly defined as "entities" gain an `Id` and broader capabilities, including fetching by Id and include-relationships.
