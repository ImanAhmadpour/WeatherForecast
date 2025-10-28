# WeatherForecast

# API Key Configuration
You need to obtain your own OpenWeather API key and configure it in the project.

Step 1: Get Your API Key
 - Visit OpenWeather API
 - Sign up for a free account
 - Generate your API key from the dashboard

Step 2: Configure the API Key
	Option 1: Using Command Line 

	For Linux/macOS:
	cd API
	dotnet user-secrets init  # if not already done
	dotnet user-secrets set "OpenWeather:ApiKey" "YOUR_API_KEY_HERE"

	For Windows (PowerShell):
	cd API
	dotnet user-secrets init  # if not already done
	dotnet user-secrets set "OpenWeather:ApiKey" "YOUR_API_KEY_HERE"

	Option 2: Using Visual Studio (Manual)

	Right-click on the API project in Solution Explorer
	Select Manage User Secrets
	Add the following configuration:
	{
  	 "OpenWeather": {
    	 "ApiKey": "YOUR_API_KEY_HERE"
  	 }
	}

# Project Architecture
  This project follows Clean Architecture principles and SOLID design patterns:

 - Separation of Concerns: Clear separation between API, Services, and Models layers
 - Dependency Injection: Loose coupling through interfaces (IGeoCodingService, IWeatherService)
 - Single Responsibility: Each service handles a specific domain (GeoCoding, Weather, Air Quality)
 - Testability: Comprehensive unit tests with >90% code coverage
 - Structured Error Handling: Consistent result types using ResultStatus and ResultStatusInfo

  The architecture ensures maintainability, scalability, and adherence to industry best practices.

# Testing & Code Coverage
  The project includes comprehensive unit tests with over 90% code coverage, covering:

 - Service layer logic (GeoCoding, Weather, Air Quality)
 - Error handling and edge cases
 - Result status propagation
 - HTTP client mocking and network failure scenarios

