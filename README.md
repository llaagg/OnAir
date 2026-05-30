# OnAir

This repository contains the OnAir "On Air" indicator project. Historically it used a USB/Serial-attached Arduino (pin 13 LED) to toggle an LED. This branch adds a wireless-forward design and PCB plan so the device can be operated over ZigBee, Wi‑Fi, or Bluetooth.

Quick status
- Branch: remote (feature: wireless/PCB)
- New docs: docs/PCB_DESIGN.md (wireless options, BOM, schematic notes, PCB recommendations)

Wireless options (overview)
- ZigBee (XBee-compatible): low-power mesh friendly, good range and interoperability with existing ZigBee networks. Good for multi-device setups where robust mesh routing matters.
- Wi‑Fi (ESP32/ESP8266): easiest to integrate with existing home networks and cloud/webhooks. Higher power draw but broadband and direct cloud connectivity.
- Bluetooth/BLE (nRF52 or ESP32): low-power direct control from phone or computer; BLE can be convenient for phone apps but has limited range compared to Wi‑Fi/ZigBee.

Recommended approach
- Make the PCB a modular carrier that supports interchangeable radio modules: an ESP32 module footprint (Wi‑Fi + BLE) and an XBee-style footprint for ZigBee. Only populate the module you need per unit.
- Provide 3.3V power rail, USB-C power input, optional LiPo battery + charger for portable installs, and a JST connector for the external LED / lamp.

Where to find the PCB design
- See docs/PCB_DESIGN.md for the full design, BOM, block diagram, footprints, and next steps (KiCad recommended).

Developer notes / next actions
1. Review docs/PCB_DESIGN.md and tell me which wireless tech you want prioritized (ZigBee, Wi‑Fi, or Bluetooth) and whether you want battery-powered units or mains/USB only.
2. After you confirm, I will:
   - Add a KiCad project skeleton under pcb/ and create the schematic & board files.
   - Produce a BOM and fabrication files for prototype PCBs.
   - Iterate on the PCB layout and add footprints for mechanical mounting.

Build / flash notes (ESP32 example)
- If you use ESP32 (recommended for Wi‑Fi/BLE): flash firmware via USB using esptool or the Arduino IDE. The ESP32 module can be programmed on the carrier board via a USB‑to‑serial or direct USB-C interface if the schematic includes an auto‑programming circuit (RTS/DTR).

License & credits
- See LICENSE for project license. PCB design files generated later will be added under an appropriate license section.

Images
- Original Arduino photos: docs/blue.png, docs/yellow_on.png, docs/yellow_off.png

