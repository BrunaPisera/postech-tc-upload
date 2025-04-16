terraform {
  backend "s3" {
    bucket = "tc-tf-backend"
    key    = "backend/terraform_upload.tfstate"
    region = "us-east-1"
  }
}