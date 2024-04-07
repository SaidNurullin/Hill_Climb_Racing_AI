import os
import neat
import socket
import json


ACTIVATION_THRESHOLD = 0.5


class Car:
    def __init__(self,
                 position, rotation, distance_to_ground, is_alive,
                 max_score, current_score, road1, road2, road3, road4):
        self.position = position
        self.rotation = rotation
        self.distance_to_ground = distance_to_ground
        self.is_alive = is_alive
        self.max_score = max_score
        self.current_score = current_score
        self.road1 = road1
        self.road2 = road2
        self.road3 = road3
        self.road4 = road4


def get_unity_data():

    data = client_socket.recv(4096).decode()

    cars = []

    if not data:
        return cars

    parsed_json = json.loads(data)

    for item in parsed_json:
        car = Car(
            item['Position'],
            item['Rotation'],
            item['DistanceToGround'],
            item['IsAlive'],
            item['MaxScore'],
            item['CurrentScore'],
            item['Road'][0],
            item['Road'][1],
            item['Road'][2],
            item['Road'][3])
        cars.append(car)
    return cars


def send_unity_outputs(outputs):
    response = "Привет, Unity! Я Python-сервер."
    client_socket.send(response.encode())


def check_cars_alive(cars):
    count = 0
    for car in cars:
        if not(car.is_alive):
            count += 1
    if count == len(cars):
        return False
    else:
        return True


def initialize_algorithm():
    local_dir = os.path.dirname(__file__)
    config_path = os.path.join(local_dir, 'config-feedforward.txt')
    config = neat.config.Config(neat.DefaultGenome, neat.DefaultReproduction,
                                neat.DefaultSpeciesSet, neat.DefaultStagnation,
                                config_path)
    return config


def create_population(config):

    p = neat.Population(config)

    p.add_reporter(neat.StdOutReporter(True))
    stats = neat.StatisticsReporter()
    p.add_reporter(stats)

    return p


def eval_genomes(genomes, config):

    global gen
    gen += 1

    nets = []
    cars = []
    ge = []

    for genome_id, genome in genomes:
        genome.fitness = 0
        net = neat.nn.FeedForwardNetwork.create(genome, config)
        nets.append(net)
        ge.append(genome)

    run = True
    while run:
        cars = get_unity_data()

        if not(check_cars_alive(cars)):
            break

        unity_outputs = []

        for x, car in enumerate(cars):
            if not(car.is_alive):
                unity_outputs.append([False, False])
                continue

            ge[x].fitness = car.current_score

            output = nets[cars.index(car)].activate([
                car.position['Y'],
                car.rotation,
                car.distance_to_ground,
                car.road1['Y'],
                car.road2['Y'],
                car.road3['Y']])

            unity_output = []

            for out in output:
                if out > ACTIVATION_THRESHOLD:
                    unity_output.append(True)
                else:
                    unity_output.append(False)

            unity_outputs.append(unity_output.copy())
        send_unity_outputs(unity_outputs)


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

gen = 0

config = None
population = None

while True:
    # Принимаем данные от Unity
    data = client_socket.recv(4096).decode()

    if not data:
        break

    if data == 'Initialize algorithm':
        config = initialize_algorithm()
    elif data == 'Create population':
        population = create_population(config)
        break
    else:
        exit(1)

    response = "Привет, Unity! Я Python-сервер."
    client_socket.send(response.encode())

winner = population.run(eval_genomes, 1)

print('\nBest genome:\n{!s}'.format(winner))

# Закрываем соединение
client_socket.close()
