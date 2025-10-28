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

# Technical Questions
1. How much time did you spend on this task?
If you had more time, what improvements or additions would you make?

---- Total Time: 2-3 hours

The implementation can be broken down as follows:

Basic API Integration (30-45 minutes):
	Calling OpenWeather APIs (Weather, GeoCoding, Air Quality)
	Simple response handling and JSON deserialization
	Basic endpoint setup

Architecture & Refactoring (1-1.5 hours):
	Implementing Clean Architecture principles
	Applying SOLID design patterns
	Creating service interfaces and dependency injection
	Structured error handling with ResultStatus
	Tehran hardcoding logic in GeoCoding service

Comprehensive Testing (45-60 minutes):
	Writing unit tests for all services
	Mocking HTTP clients and dependencies
	Achieving >90% code coverage
	Testing edge cases and error scenarios

Note!: If the requirement was just “call the APIs and return responses,” it would take less than 1 hour. However, following best practices with proper architecture, clean code, and comprehensive testing extended the timeline to ensure production-ready quality.

---- If i had more time:
 Advanced Caching Strategy
	Implement Redis/In-Memory caching for frequently requested cities
	Add cache invalidation policies based on data freshness requirements
	Reduce API calls and improve response times

 Structured Logging & Monitoring
	Integrate Serilog with structured logging
	Add Application Insights or centralized logging (ELK stack)
	Implement health checks and metrics endpoints

 Integration Tests
	Add integration tests with TestServer
	Mock external API responses using WireMock
	Test end-to-end scenarios and API contracts

 Rate Limiting & Resilience
	Implement Polly for retry policies and circuit breakers
	Add rate limiting middleware to prevent API abuse
	Handle OpenWeather API rate limits gracefully

 Enhanced Error Handling
	Implement global exception handling middleware
	Add custom exception types for domain-specific errors
	Provide detailed error responses with correlation IDs

 Performance Optimization
	Implement parallel API calls where applicable
	Add response compression
	Optimize JSON serialization settings

 Background Jobs
	Schedule periodic weather data updates using Hangfire
	Implement job queues for batch processing
	Add support for recurring tasks

2. What is the most useful feature recently added to your favorite programming language?
Please include a code snippet to demonstrate how you use it.

A powerful and lightweight technique available in ASP.NET Core (since .NET 8) is Streaming Responses using IAsyncEnumerable<T>. This feature allows an endpoint to stream data to the client over a standard HTTP GET request as soon as data chunks are available, rather than buffering the entire response.

How It Works
	The mechanism leverages a standard HTTP feature called Chunked Transfer Encoding.

Server-Side: 
	An ASP.NET Core Minimal API endpoint is defined with a return type of IAsyncEnumerable<T>. Inside the method, data is produced and sent to the client using yield return. Each yield return statement sends a new chunk of data immediately. The framework automatically sets the Transfer-Encoding: chunked header.

Client-Side: 
	The client (e.g., a web browser) uses the Fetch API to make a GET request. It then gets a ReadableStream from the response body and processes each chunk of data as it arrives, providing a real-time user experience.

Key Advantages
	Reduced Latency: The client receives and can display the first piece of data almost instantly, without waiting for the entire server-side process to complete. This is ideal for long-running queries or reports.
	Low Server Memory Footprint: The server doesn’t need to hold the entire dataset in memory. It generates and streams data on the fly, making it highly efficient for large datasets.
	Simplicity & Compatibility: It relies on a standard HTTP GET request, making it simpler than setting up a persistent, two-way WebSocket connection (like SignalR). Any HTTP client can consume the stream without special libraries.

Server-Side (ASP.NET Core Minimal API)
	// In Program.cs
	var builder = WebApplication.CreateBuilder(args);
	var app = builder.Build();

	// This endpoint streams data to the client.
	app.MapGet("/stream-data", async IAsyncEnumerable<object> () =>
	{
    	for (int i = 1; i <= 10; i++)
    	{
        	// Simulate a delay (e.g., a database query)
        	await Task.Delay(1000); 

        	// 'yield return' sends the data chunk to the client immediately.
        	yield return new { Item = i, Timestamp = DateTime.UtcNow };
    	}
	});

	app.Run();

3. How do you identify and diagnose a performance issue in a production environment?
Have you done this before?

My Approach:

	- Monitor & Alert: Use Application Insights/ELK to identify slow endpoints, high response times, or increased error rates
	- Reproduce Locally: Try to replicate the issue in staging with similar load/data
	- Analyze Logs: Check structured logs for exceptions, slow queries, or external API timeouts
	- Profile the Code: Use profiling tools (dotMemory, dotTrace) to identify bottlenecks
	- Check Dependencies: Verify database query performance, external API latency, and network issues
	- Apply Fixes: Optimize queries, add caching, or implement async patterns
	- Load Test: Validate the fix under production-like load before deployment
	- Have I done this before?

