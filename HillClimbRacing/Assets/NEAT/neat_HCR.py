import os
import neat
import socket
import json


ACTIVATION_THRESHOLD = 0.5

gen = 0

config = None
population = None


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
    while True:
        data = client_socket.recv(4096*4).decode()
        #print(data)
        parsed_json = json.loads(data)

        if parsed_json["command"] == "Initialize algorithm":
            print("Error: algorithm is initialized")
            exit(1)
        elif parsed_json["command"] == "Create population":
            send_unity_outputs("out")
            print("Error: population is created")
            send_unity_outputs("out")
            exit(1)
        elif parsed_json["command"] == "Evaluate population":
            send_unity_outputs("out")
            return "new"
        elif parsed_json["command"] == "Process individuals data":
            return parsed_json["data"]
        else:
            print("Error: wrong command")
            exit(1)


def parse_unity_data(data):

    cars = []

    if not data:
        return cars
    print(data[0]['Road'])
    for item in data:
        car = Car(
            {'X': float(item['Position']['X']), 'Y': float(item['Position']['Y'])},
            float(item['Rotation']),
            float(item['DistanceToGround']),
            bool(item['IsAlive']),
            float(item['MaxScore']),
            float(item['CurrentScore']),
            float(item['Road'][0]['Y']),
            float(item['Road'][1]['Y']),
            float(item['Road'][2]['Y']),
            float(item['Road'][3]['Y']))
        cars.append(car)
    return cars


def send_unity_outputs(outputs):
    response = outputs.lower()
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


def create_population(config, data):
    pop_size = 10
    if len(data) != 0:
        pop_size = int(data[0])
    config.__setattr__('pop_size', pop_size)
    print(config.__getattribute__('pop_size'))

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
        data = get_unity_data()
        if(data == "new"):
            break
        print("Get data")
        cars = parse_unity_data(data)

        # if not(check_cars_alive(cars)):
        #     break

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
                car.road1,
                car.road2,
                car.road3])

            unity_output = []

            for out in output:
                if out > ACTIVATION_THRESHOLD:
                    unity_output.append(True)
                else:
                    unity_output.append(False)

            unity_outputs.append(unity_output.copy())
        send_unity_outputs(str(unity_outputs))
        print("Send output")


def waiting_for_commands():
    global config, population
    while True:
        data = client_socket.recv(4096*4).decode()

        parsed_json = json.loads(data)

        if parsed_json['command'] == 'Initialize algorithm':
            config = initialize_algorithm()
            print("config created")
            send_unity_outputs("out")
            continue
        elif parsed_json['command'] == 'Create population':
            send_unity_outputs("Population was created")
            if config is not None:
                if population is None:
                    population = create_population(config, parsed_json['data'])
                    print("population created")
                send_unity_outputs("out")
                population.run(eval_genomes, 1)
            else:
                print("Error: config is None")
                exit(1)
        elif parsed_json['command'] == 'Evaluate population':
            print("Error: no genoms to evaluate")
            exit(1)
        elif parsed_json['command'] == 'Process individuals data':
            print("Error: population is None")
            exit(1)
        else:
            print("Error: wrong command")
            exit(1)


server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

host = '127.0.0.1'
port = 12345

server_socket.bind((host, port))

server_socket.listen(1)
print(f"Сервер запущен на {host}:{port}")

client_socket, addr = server_socket.accept()
print(f"Подключение установлено с {addr}")

waiting_for_commands()

winner = population.run(eval_genomes, None)

print('\nBest genome:\n{!s}'.format(winner))


client_socket.close()
    