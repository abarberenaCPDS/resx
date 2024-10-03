output "api_management_instance_id" {
  description = "ID of the Azure API Management instance"
  value       = azurerm_api_management.res-1.id
}

output "api_management_name" {
    value = azurerm_api_management.res-1.name
}

output "backend_service_url" {
  description = "URL of the backend service used by the API"
  value       = var.backend_service_url
}

output "full_api_name" {
  description = "Full name of the API including revision"
  value       = local.full_api_name  # Using local if defined
}