Yes, I’ve diagnosed performance issues in production environments. Common scenarios I’ve handled include:

	- Identifying N+1 query problems causing database bottlenecks
	- Finding memory leaks through profiling tools
	- Optimizing slow API endpoints by adding caching layers
	- Resolving timeout issues with external service calls
	- The key is to combine monitoring tools, structured logging, and systematic investigation to pinpoint the root cause quickly.

4. What’s the last technical book you read or technical conference you attended?
What did you learn from it?

	I haven’t attended a technical conference recently, but I actively learn through I regularly follow .NET docs and tutorials to stay updated on best practices

5. What’s your opinion about this technical test?
It’s a fair but basic test that evaluates fundamentals—calling APIs, dependency injection, and unit testing.

	The challenge wasn’t technical complexity (it’s straightforward HTTP calls), but rather deciding the scope: should I spend 30 minutes on a quick implementation, or 2-3 hours on production-ready code with Clean Architecture and >90% test coverage?
	I chose the latter to demonstrate my standards
	
6. Please describe yourself using JSON format.


	{
	"person": {
		"name": "Iman Ahmadpour",
		"version": "2.0.beta",
		"status": "actively_learning",
		"current_quest": "AI Graduate Studies",
		
		"education": {
		"undergraduate": {
			"degree": "Computer Science",
			"university": "Kharazmi University",
			"status": "completed"
		},
		"graduate": {
			"field": "Artificial Intelligence",
			"university": "Islamic Azad University, Science And Research Branch",
			"status": "in_progress",
			"excitement_level": "very_high"
		}
		},
		
		"personality_traits": [
		"intensely_curious",
		"continuous_learner",
		"story_driven"
		],
		
		"interests": {
		"gaming": [
			"narrative-driven games", 
			"story-rich experiences"
		],
		"reading": [
			"philosophy", 
			"fantasy"
		],
		"tech": [
			"AI/ML", 
			"microservice architecture", 
			"scalable systems"
		]
		},
		
		"technical_stack": {
		"core_expertise": {
			"languages": [
			"C#", 
			"Python", 
			"Go", 
			"Java", 
			"JavaScript"
			],
			"frameworks": [
			"ASP.NET Core", 
			".NET Framework"
			],
			"architecture": [
			"Clean Architecture", 
			"Microservices", 
			"RESTful APIs"
			]
		},
		
		"backend_skills": {
			"databases": [
			"SQL Server", 
			"MySQL", 
			"PostgreSQL", 
			"Redis"
			],
			"messaging": [
			"RabbitMQ", 
			"Kafka"
			],
			"real_time": [
			"SignalR"
			],
			"orm": [
			"Entity Framework"
			],
			"integrations": [
			"Payment Gateways (PGW)", 
			"SMS with Pattern", 
			"Financial Transactions"
			]
		},
		
		"quality_assurance": {
			"testing": [
			"Unit Tests", 
			"Integration Tests", 
			"Load Testing", 
			"Stress Testing"
			],
			"tools": [
			"K6", 
			"xUnit", 
			"NUnit"
			]
		},
		
		"devops": {
			"containerization": [
			"Docker"
			],
			"ci_cd": [
			"GitHub Actions (self-hosted & cloud runners)"
			],
			"deployment": [
			"automated pipelines"
			]
		},
		
		"security": {
			"authentication": [
			"JWT"
			],
			"authorization": [
			"Role-Based Access Control (RBAC)"
			]
		},
		
		"methodologies": [
			"Agile", 
			"Scrum", 
			"Test-Driven Development"
		]
		},
		
		"experience_highlights": [
		{
			"achievement": "Built accounting & treasury systems for industrial zones",
			"impact": "high",
			"complexity": "enterprise_level"
		},
		{
			"achievement": "Migrating monolith to microservices architecture",
			"status": "in_progress",
			"technologies_used": [
			"Docker", 
			"RabbitMQ", 
			"Kafka", 
			"Redis"
			]
		},
		{
			"achievement": "Implemented CI/CD pipelines with self-hosted runners",
			"automation_level": "full"
		}
		],
		
		"currently_exploring": [
		"AI/ML integration with backend systems",
		"Advanced distributed systems patterns",
		"Scalability & performance optimization"
		],
		
		"contact": {
		"status": "open_to_opportunities",
		"preferred_role": "Backend Developer | AI Engineer",
		"availability": "ready_to_contribute"
		}
	}
	}
