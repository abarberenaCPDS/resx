resource "azurerm_resource_group" "res-0" {
  location = var.location
  name     = var.rg_name
}

resource "azurerm_api_management" "res-1" {
  location            = var.location
  name                = var.apim_mgmt_name
  publisher_email     = var.api_management_instance_publisher_email
  publisher_name      = var.api_management_instance_publisher_name
  resource_group_name = azurerm_resource_group.res-0.name
  sku_name            = var.api_management_instance_sku
  depends_on = [
    azurerm_resource_group.res-0,
  ]
}

resource "azurerm_api_management_api" "res-2" {
  api_management_name = azurerm_api_management.res-1.name
  name                = var.api_name
  resource_group_name = azurerm_resource_group.res-0.name
  display_name        = "Echo API"
  service_url         = var.backend_service_url
  protocols           = ["https"]
  revision            = var.api_revision
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_api_operation" "res-3" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  description         = "A demonstration of a POST call based on the echo backend above. The request body is expected to contain JSON-formatted data (see example below). A policy is used to automatically transform any request sent in JSON directly to XML. In a real-world scenario this could be used to enable modern clients to speak to a legacy backend."
  display_name        = "Create resource"
  method              = "POST"
  operation_id        = "create-resource"
  resource_group_name = azurerm_resource_group.res-0.name
  url_template        = "/resource"
  response {
    status_code = 200
  }
  depends_on = [
    azurerm_api_management_api.res-2,
  ]
}

resource "azurerm_api_management_api_operation_policy" "res-4" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  operation_id        = "create-resource"
  resource_group_name = azurerm_resource_group.res-0.name

  xml_content = <<XML
  <policies>
    <inbound>
        <base />
        <json-to-xml apply="always" consider-accept-header="false" />
    </inbound>
    <backend>
        <base />
    </backend>
    <outbound>
        <base />
    </outbound>
    <on-error>
        <base />
    </on-error>
  </policies>
  XML

  depends_on = [
    azurerm_api_management_api_operation.res-3,
  ]
}

resource "azurerm_api_management_api_operation" "res-5" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  description         = "A demonstration of a PUT call handled by the same \"echo\" backend as above. You can now specify a request body in addition to headers and it will be returned as well."
  display_name        = "Modify Resource"
  method              = "PUT"
  operation_id        = "modify-resource"
  resource_group_name = azurerm_resource_group.res-0.name
  url_template        = "/resource"
  response {
    status_code = 200
  }
  depends_on = [
    azurerm_api_management_api.res-2,
  ]
}
resource "azurerm_api_management_api_operation" "res-6" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  description         = "A demonstration of a DELETE call which traditionally deletes the resource. It is based on the same \"echo\" backend as in all other operations so nothing is actually deleted."
  display_name        = "Remove resource"
  method              = "DELETE"
  operation_id        = "remove-resource"
  resource_group_name = azurerm_resource_group.res-0.name
  url_template        = "/resource"
  response {
    status_code = 200
  }
  depends_on = [
    azurerm_api_management_api.res-2,
  ]
}

resource "azurerm_api_management_api_operation" "res-7" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  description         = "The HEAD operation returns only headers. In this demonstration a policy is used to set additional headers when the response is returned and to enable JSONP."
  display_name        = "Retrieve header only"
  method              = "HEAD"
  operation_id        = "retrieve-header-only"
  resource_group_name = azurerm_resource_group.res-0.name
  url_template        = "/resource"
  response {
    status_code = 200
  }
  depends_on = [
    azurerm_api_management_api.res-2,
  ]
}

