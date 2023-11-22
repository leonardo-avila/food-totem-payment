# Food Totem Catalog


This is a repository to maintain the payment of Food Totem project. It is a microservice that provides the payments of products bought by customers. It is a REST API, accessible through HTTP requests by the API Gateway configured on another repository.

The complete documentation could be found on the [API Gateway repository](https://github.com/leonardo-avila/food-totem).

Besides that, it has an local version to be used on development environment. It is a simple web application that provides a user interface to manage the payments. It is accessible through a web browser on the address http://localhost:{port}/swagger.

## Requirements

It is a WebAPI project developed on .NET 6. It uses Entity Framework Core to access the database and Swagger to document the API. You can check your .NET version with the following command:
  
  ```bash
  dotnet --version
  ```

Also, you'll need Terraform, Docker and Kubernetes installed on your machine. You can check your Terraform version with the following command:

  ```bash
  terraform --version
  ```

And your Docker version with the following command:

  ```bash
  docker --version
  ```

And your Kubernetes version with the following command:
  
  ```bash
  kubectl version
  ```

## Running the application

The easiest way is using Terraform on the `infra` folder. You can run the following commands:
  
  ```bash
  terraform init
  terraform apply
  ```

It will create a Docker image and deploy it on a Kubernetes cluster. You can check the application logs with the following command:

  ```bash
  kubectl logs -f deployment/food-totem-payment
  ```