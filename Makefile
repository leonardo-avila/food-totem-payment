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
	cd src; docker-compose build --no-cache;
	cd src; docker-compose up -d

stop-services:
	cd src; docker-compose down