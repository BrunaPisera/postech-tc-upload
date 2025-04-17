<div align="center">
<img src="https://github.com/user-attachments/assets/208a0ebb-ca7c-4b0b-9f68-0b35050a9880" width="30%" />
</div>

# VidProc- Video Upload API (POS TECH: TECH CHALLENGE - FASE FINAL)🚀

Seja bem vindo! Este é um desafio proposto pela PósTech (Fiap + Alura) na ultima fase da pós graduação de Software Architecture (8SOAT).

📼 Vídeo de demonstração do projeto desta fase: em produção

Integrantes do grupo:<br>
Alexis Cesar (RM 356558)<br>
Bruna Gonçalves (RM 356557)

A Video Upload API permite que os usuários façam upload de vídeos, que são armazenados no Amazon S3. Após o upload, o sistema envia uma mensagem para o serviço de processamento, informando que há um novo vídeo para ser processado.

A aplicação é containerizada utilizando Docker, orquestrada por Kubernetes (K8s) para garantir escalabilidade e resiliência, e gerenciada por Helm, que automatiza o deployment e rollbacks no cluster Kubernetes (EKS) na nuvem da AWS.

## Navegação
- [Arquitetura](#arquitetura)
- [Fluxo](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

## Arquitetura

A aplicação segue a Arquitetura Limpa, que promove a separação de responsabilidade, facilitando a manutenção e escalabilidade. Esta abordagem permite que a lógica de negócios principal seja independente de qualquer dependência externa, como bancos de dados ou serviços externos.

## Fluxo
 
1. O usuário envia o vídeo através do endpoint de upload.
2. O vídeo é armazenado no S3.
3. Uma mensagem é enviada para o RabbitMQ, notificando que o vídeo foi enviado e está pronto para processamento.

## Tecnologias Utilizadas
 
- **API Gateway**: Exposição da API.
- **Amazon S3**: Armazenamento de vídeos.
- **RabbitMQ (via CloudAMQP)**: Fila de mensagens para notificar o processamento.
- **Kubernetes**: Orquestração de contêineres.
- **Helm**: Gerenciamento de de pacotes kubernetes.
- **EKS**: Cluster Kubernetes na nuvem da AWS.
- **Terraform**: Automação de criação de recursos em provedores de nuvem.

ℹ️ Este repositório faz parte de um conjunto de repositórios (outros serviços, infraestrutura e banco de dados) que formam o sistema VidProc. Link de todos os repositórios envolvidos:
- [Infraestrutura AWS](https://github.com/BrunaPisera/postech-tc-infra)
- [API de StatusTracking](https://github.com/BrunaPisera/postech-tc-status-tracking-api)
- [API de upload](https://github.com/BrunaPisera/postech-tc-upload)
- [Serviço de Processamento](https://github.com/BrunaPisera/postech-tc-process-service)
- [Banco de Dados](https://github.com/BrunaPisera/postech-tc-db)
