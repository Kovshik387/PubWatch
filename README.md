# Мониторинг данных из публичных источников

## Запуск

1. Клонирование репозитория `git@github.com:Kovshik387/PubWatch.git`

2. Запуск docker контейнеров `cd deploy && docker-compose -f docker-compose.base.yaml -f docker-compose.override.yaml up`

3. Откройте localhost в вашем браузере

---

## Используемые технологии

- [ASP.NET Core](https://dotnet.microsoft.com/ru-ru/apps/aspnet)
- [Nginx](https://nginx.org/ru/)
- [Docker](https://www.docker.com/)
- [Redis](https://redis.io/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Minio](https://min.io/)
- [PostgreSQL](https://www.postgresql.org/)

---

## Архитектура приложения

![Архитектура](docs/schema.png)

### Контакты

- Telegram [@yrulewet](https://t.me/yrulewet)
