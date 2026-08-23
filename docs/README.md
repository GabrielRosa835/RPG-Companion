# Documentation Overview

This directory (`docs/`) serves as the central knowledge base for the RPG-Companion project.

## Structure
- **`Principles.md`**: Outlines the core philosophy and goals driving the development of the framework.
- **`architecture/`**: Contains markdown documents describing the core technical components and abstractions of the application (e.g., Events, Plugins, Storage).
- **`sessions/`**: Contains chronological tracking of work sessions, decisions made, and developments.

## Session Summaries
- **2026-08-22 Initial Documentation**: Established the core agent rules in `GEMINI.md`, set up the documentation structure, and created the initial architectural component documents.
- **2026-08-22 Project Architecture Principles**: Established the core principles guiding the architecture of the platform, emphasizing flexibility, extensibility (plugins), and a UI-agnostic server-client model.
- **2026-08-22 Signal API Design**: Brainstormed and defined the DX and architecture for the new non-blocking Signal API.
- **2026-08-23 Rules API Design**: Discussed and established the Developer Experience (DX) for the Rules API, including its functional pipeline composition and the Railway-Oriented Programming pattern using `RuleResult<T>` with Exception allocations for deep expressive trees.
