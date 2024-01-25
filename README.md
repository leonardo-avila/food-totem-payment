# Food Totem Payment
[![build](https://github.com/leonardo-avila/food-totem-payment/actions/workflows/build.yml/badge.svg)](https://github.com/leonardo-avila/food-totem-payment/actions/workflows/build.yml)
[![deploy](https://github.com/leonardo-avila/food-totem-payment/actions/workflows/deploy.yml/badge.svg)](https://github.com/leonardo-avila/food-totem-payment/actions/workflows/deploy.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=leonardo-avila_food-totem-payment&metric=coverage)](https://sonarcloud.io/summary/new_code?id=leonardo-avila_food-totem-payment)

This is a repository to maintain the payment of Food Totem project. It is a microservice that provides the payments of products bought by customers. It is a REST API, accessible through HTTP requests by the API Gateway configured on another repository.

The complete documentation could be found on the [API Gateway repository](https://github.com/leonardo-avila/food-totem).

Besides that, it has an local version to be used on development environment. It is a simple web application that provides a user interface to manage the payments. It is accessible through a web browser on the address http://localhost:3002/swagger.

## Requirements

- .NET 6.0 or above
- Docker

## Running the application

On the root folder:
  
  ```bash
    make run-services
  ```

Be sure to update the environment variables for your Mercado Pago access token and etc. This variables are set on the `src/docker-compose.yaml` file.