resource "azurerm_api_management_api_operation_policy" "res-8" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  operation_id        = "retrieve-header-only"
  resource_group_name = azurerm_resource_group.res-0.name

  xml_content = <<XML
  <policies>
    <inbound>
        <base />
    </inbound>
    <backend>
        <base />
    </backend>
    <outbound>
        <base />
        <set-header name="X-My-Sample" exists-action="override">
            <value>This is a sample</value>
        </set-header>
        <jsonp callback-parameter-name="ProcessResponse" />
    </outbound>
    <on-error>
        <base />
    </on-error>
  </policies>
  XML

  depends_on = [
    azurerm_api_management_api_operation.res-7,
  ]
}

resource "azurerm_api_management_api_operation" "res-9" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  description         = "A demonstration of a GET call on a sample resource. It is handled by an \"echo\" backend which returns a response equal to the request (the supplied headers and body are being returned as received)."
  display_name        = "Retrieve resource"
  method              = "GET"
  operation_id        = "retrieve-resource"
  resource_group_name = azurerm_resource_group.res-0.name
  url_template        = "/resource"
  response {
    description = "Returned in all cases."
    status_code = 200
  }
  depends_on = [
    azurerm_api_management_api.res-2,
  ]
}

resource "azurerm_api_management_api_operation" "res-10" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  description         = "A demonstration of a GET call with caching enabled on the same \"echo\" backend as above. Cache TTL is set to 1 hour. When you make the first request the headers you supplied will be cached. Subsequent calls will return the same headers as the first time even if you change them in your request."
  display_name        = "Retrieve resource (cached)"
  method              = "GET"
  operation_id        = "retrieve-resource-cached"
  resource_group_name = azurerm_resource_group.res-0.name
  url_template        = "/resource-cached"
  response {
    status_code = 200
  }
  depends_on = [
    azurerm_api_management_api.res-2,
  ]
}

resource "azurerm_api_management_api_operation_policy" "res-11" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  operation_id        = "retrieve-resource-cached"
  resource_group_name = azurerm_resource_group.res-0.name

  xml_content = <<XML
  <policies>
      <inbound>
          <base />
          <cache-lookup vary-by-developer="false" vary-by-developer-groups="false" downstream-caching-type="none">
              <vary-by-header>Accept</vary-by-header>
              <vary-by-header>Accept-Charset</vary-by-header>
          </cache-lookup>
          <rewrite-uri template="/resource" />
      </inbound>
      <backend>
          <base />
      </backend>
      <outbound>
          <base />
          <cache-store duration="3600" />
      </outbound>
      <on-error>
          <base />
      </on-error>
  </policies>
  XML

  depends_on = [
    azurerm_api_management_api_operation.res-10,
  ]
}

resource "azurerm_api_management_policy" "res-24" {
  api_management_id = azurerm_api_management.res-1.id
  xml_content       = "<!--\r\n    IMPORTANT:\r\n    - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n    - Only the <forward-request> policy element can appear within the <backend> section element.\r\n    - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n    - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n    - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n    - To remove a policy, delete the corresponding policy statement from the policy document.\r\n    - Policies are applied in the order of their appearance, from the top down.\r\n-->\r\n<policies>\r\n\t<inbound />\r\n\t<backend>\r\n\t\t<forward-request />\r\n\t</backend>\r\n\t<outbound />\r\n</policies>"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_product" "res-29" {
  api_management_name = azurerm_api_management.res-1.name
  description         = "Subscribers will be able to run 5 calls/minute up to a maximum of 100 calls/week."
  display_name        = "Starter"
  product_id          = "starter"
  published           = true
  resource_group_name = azurerm_resource_group.res-0.name
  subscriptions_limit = 1
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_product_api" "res-30" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  product_id          = azurerm_api_management_product.res-29.product_id
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_api_management_product.res-29,
  ]
}

resource "azurerm_api_management_product_group" "res-32" {
  api_management_name = azurerm_api_management.res-1.name
  group_name          = "developers"
  product_id          = azurerm_api_management_product.res-29.product_id
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_api_management_product.res-29,
  ]
}

resource "azurerm_api_management_product_group" "res-33" {
  api_management_name = azurerm_api_management.res-1.name
  group_name          = "guests"
  product_id          = azurerm_api_management_product.res-29.product_id
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_api_management_product.res-29,
  ]
}

