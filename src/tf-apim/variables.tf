variable "location" {
  type = string
  default = "westus"
}

variable "rg_name" {
  type = string
  default = "rg-pathway-net-azb-wus-77"
}

variable "apim_mgmt_name" {
  type = string
  description = "Name of the Azure API Management instance"
  default = "apim-pathway-net-azb-wus-77"
}

variable "api_name" {
  type = string
  description = "Name of the API within API Management"
  default = "echo-api"
}

variable "backend_service_url" {
  description = "URL of the backend service"
  type        = string
  default     = "http://echoapi.cloudapp.net/api"
}

variable "api_revision" {
  description = "Revision of the API"
  type        = string
  default     = "1"
}

variable "api_management_instance_publisher_email" {
  description = "Email Address of the publisher"
  type        = string
  default     = ""
}

variable "api_management_instance_publisher_name" {
  description = "Email Address of the publisher"
  type        = string
  default     = "Software"
}

variable "api_management_instance_sku" {
  description = "The pricing SKU tier"
  type        = string
  default     = "Developer_1"
}



# Locals example (if needed)
locals {
  full_api_name = format("%s-%s", var.api_name, var.api_revision)
}

