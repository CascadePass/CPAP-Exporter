# CPAP Exporter

CPAP Exporter is a flexible, open-source tool that helps users extract and analyze data from CPAP or BiPAP machines. It reads the files saved on the device's SD card and exports the data into **CSV files**, with planned support for **XML**, **JSON**, and other formats in the future.

The application features an **intuitive user interface** that guides users through selecting:
- Which signals to export (e.g., flow rate, leak rate, pressure, etc.).
- Specific nights to pull data from.
- Output preferences (e.g., single file or multiple files).

This level of customization makes it easier for users to analyze their data in tools like Excel or share it with medical professionals. While there are other tools for CPAP data export, none match CPAP Exporter’s flexibility and ease of use.

## Features
- Extracts CPAP/BiPAP data and exports it as CSV files.
- Planned support for XML, JSON, and more.
- Intuitive user interface for selecting data and structuring exports.
- Open source, free to use, with no ads or data collection.

## Getting Started
### Prerequisites
- [Visual Studio](https://visualstudio.microsoft.com/) (to load the `.sln` file and run the application).

### Installation of Code Base
1. Clone the repository:
   ```bash
   git clone https://github.com/CascadePass/CPAP-Exporter
2. Open the .sln file in Visual Studio.

3. Build and run the project.

> Note: An installer is not yet available, but this is planned for future releases.

### Usage
1. Insert your CPAP or BiPAP machine's SD card into your computer.

2. Launch CPAP Exporter.

3. Follow the guided steps to:

4. Select desired signals (e.g., flow rate, leak rate, pressure, etc.).

5. Choose specific nights for data export.

6. Configure output file settings.

7. Export the data and analyze it in your favorite tools (e.g., Excel).

### Contributing
Contributions are welcome! Follow the standard Git workflow:

1. Fork the repository.

2. Create a new branch for your feature or bug fix.

3. Submit a pull request when you're ready for review.
Please ensure your contributions align with the project’s goals and include proper documentation.

4. Smaller, easily reviewed pull requests are preferred.

License
This project is licensed under the MIT License. See the [LICENSE file](https://github.com/CascadePass/CPAP-Exporter/blob/main/LICENSE) for details.
