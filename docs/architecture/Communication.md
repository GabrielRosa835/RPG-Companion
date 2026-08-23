# Communication

The application is split between a centralized monolithic **Host** and an agnostic **Client**.

## Host-Client Architecture
- **Client:** The user's entry point. It is agnostic from the core app (could be web, desktop, or mobile).
- **Communication Protocol:** Communication between the Client and the Host is designed to be handled via **SignalR WebSockets** (pending full implementation).
