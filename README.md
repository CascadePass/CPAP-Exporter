# CPAP Exporter

CPAP Exporter is a flexible, open-source tool that helps users extract and analyze data from CPAP or BiPAP machines. It reads the files saved on the device's SD card and exports the data into **CSV files**, with planned support for **XML**, **JSON**, and other formats in the future.

The application features a **simple, intuitive user interface** that guides users through selecting:
- Which signals to export (e.g., flow rate, leak rate, pressure, etc.).
- Specific nights to pull data from.
- Output preferences (e.g., single file or multiple files).

This level of customization makes it easier for users to analyze their data in tools like Excel or share it with medical professionals. While there are other tools for CPAP data export, none match CPAP Exporter’s flexibility and ease of use.

## Features
- Extracts CPAP/BiPAP data and exports it as CSV files.
- (Planned support for XML, JSON, and more).
- Resample high resolution signals like *Mask Pressure* as needed, allowing users to export any data available in the EDF files.
- Analyzes wave form data to find periods when the user didn't fully stop breathing, but did not breathe well.
- Open source, free to use, with no ads or data collection.

## Getting Started
### Prerequisites
- [Visual Studio](https://visualstudio.microsoft.com/) is necessary to load the `.sln` file and run the application using the code.

### Getting the Code Base
1. Clone the repository:
   ```bash
   git clone https://github.com/CascadePass/CPAP-Exporter
2. Open the .sln file in Visual Studio.

3. Build and run the project.

### Usage
1. Insert your CPAP or BiPAP machine's SD card into your computer, or make the files from the SD card available eg over a network share.

2. Launch CPAP Exporter.

3. Follow the guided steps to:

4. Choose specific nights for data export.

5. Select desired signals (e.g., flow rate, leak rate, pressure, etc.).

6. Configure output file settings (eg one file per night or one file in total).

7. Export the data and analyze it in your favorite tools (e.g., Excel).

### Contributing
Contributions are welcome! There are usually some "[good first issues](https://github.com/CascadePass/CPAP-Exporter/labels/good%20first%20issue)" for new developers, and more challenging problems too.  See [CONTRIBUTING.md](CONTRIBUTING.md) for more details.

License
This project is licensed under the MIT License. See the [LICENSE file](https://github.com/CascadePass/CPAP-Exporter/blob/main/LICENSE) for details.