resource "azurerm_api_management_product_policy" "res-34" {
  api_management_name = azurerm_api_management.res-1.name
  product_id          = azurerm_api_management_product.res-29.product_id
  resource_group_name = azurerm_resource_group.res-0.name

  xml_content = <<XML
  <policies>
      <inbound>
          <rate-limit calls="5" renewal-period="60" />
          <quota calls="100" renewal-period="604800" />
          <base />
      </inbound>
      <backend>
          <base />
      </backend>
      <outbound>
          <base />
      </outbound>
      <on-error>
          <base />
      </on-error>
  </policies>
  XML

  depends_on = [
    azurerm_api_management_product.res-29,
  ]
}

resource "azurerm_api_management_product" "res-35" {
  api_management_name = azurerm_api_management.res-1.name
  approval_required   = true
  description         = "Subscribers have completely unlimited access to the API. Administrator approval is required."
  display_name        = "Unlimited"
  product_id          = "unlimited"
  published           = true
  resource_group_name = azurerm_resource_group.res-0.name
  subscriptions_limit = 1
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_product_api" "res-36" {
  api_management_name = azurerm_api_management.res-1.name
  api_name            = var.api_name
  product_id          = azurerm_api_management_product.res-35.product_id
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_api_management_product.res-35,
  ]
}

resource "azurerm_api_management_product_group" "res-38" {
  api_management_name = azurerm_api_management.res-1.name
  group_name          = "developers"
  product_id          = azurerm_api_management_product.res-35.product_id
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_api_management_product.res-35,
  ]
}

resource "azurerm_api_management_product_group" "res-39" {
  api_management_name = azurerm_api_management.res-1.name
  group_name          = "guests"
  product_id          = azurerm_api_management_product.res-35.product_id
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_api_management_product.res-35,
  ]
}

resource "azurerm_api_management_email_template" "res-43" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          On behalf of $OrganizationName and our customers we thank you for giving us a try. Your $OrganizationName API account is now closed.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Your $OrganizationName Team</p>\r\n    <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n    <p />\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Thank you for using the $OrganizationName API!"
  template_name       = "AccountClosedDeveloper"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-44" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          We are happy to let you know that your request to publish the $AppName application in the application gallery has been approved. Your application has been published and can be viewed <a href=\"http://$DevPortalUrl/Applications/Details/$AppId\">here</a>.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your application $AppName is published in the application gallery"
  template_name       = "ApplicationApprovedNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-45" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you for joining the $OrganizationName API program! We host a growing number of cool APIs and strive to provide an awesome experience for API developers.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">First order of business is to activate your account and get you going. To that end, please click on the following link:</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a id=\"confirmUrl\" href=\"$ConfirmUrl\" style=\"text-decoration:none\">\r\n              <strong>$ConfirmUrl</strong>\r\n            </a>\r\n          </p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">If clicking the link does not work, please copy-and-paste or re-type it into your browser's address bar and hit \"Enter\".</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Please confirm your new $OrganizationName API account"
  template_name       = "ConfirmSignUpIdentityDefault"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-46" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">You are receiving this email because you made a change to the email address on your $OrganizationName API account.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Please click on the following link to confirm the change:</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a id=\"confirmUrl\" href=\"$ConfirmUrl\" style=\"text-decoration:none\">\r\n              <strong>$ConfirmUrl</strong>\r\n            </a>\r\n          </p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">If clicking the link does not work, please copy-and-paste or re-type it into your browser's address bar and hit \"Enter\".</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Please confirm the new email associated with your $OrganizationName API account"
  template_name       = "EmailChangeIdentityDefault"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}
