variable "lab_account_id" {
  description = "AWS Labs account ID"
}

variable "lab_account_region" {
  description = "AWS Labs account region"
  default = "us-west-2"
}

variable "food_totem_payment_image" {
  description = "Food Totem Payment Docker image"
}

variable "mongo_image" {
  description = "Mongo Docker image"
}

variable "mongo_root_user" {
  description = "Mongo root user"
}

variable "mongo_root_password" {
  description = "Mongo root password"
}

