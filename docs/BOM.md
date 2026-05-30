# OnAir — Bill of Materials (BOM)

This is a high-level BOM for the OnAir wireless indicator PCB. Quantities listed are per-board (1 unit). Part numbers are *suggestions* — I recommend verifying footprints and exact vendor SKUs before ordering.

| Ref | Qty | Part / Description | Purpose | Footprint / Notes |
|-----|-----:|--------------------|---------|-------------------|
| U1 | 1 | ESP32 module (ESP32-WROOM-32) OR XBee-style ZigBee module OR nRF52 module | Primary radio / MCU (populate one) | Module footprint (castellated) — ESP32-WROOM recommended for Wi‑Fi + BLE |
| U2 | 1 | USB-C receptacle (power-only or full 5-pin) | 5V power input / programming | USB-C side-entry SMT or through-hole; include CC resistor network if full USB-C support required |
| U3 | 1 | 3.3V regulator (LDO) e.g. MCP1700-33, LD1117-3.3 | Generate 3.3V rail from 5V USB | SOT-223 or SOT-23-5 depending on chosen part; check dropout vs battery use |
| U4 | 1 | LiPo charger IC or module footprint (optional) e.g. MCP73831 or TP4056 module | Optional battery charging (single-cell LiPo) | Add JST PH 2-pin battery connector if used |
| Q1 | 1 | N-channel MOSFET (logic-level) e.g. BSS138 / IRLML2502 / AOZ small MOSFET | Drive external lamp (5–12V) or high-current LED | SOT-23 or SOT-23-3 footprint; choose MOSFET with Rds(on) suited to load |
| D1 | 1 | Schottky diode (optional) e.g. BAT54S | Reverse protection / power ORing | SOD‑523 or SOD‑123 footprint depending on current rating |
| F1 | 1 | Ferrite bead / power choke | EMI / noise suppression on 5V | 0805 or 0603 ferrite bead |
| C1, C2, C3 | 3 | Decoupling capacitors (0.1uF, 10uF, 22uF) | Power decoupling / bulk | 0805/0603 depending on space |
| R1 | 1 | USB-C CC pull resistor (if implementing USB-C CC detection for power role) | USB-C CC configuration | 5.1k / 56k depending on role — follow USB‑C spec |
| J1 | 1 | JST PH 2-pin (BAT+) | LiPo battery connector (optional) | JST PH series, 2-pin, 2.0mm pitch |
| J2 | 1 | JST PH 2/3-pin (LAMP_OUT + GND [+V if separate]) | External lamp / LED connector | JST PH 2–3 pin depending on lamp wiring |
| LED_PWR | 1 | 0805 LED (power indicator) | Power/status indicator | Add resistor sized for 3.3V rail |
| LED_STAT | 1 | 0805 LED (status) | Network/paired indicator | Optional |
| H1 | 1 | 4-pin programming header / UART pads | Serial flash / debug / esptool | 2x2 1.27mm or 1.27/2.54mm pads depending on preference; add RTS/DTR if ESP32 auto-flash desired |
| SW1 | 1 | Tactile push button | Reset / user button | 6x6mm or 4x4mm footprint |
| TP1..TP4 | 4 | Test pads / castellated pads | GPIO access / programming / measurements | small circular test pads or castellations |
| MISC | — | Screws / standoffs, mounting hardware | Mechanical | 2x M2.5 mounting holes recommended |

Notes and procurement tips
- Radio module: decide which radio to prioritize early (ESP32 vs XBee vs nRF52). ESP32 gives Wi‑Fi + BLE in one module and is easiest to program and integrate with webhooks/cloud.
- USB-C: if you only need power input, a power-only USB‑C receptacle is simplest. If you want native USB serial for flashing, include the full 5-pin receptacle and CC resistors, or add an external USB‑to‑UART bridge (CP2102/FTDI) footprint.
- Battery: for LiPo support add a single-cell charger IC and a JST battery connector plus protection if needed.
- Lamp drive: if you will drive a 12V studio lamp, use an N‑channel MOSFET on the high side with appropriate gate driver or use a low-side MOSFET with the lamp between +12V and MOSFET drain; include a flyback diode if driving inductive loads.
- Footprints: I will create the KiCad schematic and attach matching footprints; confirm the exact module product before ordering PCBs to ensure pinout/outline match.

Want a CSV BOM for ordering (with supplier part numbers and suggested quantities)? I can generate a CSV and attempt to populate Digikey/Mouser SKUs for each suggested part — tell me whether you prefer Mouser or DigiKey and whether I should include 10% overage for prototype orders.