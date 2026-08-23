# Session: Project Architecture Principles
**Date:** 2026-08-22
**Purpose:** Define the core architectural principles and the overarching goal of the RPG-Companion platform.

## Decisions & Actions Taken
1. **Defined Core Principles:** Synthesized the primary objectives and philosophical foundations for the project. 
   - Focused on the application being a "tool" rather than a replacement for players.
   - Emphasized automation of rules and data persistence while maintaining flexibility for "house rules".
   - Solidified the need for an extensible plugin framework and a UI-agnostic server-client model.
   - Adopted a GNU-like "Give them the tools..." mentality for feature development and plugin integration.

2. **Created `Principles.md`:** Consolidated these decisions into a new documentation file `docs/Principles.md` for permanent reference.

3. **Updated `README.md`:** Updated the docs overview to include the newly created principles file and this session log.

4. **Documented `Questions` Architecture:** Analyzed the `IQuestion` delegation API and authored a comprehensive guide in `docs/architecture/Questions.md`. Highlighted the use of Policies (Target, Secrecy, Blocking) and `IResponseSchema` to adhere to the UI-Agnostic principles, and the role of Context objects for scoped DI and ambient data access.

5. **Documented `Events` Architecture:** Analyzed and updated `docs/architecture/Events.md` detailing the State Machine-like execution of TTRPG rules. Covered the surface abstractions (`Event` lifecycle, `IEventContext` resolutions) and the host implementation (`EventEngine`, execution intervals, pipeline management, and `EventResult` pattern matching).## Next Steps
- Begin laying out the technical architecture of the plugin system to align with these newly defined principles.
- Outline the communication protocol between the host server and agnostic clients.
