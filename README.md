# IP Geolocation API Service
A .NET Core API service for IP geolocation lookup and country-based access control.

## Features
-  IP address geolocation lookup
- Country-based IP blocking
- Request logging

## Technologies
- .NET 8
- ASP.NET Core Web API
- HttpClient
- JSON serialization

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code
- IP Geolocation API key (from [ipgeolocation.io](https://ipgeolocation.io))

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/E_Technology_Task.git
   cd E_Technology_Task
   ```
2. Configure the application
  Add your API key to appsettings.json:
```
   "IpApi": {
    "BaseUrl": "https://api.ipgeolocation.io/ipgeo",
    "ApiKey": "your_api_key_here"
  }
```
3. Endpoints
   
![image](https://github.com/user-attachments/assets/4431c289-cab8-48fd-be9c-7b18549de58e)
