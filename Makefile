sonar-scan:
	dotnet sonarscanner begin /k:"food-totem-payment" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="sqp_47242035fd87a1ee6d26c1dcf4a2b3e56a13791c" /d:sonar.cs.opencover.reportsPaths="**\TestResults\*\*.xml"
	dotnet clean
	dotnet build
	dotnet test --no-build --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
	dotnet sonarscanner end /d:sonar.token="sqp_47242035fd87a1ee6d26c1dcf4a2b3e56a13791c"

test:
	dotnet clean
	dotnet build
	dotnet test --no-build --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

full-clean:
	find . -type d -name "bin" -o -name "obj" -o -name "TestResults" | xargs rm -rf
	dotnet clean

run-services:
	cd infra/local; docker-compose build --no-cache;
	cd infra/local; docker-compose up -d

run-database:
	cd infra/local;	docker-compose up -d payment-database

run-api:
	cd infra/local; docker-compose up -d payment-api

stop-services:
	cd infra/local; docker-compose down