resource "azurerm_api_management_email_template" "res-47" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Your account has been created. Please follow the link below to visit the $OrganizationName developer portal and claim it:\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <a href=\"$ConfirmUrl\">$ConfirmUrl</a>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "You are invited to join the $OrganizationName developer network"
  template_name       = "InviteUserNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-48" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">This is a brief note to let you know that $CommenterFirstName $CommenterLastName made the following comment on the issue $IssueName you created:</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">$CommentText</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          To view the issue on the developer portal click <a href=\"http://$DevPortalUrl/issues/$IssueId\">here</a>.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "$IssueName issue has a new comment"
  template_name       = "NewCommentNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-49" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <h1 style=\"color:#000505;font-size:18pt;font-family:'Segoe UI'\">\r\n          Welcome to <span style=\"color:#003363\">$OrganizationName API!</span></h1>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Your $OrganizationName API program registration is completed and we are thrilled to have you as a customer. Here are a few important bits of information for your reference:</p>\r\n    <table width=\"100%\" style=\"margin:20px 0\">\r\n      <tr>\r\n            #if ($IdentityProvider == \"Basic\")\r\n            <td width=\"50%\" style=\"height:40px;vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n              Please use the following <strong>username</strong> when signing into any of the $${OrganizationName}-hosted developer portals:\r\n            </td><td style=\"vertical-align:top;font-family:'Segoe UI';font-size:12pt\"><strong>$DevUsername</strong></td>\r\n            #else\r\n            <td width=\"50%\" style=\"height:40px;vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n              Please use the following <strong>$IdentityProvider account</strong> when signing into any of the $${OrganizationName}-hosted developer portals:\r\n            </td><td style=\"vertical-align:top;font-family:'Segoe UI';font-size:12pt\"><strong>$DevUsername</strong></td>            \r\n            #end\r\n          </tr>\r\n      <tr>\r\n        <td style=\"height:40px;vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n              We will direct all communications to the following <strong>email address</strong>:\r\n            </td>\r\n        <td style=\"vertical-align:top;font-family:'Segoe UI';font-size:12pt\">\r\n          <a href=\"mailto:$DevEmail\" style=\"text-decoration:none\">\r\n            <strong>$DevEmail</strong>\r\n          </a>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best of luck in your API pursuits!</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <a href=\"http://$DevPortalUrl\">$DevPortalUrl</a>\r\n    </p>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Welcome to the $OrganizationName API!"
  template_name       = "NewDeveloperNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-50" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you for contacting us. Our API team will review your issue and get back to you soon.</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Click this <a href=\"http://$DevPortalUrl/issues/$IssueId\">link</a> to view or edit your request.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Best,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your request $IssueName was received"
  template_name       = "NewIssueNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-51" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">The password of your $OrganizationName API account has been reset, per your request.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n                Your new password is: <strong>$DevPassword</strong></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Please make sure to change it next time you sign in.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your password was reset"
  template_name       = "PasswordResetByAdminNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-52" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <title>Letter</title>\r\n  </head>\r\n  <body>\r\n    <table width=\"100%\">\r\n      <tr>\r\n        <td>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\"></p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">You are receiving this email because you requested to change the password on your $OrganizationName API account.</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Please click on the link below and follow instructions to create your new password:</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a id=\"resetUrl\" href=\"$ConfirmUrl\" style=\"text-decoration:none\">\r\n              <strong>$ConfirmUrl</strong>\r\n            </a>\r\n          </p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">If clicking the link does not work, please copy-and-paste or re-type it into your browser's address bar and hit \"Enter\".</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">$OrganizationName API Team</p>\r\n          <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n          </p>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your password change request"
  template_name       = "PasswordResetIdentityDefault"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-53" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Greetings $DevFirstName $DevLastName!</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Thank you for subscribing to the <a href=\"http://$DevPortalUrl/products/$ProdId\"><strong>$ProdName</strong></a> and welcome to the $OrganizationName developer community. We are delighted to have you as part of the team and are looking forward to the amazing applications you will build using our API!\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Below are a few subscription details for your reference:</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <ul>\r\n            #if ($SubStartDate != \"\")\r\n            <li style=\"font-size:12pt;font-family:'Segoe UI'\">Start date: $SubStartDate</li>\r\n            #end\r\n            \r\n            #if ($SubTerm != \"\")\r\n            <li style=\"font-size:12pt;font-family:'Segoe UI'\">Subscription term: $SubTerm</li>\r\n            #end\r\n          </ul>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n            Visit the developer <a href=\"http://$DevPortalUrl/developer\">profile area</a> to manage your subscription and subscription keys\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">A couple of pointers to help get you started:</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <strong>\r\n        <a href=\"http://$DevPortalUrl/docs/services?product=$ProdId\">Learn about the API</a>\r\n      </strong>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The API documentation provides all information necessary to make a request and to process a response. Code samples are provided per API operation in a variety of languages. Moreover, an interactive console allows making API calls directly from the developer portal without writing any code.</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <strong>\r\n        <a href=\"http://$DevPortalUrl/applications\">Feature your app in the app gallery</a>\r\n      </strong>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">You can publish your application on our gallery for increased visibility to potential new users.</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n      <strong>\r\n        <a href=\"http://$DevPortalUrl/issues\">Stay in touch</a>\r\n      </strong>\r\n    </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          If you have an issue, a question, a suggestion, a request, or if you just want to tell us something, go to the <a href=\"http://$DevPortalUrl/issues\">Issues</a> page on the developer portal and create a new topic.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Happy hacking,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n    <a style=\"font-size:12pt;font-family:'Segoe UI'\" href=\"http://$DevPortalUrl\">$DevPortalUrl</a>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your subscription to the $ProdName"
  template_name       = "PurchaseDeveloperNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-54" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head>\r\n    <style>\r\n          body {font-size:12pt; font-family:\"Segoe UI\",\"Segoe WP\",\"Tahoma\",\"Arial\",\"sans-serif\";}\r\n          .alert { color: red; }\r\n          .child1 { padding-left: 20px; }\r\n          .child2 { padding-left: 40px; }\r\n          .number { text-align: right; }\r\n          .text { text-align: left; }\r\n          th, td { padding: 4px 10px; min-width: 100px; }\r\n          th { background-color: #DDDDDD;}\r\n        </style>\r\n  </head>\r\n  <body>\r\n    <p>Greetings $DevFirstName $DevLastName!</p>\r\n    <p>\r\n          You are approaching the quota limit on you subscription to the <strong>$ProdName</strong> product (primary key $SubPrimaryKey).\r\n          #if ($QuotaResetDate != \"\")\r\n          This quota will be renewed on $QuotaResetDate.\r\n          #else\r\n          This quota will not be renewed.\r\n          #end\r\n        </p>\r\n    <p>Below are details on quota usage for the subscription:</p>\r\n    <p>\r\n      <table>\r\n        <thead>\r\n          <th class=\"text\">Quota Scope</th>\r\n          <th class=\"number\">Calls</th>\r\n          <th class=\"number\">Call Quota</th>\r\n          <th class=\"number\">Bandwidth</th>\r\n          <th class=\"number\">Bandwidth Quota</th>\r\n        </thead>\r\n        <tbody>\r\n          <tr>\r\n            <td class=\"text\">Subscription</td>\r\n            <td class=\"number\">\r\n                  #if ($CallsAlert == true)\r\n                  <span class=\"alert\">$Calls</span>\r\n                  #else\r\n                  $Calls\r\n                  #end\r\n                </td>\r\n            <td class=\"number\">$CallQuota</td>\r\n            <td class=\"number\">\r\n                  #if ($BandwidthAlert == true)\r\n                  <span class=\"alert\">$Bandwidth</span>\r\n                  #else\r\n                  $Bandwidth\r\n                  #end\r\n                </td>\r\n            <td class=\"number\">$BandwidthQuota</td>\r\n          </tr>\r\n              #foreach ($api in $Apis)\r\n              <tr><td class=\"child1 text\">API: $api.Name</td><td class=\"number\">\r\n                  #if ($api.CallsAlert == true)\r\n                  <span class=\"alert\">$api.Calls</span>\r\n                  #else\r\n                  $api.Calls\r\n                  #end\r\n                </td><td class=\"number\">$api.CallQuota</td><td class=\"number\">\r\n                  #if ($api.BandwidthAlert == true)\r\n                  <span class=\"alert\">$api.Bandwidth</span>\r\n                  #else\r\n                  $api.Bandwidth\r\n                  #end\r\n                </td><td class=\"number\">$api.BandwidthQuota</td></tr>\r\n              #foreach ($operation in $api.Operations)\r\n              <tr><td class=\"child2 text\">Operation: $operation.Name</td><td class=\"number\">\r\n                  #if ($operation.CallsAlert == true)\r\n                  <span class=\"alert\">$operation.Calls</span>\r\n                  #else\r\n                  $operation.Calls\r\n                  #end\r\n                </td><td class=\"number\">$operation.CallQuota</td><td class=\"number\">\r\n                  #if ($operation.BandwidthAlert == true)\r\n                  <span class=\"alert\">$operation.Bandwidth</span>\r\n                  #else\r\n                  $operation.Bandwidth\r\n                  #end\r\n                </td><td class=\"number\">$operation.BandwidthQuota</td></tr>\r\n              #end\r\n              #end\r\n            </tbody>\r\n      </table>\r\n    </p>\r\n    <p>Thank you,</p>\r\n    <p>$OrganizationName API Team</p>\r\n    <a href=\"$DevPortalUrl\">$DevPortalUrl</a>\r\n    <p />\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "You are approaching an API quota limit"
  template_name       = "QuotaLimitApproachingDeveloperNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-55" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          We would like to inform you that we reviewed your subscription request for the <strong>$ProdName</strong>.\r\n        </p>\r\n        #if ($SubDeclineReason == \"\")\r\n        <p style=\"font-size:12pt;font-family:'Segoe UI'\">Regretfully, we were unable to approve it, as subscriptions are temporarily suspended at this time.</p>\r\n        #else\r\n        <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Regretfully, we were unable to approve it at this time for the following reason:\r\n          <div style=\"margin-left: 1.5em;\"> $SubDeclineReason </div></p>\r\n        #end\r\n        <p style=\"font-size:12pt;font-family:'Segoe UI'\"> We truly appreciate your interest. </p><p style=\"font-size:12pt;font-family:'Segoe UI'\">All the best,</p><p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p><a style=\"font-size:12pt;font-family:'Segoe UI'\" href=\"http://$DevPortalUrl\">$DevPortalUrl</a></body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your subscription request for the $ProdName"
  template_name       = "RejectDeveloperNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}

resource "azurerm_api_management_email_template" "res-56" {
  api_management_name = azurerm_api_management.res-1.name
  body                = "<!DOCTYPE html >\r\n<html>\r\n  <head />\r\n  <body>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Dear $DevFirstName $DevLastName,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          Thank you for your interest in our <strong>$ProdName</strong> API product!\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">\r\n          We were delighted to receive your subscription request. We will promptly review it and get back to you at <strong>$DevEmail</strong>.\r\n        </p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">Thank you,</p>\r\n    <p style=\"font-size:12pt;font-family:'Segoe UI'\">The $OrganizationName API Team</p>\r\n    <a style=\"font-size:12pt;font-family:'Segoe UI'\" href=\"http://$DevPortalUrl\">$DevPortalUrl</a>\r\n  </body>\r\n</html>"
  resource_group_name = azurerm_resource_group.res-0.name
  subject             = "Your subscription request for the $ProdName"
  template_name       = "RequestDeveloperNotificationMessage"
  depends_on = [
    azurerm_api_management.res-1,
  ]
}