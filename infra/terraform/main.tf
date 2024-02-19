terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
    }
  }
  required_version = ">= 1.3.7"
  backend "s3" {
    bucket                  = "terraform-buckets-food-totem"
    key                     = "food-totem-payment/terraform.tfstate"
    region                  = "us-west-2"
  }
}
provider "aws" {
    region = var.lab_account_region
}

data "aws_security_group" "default" {
  name = "default"
}

data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

data "aws_lb" "rabbitmq_lb" {
  name = "rabbitmq-lb"
}

resource "aws_lb_target_group" "payment-mongodb-tg" {
  name     = "payment-mongodb"
  port     = 27017
  protocol = "TCP"
  vpc_id   = data.aws_vpc.default.id
  target_type = "ip"
}

resource "aws_lb" "payment-mongodb-lb" {
  name               = "payment-mongodb"
  internal           = true
  load_balancer_type = "network"
  subnets            = data.aws_subnets.default.ids
}

resource "aws_lb_listener" "payment-mongodb-lbl" {
  load_balancer_arn = aws_lb.payment-mongodb-lb.arn
  port              = 27017
  protocol          = "TCP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.payment-mongodb-tg.arn
  }
}

resource "aws_lb_target_group" "payment-api-tg" {
  name     = "payment-api"
  port     = 80
  protocol = "HTTP"
  vpc_id   = data.aws_vpc.default.id
  target_type = "ip"

  health_check {
    enabled = true
    interval = 300
    path = "/health-check"
    protocol = "HTTP"
    timeout = 60
    healthy_threshold = 3
    unhealthy_threshold = 3
  }
}

resource "aws_lb" "payment-api-lb" {
  name               = "payment-api"
  internal           = true
  load_balancer_type = "application"
  subnets            = data.aws_subnets.default.ids
}

resource "aws_lb_listener" "payment-api-lbl" {
  load_balancer_arn = aws_lb.payment-api-lb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.payment-api-tg.arn
  }
}

resource "aws_ecs_task_definition" "food-totem-payment-mongodb-task" {
  family                   = "food-totem-payment-mongodb"
  network_mode             = "awsvpc"
  execution_role_arn       = "arn:aws:iam::${var.lab_account_id}:role/LabRole"
  cpu                      = 256
  memory                   = 512
  requires_compatibilities = ["FARGATE"]
  container_definitions    = jsonencode([
    {
        "name": "food-totem-payment-mongodb",
        "image": var.mongo_image,
        "essential": true,
        "portMappings": [
            {
              "containerPort": 27017,
              "hostPort": 27017,
              "protocol": "tcp"
            }
        ],
        "environment": [
            {
                "name": "MONGO_INITDB_ROOT_USERNAME",
                "value": var.mongo_root_user
            },
            {
                "name": "MONGO_INITDB_ROOT_PASSWORD",
                "value": var.mongo_root_password
            }
        ],
        "cpu": 256,
        "memory": 512,
        "logConfiguration": {
            "logDriver": "awslogs",
            "options": {
                "awslogs-group": "food-totem-payment-mongodb-logs",
                "awslogs-region": var.lab_account_region,
                "awslogs-stream-prefix": "food-totem-payment-mongodb"
            }
        }
    }
  ])
}

resource "aws_ecs_task_definition" "food-totem-payment-task" {
  depends_on = [ aws_ecs_task_definition.food-totem-payment-mongodb-task ]
  family                   = "food-totem-payment"
  network_mode             = "awsvpc"
  execution_role_arn       = "arn:aws:iam::${var.lab_account_id}:role/LabRole"
  cpu                      = 256
  memory                   = 512
  requires_compatibilities = ["FARGATE"]
  container_definitions    = jsonencode([
    {
        "name": "food-totem-payment",
        "image": var.food_totem_payment_image,
        "essential": true,
        "portMappings": [
            {
              "containerPort": 80,
              "hostPort": 80,
              "protocol": "tcp"
            }
        ],
        "environment": [
            {
                "name": "PaymentDatabaseSettings__ConnectionString",
                "value": join("", ["mongodb://", var.mongo_root_user, ":", var.mongo_root_password, "@", aws_lb.payment-mongodb-lb.dns_name, ":27017"])
            },
            {
                "name": "RabbitMQ__HostName",
                "value": data.aws_lb.rabbitmq_lb.dns_name
            },
            {
                "name": "RabbitMQ__UserName",
                "value": var.rabbitMQ_user
            },
            {
                "name": "RabbitMQ__Password",
                "value": var.rabbitMQ_password
            }
        ],
        "cpu": 256,
        "memory": 512,
        "logConfiguration": {
            "logDriver": "awslogs",
            "options": {
                "awslogs-group": "food-totem-payment-logs",
                "awslogs-region": var.lab_account_region,
                "awslogs-stream-prefix": "food-totem-payment"
            }
        }
    }
  ])
}

resource "aws_ecs_service" "food-totem-payment-mongodb-service" {
  name            = "food-totem-payment-mongodb-service"
  cluster         = "food-totem-ecs"
  task_definition = aws_ecs_task_definition.food-totem-payment-mongodb-task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    security_groups  = [data.aws_security_group.default.id]
    subnets = data.aws_subnets.default.ids
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.payment-mongodb-tg.arn
    container_name   = "food-totem-payment-mongodb"
    container_port   = 27017
  }

  health_check_grace_period_seconds = 120
}

resource "aws_ecs_service" "food-totem-payment-service" {
  name            = "food-totem-payment-service"
  cluster         = "food-totem-ecs"
  task_definition = aws_ecs_task_definition.food-totem-payment-task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    security_groups  = [data.aws_security_group.default.id]
    subnets = data.aws_subnets.default.ids
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.payment-api-tg.arn
    container_name   = "food-totem-payment"
    container_port   = 80
  }

  health_check_grace_period_seconds = 120
}

resource "aws_cloudwatch_log_group" "food-totem-payment-logs" {
  name = "food-totem-payment-logs"
  retention_in_days = 1
}

resource "aws_cloudwatch_log_group" "food-totem-payment-mongodb-logs" {
  name = "food-totem-payment-mongodb-logs"
  retention_in_days = 1
}

