# RPG-Companion Principles

This document outlines the core principles and philosophy driving the development of the RPG-Companion project.

## 1. Tooling First
RPG-Companion is designed as a **tool**, not a replacement for the players. Its primary goal is to assist users in running their TTRPG sessions, managing the heavy lifting of rules, numbers, and text, without outright playing the game for them.

## 2. Automation and Persistence
TTRPG systems are notoriously dense. The framework aims to automate rule application and securely persist relevant data to streamline gameplay and reduce cognitive load for players and Game Masters.

## 3. Flexibility for "House Rules"
TTRPGs are inherently flexible. The automation provided must be adaptable enough to accommodate custom interpretations and modifications to rules (house rules).

## 4. Extensibility through Plugins
To support unparalleled flexibility, the core application serves as a **framework**. It enables developers to create and attach custom plugins to a host server, expanding or altering the available features to suit specific game systems or styles.

## 5. UI-Agnostic Server-Client Architecture
The logic of the game runs on a primary **host server**. Connected clients (whether Web, mobile, CLI, etc.) should have their UI remain completely agnostic to the underlying logic being executed.

## 6. "Give Them the Tools..."
Inspired by the GNU philosophy: *"Give them the tools and let them decide how to use them."* The framework provides numerous granular tools with necessary security boundaries. It is the responsibility of plugins to compose these tools together to provide meaningful, useful services to the users.
