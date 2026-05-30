# OnAir PCB design — Overview

This document outlines a PCB design for the OnAir wireless indicator. It covers recommended radio options, BOM, functional blocks, footprint suggestions, and next steps for KiCad implementation.

Goals
- Replace USB-attached Arduino variant with a compact PCB that supports wirelessly-controlled LED indicator (external lamp or integrated LED).
- Support ZigBee, Wi-Fi (ESP32), and Bluetooth (BLE) via modular radio footprints.
- Make the board USB-C powered by default, with optional LiPo battery + charger for portable operation.
- Keep 3.3V logic and an easy programming/header for flashing firmware.

Functional blocks
- Power: USB-C 5V input, reverse-polarity protection, 3.3V regulator (MCP1700/ME6211 or similar), optional LiPo charger (MCP73831 or TP4056 module footprint) and LiPo JST connector.
- MCU / radio modules:
  - ESP32 Type: ESP32-WROOM-32 footprint (SMD module, integrated Wi-Fi + BLE) OR
  - ZigBee Type: XBee-style SMD/through-hole footprint (or SMD with castellated edges) OR
  - nRF52 type: footprint for modules like the nRF52832 or nRF52840 for BLE-focused builds.
- LED driver / lamp connector: MOSFET or transistor to drive external 12V/5V lamp if required; for small LED use a GPIO + resistor. JST connector to power external lamp.
- Headers: SWD / JTAG / UART programming header, or USB-to-UART circuit for on-board programming.
- Indicator LEDs: power, status, and optional signal strength/paired LEDs.

BOM (high level)
- MCU modules: ESP32-WROOM-32 or XBee module or nRF52 module
- LDO 3.3V regulator (e.g., MCP1700-33)
- USB-C receptacle (5-pin) + CC resistor network (for host/device role as USB power in) OR use a USB-C power-only connector footprint
- JST PH 2-pin for LiPo battery + JST 2-pin for lamp out
- MOSFET (e.g., AOZ1284 or IRLML6344) for lamp switching
- Decoupling capacitors, ferrite bead, ESD diodes as usual

Mechanical
- Board size target: 50x30 mm (configurable)
- Mounting holes: two M2.5 standoffs

Footprint recommendations
- Provide both ESP32-WROOM and XBee-style footprints on the board but populate only one per unit.
- Add 4-pin castellated programming pads for direct module flashing if needed.

Firmware considerations
- Build modular firmware that supports multiple radio types via build flags. If ESP32 is chosen, implement Wi-Fi + HTTP/webhook client and mDNS/UDP discovery. If ZigBee is chosen, implement ZigBee network join/leave and a simple endpoint for LED control.

Next steps (KiCad)
1. Create pcb/OnAir.kicad_pro project skeleton and copy this document as a design note.
2. Draft schematic with the chosen radio footprint (or both) and power circuitry.
3. Create PCB board outline and place footprints with clearance for USB-C.
4. Produce Gerber/Drill files and BOM for prototype order (e.g., JLCPCB).

