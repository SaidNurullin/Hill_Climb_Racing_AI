import socket
import json

# Создаем сокет
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Указываем хост и порт для прослушивания
host = '127.0.0.1'  # Локальный адрес
port = 12345  # Произвольный порт

# Связываем сокет с хостом и портом
server_socket.bind((host, port))

# Начинаем прослушивать входящие соединения
server_socket.listen(1)
print(f"Сервер запущен на {host}:{port}")

# Принимаем соединение
client_socket, addr = server_socket.accept()
print(f"Подключение установлено с {addr}")

while True:
    # Принимаем данные от Unity
    data = client_socket.recv(4096).decode()

    if not data:
        break

    if len(data) > 30:
        # Load the JSON data
        parsed_json = json.loads(data)

        # Access and print the JSON data
        print(parsed_json[0]['Position'])

    # Отправляем ответ обратно
    response = "Привет, Unity! Я Python-сервер."
    client_socket.send(response.encode())

# Закрываем соединение
client_socket.close()
