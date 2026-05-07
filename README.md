# RouteFinder 📍

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-512BD4?style=flat&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20Windows-blue)

**RouteFinder** is a cross-platform mobile and desktop application built with **.NET MAUI**. It enables users to calculate the most efficient path between points using **Dijkstra's Algorithm**, offering options to find either the quickest or the shortest route.

---

## ✨ Features

* **Smart Routing:** Calculate optimal paths using a weighted graph implementation of Dijkstra's algorithm.
* **Optimization Modes:** Toggle between the **shortest** (distance-based) and **quickest** (time/speed-based) routes.
* **Cross-Platform:** Built with .NET MAUI to run seamlessly on Android, iOS, and Windows from a single codebase.
* **Dynamic Graph Input:** (Assuming manual input or map integration) Add points and connections to visualize the pathfinding process.
* **Modern UI:** Clean and responsive interface designed with XAML.

## 🧠 How it Works

The application represents locations as **nodes** and roads as **edges** in a mathematical graph:
1.  Each edge is assigned a "weight" (Distance or Time).
2.  **Dijkstra's Algorithm** scans the graph from the starting node.
3.  It continuously updates the shortest known distance to every other node until the destination is reached with the lowest possible total weight.

## 🚀 Getting Started

### Prerequisites
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the **.NET MAUI workload** installed.
* [.NET 7.0 or 8.0 SDK](https://dotnet.microsoft.com/download).

### Installation
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/wojciechszymanski-dev/RouteFinder.git](https://github.com/wojciechszymanski-dev/RouteFinder.git)
    ```
2.  **Open the solution:**
    Launch `RouteFinder.sln` in Visual Studio.
3.  **Restore dependencies:**
    Visual Studio will automatically restore the NuGet packages.
4.  **Run the app:**
    Select your target platform (e.g., Windows Machine or Android Emulator) and press **F5**.

## 🛠️ Built With
* **C#** - Core logic and algorithm implementation.
* **.NET MAUI** - Multi-platform App UI framework.
* **XAML** - UI declarative syntax.

## 📂 Project Structure
* `/RouteFinder` - Main project folder containing:
    * `Logic/` - Dijkstra algorithm and Graph data structures.
    * `ViewModels/` - Application logic and data binding.
    * `Views/` - XAML pages for the user interface.

## 📝 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
