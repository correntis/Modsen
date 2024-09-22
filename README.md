# Тестовое задание для стажировки в Modsen


## Инструкция для запуска

Для запуска проекта необходим установленный `Docker Desktop`. При старте базы данных она автоматически заполняется начальными данными о книгах, авторах и пользователях.


### Запуск контейнеров
1. Клонируйте репозиторий в любую папку.
```bash
    git clone https://github.com/correntis/Modsen.git
```
2. Откройте терминал и перейдите в папку с репозиторием.
```bash
    cd {cloneDirectory}/Modsen
```
3. Запустите сборку с помощью Docker Compose:
```
    docker-compose up --build
```
4. Дождитесь загрузки и запуска всех контейнеров (_обратите внимание, что library-api должен запуститься последним_).

### Доступные адреса
1. [Swagger](http://localhost:5000/swagger/index.html)
```yml
    http://localhost:5000/swagger/index.html
```

2. [Frontend](http://localhost:4200)
```yml
    http://localhost:4200
```

### Пользователи по умолчанию

1. Администратор:
    - Email: admin@example.com
    - Пароль: admin
2. Пользователь:
    - Email: user@example.com
    - Пароль: user
