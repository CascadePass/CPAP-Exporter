# 🔐 Security Overview

CPAP Exporter is designed with user control, privacy, and transparency in mind. It processes medical device logs locally and does not transmit, store, or share data outside the user’s device.

## 🧭 Core Principles

- **Local-first:** All data processing occurs on the user’s machine.
- **Privacy-respecting:** No telemetry, no logging, no network calls.
- **Minimal dependencies:** External libraries are kept to an absolute minimum.
- **Portable-ready:** CPAP Exporter can be run as a standalone executable.

## 📦 What CPAP Exporter Handles

- **Therapy log files** from CPAP/PAP machines, including:
  - Flow rate
  - Snore index
  - Other session metrics
- **No Personally Identifiable Information (PII)** is accessed or stored.
- Data is translated into CSV format based on user input and saved only to user-specified locations.

## 🛡️ Security Features

- **Offline Operation:** CPAP Exporter performs its functions entirely without an internet connection.
- **No HTTP Calls:** The app does not make outbound or inbound web requests.
- **No Registry Writes:** Registry is accessed only to detect OS theme (light/dark mode) for UI display.
- **Safe Defaults:** The only persistent data written is a settings file stored in `AppData`, which users may delete freely.
- **Controlled Dependency Injection:**
  - Used during testing to improve code quality and reliability.
  - Disabled in release builds to prevent runtime injection or malicious code hooks.
- **No Background Activity:** CPAP Exporter does not operate in the background or monitor system events.

## 💡 User Control

- Users must explicitly choose:
  - Which files to load
  - Which data to export
  - Where to save the output
- Export destinations may include local folders or network locations, but CPAP Exporter will not access network paths unless directed.

## 🧰 Developer Notes

- **Libraries used:**
  - Binary parser for CPAP log format
  - Newtonsoft.Json for settings serialization
- **UI components are custom-built** (e.g. ToggleSwitch) to avoid external UI dependencies.
- **No plugin architecture** in release builds; all functionality is baked-in and self-contained.

## ✅ Trust & Transparency

We believe privacy and simplicity go hand-in-hand. CPAP Exporter was developed with the ethos of “no surprises”: what you see is what you get, and everything that happens is in your control.

For questions, contributions, or suggestions, feel free to [open an issue](https://github.com/YourRepoLink/issues) or explore the repository.
