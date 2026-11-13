A client-server application designed to monitor the current work activity of employees across an organization in real time.

> **Note:** This application is a functional prototype created as part of a technical assignment and is not intended for commercial use or production deployment.

## Key Features

* **Silent Background Execution:** Automatically registers in Windows startup on first launch and continues running silently in the background when closed.
* **Real-Time Client Tracking:** Lists all clients registered at least once with their `Domain`, `Machine Name`, `IP Address`, and `Username`.
* **User Activity Status:** Tracks and displays each client's last active time.
* **On-Demand Desktop Screenshots:** Allows administrators to capture and view a current screenshot of any connected client's desktop.

## Tech Stack

The solution is built using official Microsoft technologies with **zero third-party libraries**: `C#; .NET; ASP.NET Core; SignalR; .NET MAUI (Windows);`

## Requirements & Demo

* **Environment:** Windows 10/11, .NET SDK (with `.NET MAUI` and `ASP.NET Core` modules)
* **Video Demo:** [Watch Demo (MP4)](https://cloud.mail.ru/public/anpB/taroG2XFQ)
