variable "defaultRegion" {
  default = "us-east-1"
}

variable "brokerpassword" {
  description = "Secret passed from GitHub Actions"
  type        = string
}

variable "bucketName" {
  description = "Secret passed from GitHub Actions"
  type        = string
}